using OccamsRazor.Context;
using OccamsRazor.Repositories;
using OccamsRazor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace OccamsRazor
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            var priKey = Environment.GetEnvironmentVariable("JWT_PRI_KEY");
            var priKeyBytes = Convert.FromBase64String(priKey);
            var pubKey = Environment.GetEnvironmentVariable("JWT_PUB_KEY");
            var pubKeyBytes = Convert.FromBase64String(pubKey);
            var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");

            var tokenParams = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidIssuer = issuer,
                IssuerSigningKey = new SymmetricSecurityKey(priKeyBytes)
            };

            var connString = System.Environment.GetEnvironmentVariable("CONNECTION_STRING");

            services.AddDbContext<OccamsRazorEfSqlContext>(options =>
               options.UseSqlServer(connString)
            );

            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<TokenService>((svc) => new TokenService(priKeyBytes, issuer, tokenParams));
            services.AddScoped<GameService>();
            services.AddSingleton<WebSocketService>();


            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Bearer";
            }).AddJwtBearer(options => {
                options.Audience = issuer;
                options.TokenValidationParameters = tokenParams;
            });

            services.AddAuthorization(options => {
                options.AddPolicy("player", policy => policy.RequireClaim(Constants.ClaimRole, "Player"));
                options.AddPolicy("host", policy => policy.RequireClaim(Constants.ClaimRole, "Host"));
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes("Bearer")
                    .Build();
            });

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

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = System.TimeSpan.FromSeconds(120),
            };

            app.UseWebSockets(webSocketOptions);


            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
