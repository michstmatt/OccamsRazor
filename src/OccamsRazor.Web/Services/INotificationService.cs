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
        Task HandleHostConnected(int gameId, WebSocket socket, TaskCompletionSource<object> t);
        Task HandleConnected(int gameId, Player player, WebSocket socket, TaskCompletionSource<object> t);
        Task<bool> SendPlayerMessage(int gameId, string message);
        Task<bool> UpdateHost(int gameId, string message);
    }
}