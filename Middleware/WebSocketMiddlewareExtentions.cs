using Microsoft.AspNetCore.Builder;

namespace WeirdUnitBE.Middleware
{
    public static class WebSocketMiddlewareExtentions
    {
        public static IApplicationBuilder UseWebSocketServer(this IApplicationBuilder builder) {
            return builder.UseMiddleware<WebSocketServerMiddleware>();
        }
    }
}