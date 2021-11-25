using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql;

using OccamsRazor.Common.Context;
using OccamsRazor.Web.Repository;
using OccamsRazor.Web.Service;
using OccamsRazor.Web.Persistence.Repository;
using OccamsRazor.Web.Persistence.Service;
namespace OccamsRazor.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews();

            var connString = System.Environment.GetEnvironmentVariable("CONNECTION_STRING");
            var mariaDbVersion = System.Environment.GetEnvironmentVariable("MARIADB_VERSION");

            services.AddDbContext<OccamsRazorEfSqlContext>(options =>
               options.UseMySql(connString, new MariaDbServerVersion(new System.Version(mariaDbVersion)))
            );

            OccamsRazorEfSqlContext.ANSWER_TABLE = System.Environment.GetEnvironmentVariable("ANSWERS_TABLE");
            OccamsRazorEfSqlContext.GAMEMETADATA_TABLE = System.Environment.GetEnvironmentVariable("GAMEMETADATA_TABLE");
            OccamsRazorEfSqlContext.QUESTION_TABLE = System.Environment.GetEnvironmentVariable("QUESTIONS_TABLE");
            OccamsRazorEfSqlContext.KEY_TABLE = System.Environment.GetEnvironmentVariable("KEYS_TABLE");
            OccamsRazorEfSqlContext.MC_QUESTION_TABLE = System.Environment.GetEnvironmentVariable("MC_QUESTIONS_TABLE");

            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            //services.AddSingleton<IGameDataRepository, GameTestDataRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IMultipleChoiceRepository, MultipleChoiceQuestionRepository>();
            services.AddScoped<IGameDataRepository, GameDataRepository>();
            services.AddScoped<IGameDataService, GameDataService>();
            //services.AddSingleton<IPlayerAnswerRepository, PlayerTestAnswerRepository>();
            services.AddScoped<IPlayerAnswerRepository, PlayerAnswerRepository>();
            services.AddScoped<IPlayerAnswerService, PlayerAnswerService>();
            services.AddSingleton<INotificationService, NotificationService>((svc) => NotificationService.singleton);
            services.AddHttpContextAccessor();
            services.AddConnections();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = System.TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };

            app.UseWebSockets(webSocketOptions);
            app.UseHttpsRedirection();
            app.UseRouting();

            
            app.Use(async (context, next) =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    if (context.Request.Path.Value.StartsWith("/notifications/player"))
                    {
                        var splits = context.Request.Path.Value.Split('/');
                        var name = splits[splits.Length - 1];
                        using (var webSocket = await context.WebSockets.AcceptWebSocketAsync())
                        {
                            var task = new System.Threading.Tasks.TaskCompletionSource<object>();
                            await NotificationService.singleton.HandleConnected(new Common.Models.Player { Name = name }, webSocket, task);
                            await task.Task;
                        }
                    }
                    else if (context.Request.Path.Value.StartsWith("/notifications/host"))
                    {
                        using (var webSocket = await context.WebSockets.AcceptWebSocketAsync())
                        {
                            var task = new System.Threading.Tasks.TaskCompletionSource<object>();
                            await NotificationService.singleton.HandleHostConnected(webSocket, task);
                            await task.Task;
                        }
                    }
                }
                else
                {
                    await next();
                }
            });
            // global cors policy
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials

            var www = System.Environment.GetEnvironmentVariable("WWW_ROOT") ?? "web";

            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, www)  
                ),
                RequestPath = ""
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

        }
    }
}
