using CTS_BE.Events;

namespace CTS_BE.Adapters
{
    public class MockMqAdapter : MqAdapter
    {

        public override string Despatch(string queueName, string message)
        {
            Console.WriteLine($"Dispatching {message} to {queueName} queue via MockMqAdapter");
            return $"Sending {message} to {queueName} queue via MockMqAdapter";
        }

        public override string ReceiveSingle(string queueName)
        {
            Console.WriteLine($"Received message from {queueName} via MockMqAdapter");
            return $"Received message from{queueName} via MockMqAdapter";
        }

        public override string DeliveryAck(ulong deliveryTag)
        {
            Console.WriteLine($"Acknowledged {deliveryTag} via MockMqAdapter");
            return $"Acknowledged {deliveryTag} via MockMqAdapter";
        }

        public override string SetupConsumer(
            string queueName,
            EventHandler<MqDeliverEventArgs> messageReceiveHandler,
            CancellationToken cancellationToken)
        {
            Console.WriteLine($"Listening to {queueName} via MockMqAdapter");
            return $"Listening to {queueName} via MockMqAdapter";
        }

        public override string CancelConsumer(string consumerTag)
        {
            Console.WriteLine($"Cancelling consumer {consumerTag} via MockMqAdapter");
            return $"Cancelling consumer {consumerTag} via MockMqAdapter";
        }

        public override string SetupAsyncConsumer(
            string queueName,
            EventHandler<MqDeliverEventArgs> messageReceiveHandler,
            CancellationToken cancellationToken
        )
        {
            Console.WriteLine($"Listening to {queueName} as async consumer via MockMqAdapter");
            return $"Listening to {queueName} as async consumer via MockMqAdapter";
        }
    }
}