// See https://aka.ms/new-console-template for more information
using System.Text;
using CreditCardValidator;
using Cryptocop_Payments.Payment_service;
using Cryptocop_Payments.Payment_service.Models;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

string configFileName = "";

if (environment == "Development")
{
    configFileName = "appsettings.Development.json";
}
else if (environment == "Production")
{
    configFileName = "appsettings.json";
}

var configuration = new ConfigurationBuilder()
    .AddJsonFile(configFileName)
    .Build();

var configSection = configuration.GetSection("RabbitMQ");

var host = configSection.GetValue<string>("Host");
var exchange = configSection.GetValue<string>("Exchange");
var queue = configSection.GetValue<string>("Queue");
var routingKey = configSection.GetValue<string>("RoutingKey");

var factory = new ConnectionFactory() { HostName = host };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Topic);

    channel.QueueDeclare(queue: queue, durable: false, exclusive: false, autoDelete: false, arguments: null);

    channel.QueueBind(queue: queue, exchange: exchange, routingKey: routingKey);

    Console.WriteLine(" [*] Wating for orders.");

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var orderStr = Encoding.UTF8.GetString(body);
        var inputModel = JsonSerializerHelper.DeserializeWithCamelCasing<OrderInputModel>(orderStr);
        var orderId = inputModel?.Id;
        var card = inputModel?.CreditCard;

        CreditCardDetector detector = new CreditCardDetector(card);

        if (detector.IsValid())
        {
            Console.WriteLine($"OrderId {orderId}: CreditCard {card} is valid");
        }
        else 
        {
            Console.WriteLine($"OrderId {orderId}: CreditCard {card} is invalid");
        }
    };
    channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);



    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}