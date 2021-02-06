using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;

using OccamsRazor.Common.Models;
using OccamsRazor.Web.Repository;

namespace OccamsRazor.Web.Service
{
    public interface INotificationService
    {
        Task HandleHostConnected(WebSocket socket, TaskCompletionSource<object> t);
        Task HandleConnected(Player player, WebSocket socket, TaskCompletionSource<object> t);
        Task<bool> SendPlayerMessage(string message);
        Task<bool> UpdateHost(string message);
    }
}