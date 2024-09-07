using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Renci.SshNet.Security.Cryptography.Ciphers.Modes;

namespace CTS_BE.Events
{
    public class MqDeliverEventArgs : EventArgs
    {
        public string ConsumerTag { get; set; }
        public ulong DeliveryTag { get; set; }
        public bool Redelivered { get; set; }
        public string Body;
        public MqDeliverEventArgs()
        {
            
        }

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