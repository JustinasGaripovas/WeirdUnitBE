using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WeirdUnitBE.GameLogic.Services.Implementation;

namespace WeirdUnitBE.Middleware
{
    public static class WebSocketMiddlewareExtentions
    {
        public static IApplicationBuilder UseWebSocketServer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<WebSocketServerMiddleware>();
        }

        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            services.AddSingleton<WebSocketServerManager>();
            return services;
        }
    }
}