using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace MultiUserBlock.Common
{
    public interface IWebSocketConnectionManager
    {
        ConcurrentDictionary<string, WebSocket> GetAll();
        string GetId(WebSocket socket);
        void AddSocket(WebSocket socket);
        Task RemoveSocket(string id);
    }
}
