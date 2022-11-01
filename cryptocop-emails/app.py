import json
from time import sleep
import pika
from os import environ

import requests


def get_connection_string():
    with open('./config/mb.%s.json' % environ.get('PYTHON_ENV'), 'r') as f:
        return json.load(f)

def connect_to_mb():
    error = False
    connection_string = get_connection_string()
    while not error:
        try:
            credentials = pika.PlainCredentials(connection_string['user'], connection_string['password'])
            connection = pika.BlockingConnection(pika.ConnectionParameters(connection_string['host'], 5672, connection_string['virtualhost'], credentials))
            channel = connection.channel()
            return channel
        except:
            sleep(5)
            continue

channel = connect_to_mb()

exchange_name = 'cryptocop_exchange'
create_order_routing_key = 'create-order'
order_email_queue_name = 'email-queue'

def setup_queue(exchange_name, queue_name, routing_key):
    # Declare the queue, if it doesn't exist
    channel.queue_declare(queue=queue_name, durable=True)
    # Bind the queue to a specific exchange with a routing key
    channel.queue_bind(exchange=exchange_name, queue=queue_name, routing_key=routing_key)

# Declare the exchange, if it doesn't exist
channel.exchange_declare(exchange=exchange_name, exchange_type='topic', durable=False)

setup_queue(exchange_name, order_email_queue_name, create_order_routing_key)

def send_simple_message(to, subject, body):
    return requests.post(
        "https://api.mailgun.net/v3/sandboxf2e2ccda284846c38dff27eac664d868.mailgun.org/messages",
        auth=("api", "81317f299403a6e076ae57913dc2af6c-b0ed5083-92e41ccd"),
        data={"from": "Mailgun Sandbox <postmaster@sandboxf2e2ccda284846c38dff27eac664d868.mailgun.org>",
              "to": to,
              "subject": subject,
              "html": body})

def send_ack(ch, delivery_tag, success):
    if success:
        ch.basic_ack(delivery_tag)

def send_order_email(ch, method, properties, data):
    data = json.loads(data)
    #print(data)
    email = data['email']

    representation = f"""
    <h2>Thank you for ordering @ Cryptocop!</h2>
    <p>We hope you will enjoy our lovely product and don\'t hesitate to contact us if there are any questions.</p>
    <p>Name of Customer: {data['fullName']}</p>
    <p>Street name and number: {data['streetName'] + ' ' + data['houseNumber']}</p>
    <p>City: {data['city']}</p>
    <p>Zip code: {data['zipCode']}</p>
    <p>Country: {data['country']}</p>
    <p>Date of Order: {data['orderDate']}</p>
    <p>Total Price: {data['totalPrice']} USD</p>
    <table>
        <thead>Order Items</thead>
        <tr>
            <th>Name</th>
            <th>Unit price</th>
            <th>Quantity</th>
            <th>Total price</th>
        </tr>
        {''.join(['<tr>' + '<td>'+ item['productIdentifier'] + '</td>' + '<td>'+ item['unitPrice'].__str__() +'</td>' + '<td>'+ item['quantity'].__str__() +'</td>'+ '<td>'+ item['totalPrice'].__str__() +'</td>'+ '</tr>' for item in data['orderItems']])}
    </table>"""
    
    response = send_simple_message(email, 'Successful order!', representation)
    #send_ack(ch, method.delivery_tag, response.ok)

channel.basic_consume(order_email_queue_name,
                      send_order_email, auto_ack=True)


channel.start_consuming()
channel.close()