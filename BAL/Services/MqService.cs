using CTS_BE.BAL.Interfaces;
using CTS_BE.Adapters;
using CTS_BE.Events;

namespace CTS_BE.BAL.Services
{
    public class MqService : IMqService
    {
        private readonly MqAdapter _mqAdapter;

        public event EventHandler<MqDeliverEventArgs>? RecceiveHandler;

        public MqService(MqAdapter mqAdapter)
        {
            _mqAdapter = mqAdapter;
        }

        public string Despatch(string queueName, string message)
        {
            return _mqAdapter.Despatch(queueName, message);
        }

        public string ReceiveSingle(string queueName)
        {
            return _mqAdapter.ReceiveSingle(queueName);
        }

        public string DeliveryAck(ulong deliveryTag)
        {
            return _mqAdapter.DeliveryAck(deliveryTag);
        }

        public string SetupConsumer(string queueName, CancellationToken cancellationToken)
        {
            // _mqAdapter.MessageReceived += (sender, args) => MessageReceived?.Invoke(sender, args);
            if(RecceiveHandler == null) {
                return "Error: Please setup RecceiveHandler first";
            }
            return _mqAdapter.SetupConsumer(
                queueName,
                RecceiveHandler,
                cancellationToken
            );
        }

        public string CancelConsumer(string consumerTag)
        {
            _mqAdapter.CancelConsumer(consumerTag);
            return "Stopped receiving messages";
        }

        public string SetupAsyncConsumer(string queueName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("Not implemented yet");
            // if(RecceiveHandler == null) {
            //     return "Error: Please setup RecceiveHandler first";
            // }
            // return _mqAdapter.SetupAsyncConsumer(
            //     queueName,
            //     RecceiveHandler,
            //     cancellationToken
            // );
        }
    }
}