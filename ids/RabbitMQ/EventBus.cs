using AutoMapper.Configuration;
using IdentityServerHost.Quickstart.UI;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using RabbitMQ.Client;
using System;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using System.Text;
using ids.Quickstart.Account;

namespace ids.RabbitMQ
{
    public class EventBus : IEventBus
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public EventBus(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
               // Port = int.Parse(_configuration["RabbitMQPort"])
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "msgTrigger", type: ExchangeType.Fanout);

                Console.WriteLine("Connected RabbitMQ");
            }
            catch(Exception)
            {
                Console.WriteLine("Can't connect with RabbitMQ");
            }
        }

        public void PublishNewMessage(UserPublishViewModel model)
        {
            string message = JsonSerializer.Serialize(model);

            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "msgTrigger",
                routingKey: "",
                basicProperties: null,
                body: body);

            Console.WriteLine("Sent: " + message);
        }


    }
}
