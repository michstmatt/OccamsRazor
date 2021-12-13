using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OccamsRazor.Web.Service;

namespace OccamsRazor.Web.Middleware
{
    public class NotificationMiddleware
    {
        private readonly RequestDelegate requestDelegate;
        private readonly ILogger<NotificationMiddleware> logger;
        private readonly INotificationService notificationService;

        public NotificationMiddleware(RequestDelegate rDelegate, ILogger<NotificationMiddleware> logger, INotificationService service)
        {
            this.requestDelegate = rDelegate;
            this.logger = logger;
            this.notificationService = service;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await requestDelegate(context);
                return;
            }
            var source = new TaskCompletionSource<object>();
            if (context.Request.Path.Value.StartsWith("/notifications/player"))
            {
                var splits = context.Request.Path.Value.Split('/');
                if (!int.TryParse(splits[splits.Length - 2], out int gameId))
                    return;
                var name = splits[splits.Length - 1];
                using (var webSocket = await context.WebSockets.AcceptWebSocketAsync())
                {
                    await notificationService.HandleConnected(gameId, new Common.Models.Player { Name = name }, webSocket, source);
                }
            }
            else if (context.Request.Path.Value.StartsWith("/notifications/host"))
            {
                using (var webSocket = await context.WebSockets.AcceptWebSocketAsync())
                {
                    var splits = context.Request.Path.Value.Split('/');
                    if (!int.TryParse(splits[splits.Length - 1], out int gameId))
                        return;
                    await notificationService.HandleHostConnected(gameId, webSocket, source);
                }
            }
            await source.Task;
        }
    }

}