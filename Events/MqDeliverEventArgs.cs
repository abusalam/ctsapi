namespace CTS_BE.Events
{
    public class MqDeliverEventArgs : EventArgs
    {
        public string ConsumerTag { get; set; } = null!;
        public ulong DeliveryTag { get; set; }
        public bool Redelivered { get; set; }
        public string Body { get; set; } = null!;

        public MqDeliverEventArgs() { }

        public MqDeliverEventArgs(
            string consumerTag,
            ulong deliveryTag,
            bool redelivered,
            string body
        )
        {
            ConsumerTag = consumerTag;
            DeliveryTag = deliveryTag;
            Redelivered = redelivered;
            Body = body;
        }
    }
}
