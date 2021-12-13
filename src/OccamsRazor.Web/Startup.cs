using System.IO;
using Microsoft.AspNetCore.Authorization;
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
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

using OccamsRazor.Common.Configuration;
using OccamsRazor.Common.Context;
using OccamsRazor.Web.Configuration;
using OccamsRazor.Web.Middleware;
using OccamsRazor.Web.Repository;
using OccamsRazor.Web.Service;
using OccamsRazor.Web.Persistence.Repository;
using OccamsRazor.Web.Persistence.Service;

namespace OccamsRazor.Web
{
    public class Startup
    {
        private readonly string WWW_ROOT;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            WWW_ROOT = System.Environment.GetEnvironmentVariable("WWW_ROOT") ?? "web";
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews();


            var dbConfig = Configuration.GetSection("DB").Get<DbConfiguration>();
            services.AddScoped<DbConfiguration>(_ => dbConfig);

            services.AddDbContext<OccamsRazorEfSqlContext>(options =>
               options.UseMySql(dbConfig.ConnectionString, new MariaDbServerVersion(new System.Version(dbConfig.MariaDbVersion)))
            );

            var jwtConfig = Configuration.GetSection("JWT").Get<JwtConfiguration>();
            services.AddScoped<JwtConfiguration>(_ => jwtConfig);

            services.AddSpaStaticFiles(spa => spa.RootPath = WWW_ROOT);

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

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidAudience = jwtConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtConfig.Key))
                };
                options.MapInboundClaims = true;
            });

            services.AddAuthorization(options => {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim("id")
                    .RequireClaim("gameId")
                    .Build();
                options.DefaultPolicy = policy;
            });
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
                ReceiveBufferSize = 4 * 1024,
            };
            webSocketOptions.AllowedOrigins.Add("localhost");

            app.UseWebSockets(webSocketOptions);
            app.UseHttpsRedirection();
            app.UseMiddleware<NotificationMiddleware>();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials

            app.UseDefaultFiles();
            app.UseSpaStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller}/{action=Index}/{id?}");
            });
            app.MapWhen(context => !(context.Request.Path.Value.StartsWith("/api") || context.Request.Path.Value.StartsWith("/notifications")),
            config =>
            {
                System.Console.WriteLine("HERE");
                config.UseSpa(spa =>
                {
                    spa.Options.SourcePath = WWW_ROOT;
                });
            });


        }
    }
}
