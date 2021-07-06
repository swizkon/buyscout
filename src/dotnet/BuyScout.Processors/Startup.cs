using System;
using BuyScout.Common.Persistence;
using BuyScout.Domain.Interfaces;
using GreenPipes;
using MassTransit;
using MassTransit.Definition;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace BuyScout.Processors
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "BuyScout API", Version = "v1"});
            });

            services.AddSignalR();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                });
            });

            services.AddScoped<IRepository, MongoRepository>();

            AddMessageBrokerConfiguration(services, Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BuyScout API v1"));
            }

            if (env.IsProduction())
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();

            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseWebSockets();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static void AddMessageBrokerConfiguration(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumersFromNamespaceContaining<BlaBlaSomeHandler>();

                x.SetKebabCaseEndpointNameFormatter();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("", configurator =>
                    {
                        configurator.Password("guest");
                        configurator.Password("guest");
                    });

                    cfg.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(Environment.MachineName, false));
                    cfg.UseMessageRetry(r => r.Incremental(5, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2)));
                    cfg.UseInMemoryOutbox();
                });
            });

            services.AddMassTransitHostedService(waitUntilStarted: true);
        }
    }
}