using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultiUserBlock.Common;

namespace MultiUserBlock.WebSockets
{
    public enum MessageType
    {
        Text,
        ClientMethodInvocation,
        ConnectionEvent
    }

    public class Message : IMessage<MessageType>
    {
        public MessageType MessageType { get; set; }
        public string Data { get; set; }
    }
}

