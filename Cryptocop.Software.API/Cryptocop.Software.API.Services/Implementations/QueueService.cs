using Cryptocop.Software.API.Services.Helpers;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class QueueService : IQueueService, IDisposable
    {
        private readonly string _exchangeName;

        private readonly IConnection _connection;
        private readonly IModel _channel;
        public QueueService(IConfiguration configuration)
        {
            var configSection = configuration.GetSection("RabbitMQ");

            _exchangeName = configSection.GetValue<string>("Exchange");

            var HostName = configSection.GetValue<string>("Host");
            var UserName = configSection.GetValue<string>("UserName");
            var Password = configSection.GetValue<string>("Password");
            var VirtualHost = configSection.GetValue<string>("VirtualHost");

            Console.WriteLine("HELLOHELLO");
            Console.WriteLine(HostName + " " + UserName + " " + Password + " " + VirtualHost);

            IAsyncConnectionFactory connectionFactory = new ConnectionFactory
            {
                HostName = configSection.GetValue<string>("Host"),
                UserName = configSection.GetValue<string>("UserName"),
                Password = configSection.GetValue<string>("Password"),
                VirtualHost = configSection.GetValue<string>("VirtualHost")
            };
            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
        }
        public void PublishMessage(string routingKey, object body)
        {
            var json = JsonSerializerHelper.SerializeWithCamelCasing(body);

            var bytes = Encoding.UTF8.GetBytes(json);
            _channel.BasicPublish(_exchangeName, routingKey, body: bytes);
        }

        public void Dispose()
        {
            // TODO: Dispose the connection and channel
            GC.SuppressFinalize(this);

            _channel.Dispose();
            _connection.Dispose();
        }
    }
}