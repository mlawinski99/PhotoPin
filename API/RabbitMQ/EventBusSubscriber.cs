using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace API.RabbitMQ
{
    public class EventBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventProcessor _eventProcessor;
        private  IConnection _connection;
        private  IModel _channel;
        private string _queueName;
        public EventBusSubscriber(IConfiguration configuration,
            IEventProcessor eventProcessor)
        {
            _configuration = configuration;
            _eventProcessor = eventProcessor;
            Console.Write("asdasdas");
            StartRabbitMQ();
        }

        private void StartRabbitMQ()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "msgTrigger", type: ExchangeType.Fanout);
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: _queueName,
                exchange: "msgTrigger",
                routingKey: "");
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.Write("asdasasdffffffffffffffffdas");
            stoppingToken.ThrowIfCancellationRequested();
             
            var eventConsumer = new EventingBasicConsumer(_channel);

            eventConsumer.Received += (ModuleHandle, eventArgs) =>
            {
                Console.Write("asdasdas");
                var body = eventArgs.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                _eventProcessor.ProcessEvent(message);
            };
            _channel.BasicConsume(queue: _queueName, consumer: eventConsumer);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            if(_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
            base.Dispose();
        }
    }
}
