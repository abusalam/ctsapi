using CTS_BE.Events;

namespace CTS_BE.BAL.Interfaces
{
    public interface IMqService
    {

        public event EventHandler<MqDeliverEventArgs> RecceiveHandler;

        public string Despatch(string queueName, string message);

        public string ReceiveSingle(string queueName);

        public string DeliveryAck(ulong deliveryTag);

        public string SetupConsumer(string queueName, CancellationToken cancellationToken);
        public string SetupAsyncConsumer(string queueName, CancellationToken cancellationToken);

        public string CancelConsumer(string consumerTag);
    }
}