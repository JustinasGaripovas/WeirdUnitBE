using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WeirdUnitBE.GameLogic.Services.Implementation;
using WeirdUnitBE.GameLogic.Services.Interfaces;
using WeirdUnitBE.Middleware;

using WeirdUnitBE.GameLogic.TowerPackage;

namespace WeirdUnitGame
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWebSocketManager(); // Dependency Injection
            
            services.Add(new ServiceDescriptor(typeof(IGameStateBuilder), new GameStateBuilder()));  
            
            services.AddLogging(builder =>
            {
                builder.AddConsole()
                    .AddDebug();
            });
        }

        public void Configure(IApplicationBuilder app, /*Microsoft.AspNetCore.Hosting.IHostingEnvironment env,*/ ILoggerFactory loggerFactory)
        {
            /*
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            */

            #region WebSocket Configurations
            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };
            app.UseWebSockets(webSocketOptions);
            app.UseWebSocketServer();
            #endregion

            app.UseFileServer();
        }
    }
}
