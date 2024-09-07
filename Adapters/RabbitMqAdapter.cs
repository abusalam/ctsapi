using System.Text;
using System.Text.Json;
using CTS_BE.Events;
using CTS_BE.Helper;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace CTS_BE.Adapters
{
    public class RabbitMqOptions
    {
        public const string RabbitMq = "RabbitMq";

        public string HostName { get; set; } = "rabbitmq";
        public int Port { get; set; } = 5672;
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string VirtualHost { get; set; } = "/";
        public string Exchange { get; set; } = String.Empty;
        public string Queue { get; set; } = "cts";
        public string RoutingKey { get; set; } = String.Empty;
        public int PrefetchCount { get; set; } = 1;
        public string ExchangeType { get; set; } = "topic";
        public bool Durable { get; set; } = true;
        public bool Exclusive { get; set; } = false;
        public bool AutoDelete { get; set; } = false;
        public bool AutoAck { get; set; } = false;
    }

    public class RabbitMqAdapter : MqAdapter
    {
        // public event EventHandler<MqDeliverEventArgs> DeliveryHandler;
        private readonly IConnection _connection;
        private string _queueName;
        private readonly IModel _channel;

        public RabbitMqAdapter(RabbitMqOptions rabbitMqOptions)
        {
            ConnectionFactory factory = new();
            factory.FillFrom(rabbitMqOptions);

            _queueName = rabbitMqOptions.Queue;
            try {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
            }
            catch (BrokerUnreachableException e) {
                Console.WriteLine($"Failed to connect to rabbitmq: {e.Message}");
            }
        }
        
        private void SetupQueue(string queueName) {
            if(_channel == null) {
                throw new Exception($"Failed to setup queue: {queueName} via RabbitMqAdapter");
            }
            _queueName = queueName;
            _channel.QueueDeclare(queue: _queueName,
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
        }

        public override string DeliveryAck(ulong deliveryTag)
        {
            _channel.BasicAck(deliveryTag, false);
            return $"Acknowledged {deliveryTag} via RabbitMqAdapter";
        }

        public override string Despatch(string queueName, string message)
        {
            SetupQueue(queueName);
            var body = Encoding.UTF8.GetBytes(message);
            IBasicProperties properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            _channel.BasicPublish(exchange: string.Empty,
                                routingKey: queueName,
                                basicProperties: properties,
                                body: body);
            Console.WriteLine($" [x] Sent {message}");
            return $"Sending {message} via RabbitMqAdapter";
        }

        public override string ReceiveSingle(string queueName)
        {
            SetupQueue(_queueName);
            var msgData = _channel.BasicGet(_queueName,false);
            string receivedMessage = string.Empty;
            if (msgData != null) {
                receivedMessage = Encoding.UTF8.GetString(msgData.Body.ToArray());
                _channel.BasicAck(msgData.DeliveryTag, false);
                Console.WriteLine($" [x] Received {receivedMessage}");
            }
            return (receivedMessage != string.Empty) 
                ? $"Received {receivedMessage} via RabbitMqAdapter" 
                : "Queue is empty";
        }

        public override string SetupConsumer(
            string queueName,
            EventHandler<MqDeliverEventArgs> messageReceiveHandler,
            CancellationToken cancellationToken
        )
        {
            SetupQueue(_queueName);        
            string consumerTag = string.Empty;
            if(_channel.ConsumerCount(queueName) == 0) {
                EventingBasicConsumer consumer = new (_channel);
                consumer.Received += (model, ea) =>
                {
                    MqDeliverEventArgs args = new(
                        consumerTag: ea.ConsumerTag,
                        deliveryTag: ea.DeliveryTag,
                        redelivered: ea.Redelivered,
                        body: Encoding.UTF8.GetString(ea.Body.ToArray())
                    );
                    messageReceiveHandler.Invoke(model, args);
                    // _channel.BasicAck(ea.DeliveryTag, false);
                };
                consumer.ConsumerCancelled += (model, ea) =>
                {
                    Console.WriteLine($" [x] Stopped Listening to {queueName} via RabbitMqAdapter");
                };
                consumerTag = _channel.BasicConsume(
                    queueName,
                    false,
                    consumer
                );
                cancellationToken.Register(()=>{
                        _channel.BasicCancel(consumerTag);
                        Console.WriteLine($" [x] Cancelling Listener on {queueName} via RabbitMqAdapter");
                    }
                );
                Console.WriteLine($" [x] Listening to {queueName} via RabbitMqAdapter");
            }
            return consumerTag;// $"Listening to {queueName} with consumerTag {consumerTag} via RabbitMqAdapter";
        }

        /// <summary>
        /// Setup async consumer is not working correctly
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="messageReceiveHandler"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override string SetupAsyncConsumer(
            string queueName,
            EventHandler<MqDeliverEventArgs> messageReceiveHandler,
            CancellationToken cancellationToken
        )
        {
            SetupQueue(_queueName);             
            string consumerTag = string.Empty;
            if(_channel.ConsumerCount(queueName) == 0) {
                AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.Received += async (model, ea) =>
                {
                    MqDeliverEventArgs args = new(
                        consumerTag: ea.ConsumerTag,
                        deliveryTag: ea.DeliveryTag,
                        redelivered: ea.Redelivered,
                        body: Encoding.UTF8.GetString(ea.Body.ToArray())
                    );
                    messageReceiveHandler.Invoke(model, args);
                    await Task.Yield();
                    // _channel.BasicAck(ea.DeliveryTag, false);
                };
                consumer.ConsumerCancelled += async (model, ea) =>
                {
                    Console.WriteLine($" [x] Stopped Listening to {queueName} via RabbitMqAdapter");
                    await Task.Yield();
                };
                consumerTag = _channel.BasicConsume(
                    queueName,
                    false,
                    consumer
                );
                cancellationToken.Register(()=>{
                        _channel.BasicCancel(consumerTag);
                        Console.WriteLine($" [x] Cancelling Listener on {queueName} via RabbitMqAdapter");
                    }
                );
                Console.WriteLine($" [x] Listening to {queueName} via RabbitMqAdapter");
            }
            return consumerTag;// $"Listening to {queueName} with consumerTag {consumerTag} via RabbitMqAdapter";
        }

        public override string CancelConsumer(string consumerTag)
        {
            if(_channel == null) {
                throw new Exception($"Failed to stop consumer: {consumerTag} via RabbitMqAdapter");
            }
            _channel.BasicCancel(consumerTag);
            Console.WriteLine($" [x] Cancelling Listener on {consumerTag} via RabbitMqAdapter");
            return $"Cancelling Listener on {consumerTag} via RabbitMqAdapter";
        }
    }
}