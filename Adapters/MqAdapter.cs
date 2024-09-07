using CTS_BE.Events;

namespace CTS_BE.Adapters
{
    public abstract class MqAdapter
    {
        public abstract string Despatch(string queueName, string message);

        public abstract string ReceiveSingle(string queueName);

        public abstract string DeliveryAck(ulong deliveryTag);

        public abstract string SetupConsumer(
            string queueName,
            EventHandler<MqDeliverEventArgs> messageReceiveHandler,
            CancellationToken cancellationToken
        );

        public abstract string SetupAsyncConsumer(
            string queueName,
            EventHandler<MqDeliverEventArgs> messageReceiveHandler,
            CancellationToken cancellationToken
        );
        
        public abstract string CancelConsumer(string consumerTag);
    }
}