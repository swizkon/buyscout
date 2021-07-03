using System;
using System.Threading.Tasks;
using BuyScout.Common.Persistence;
using BuyScout.Contracts;
using BuyScout.Domain.Interfaces;
using GreenPipes;
using MassTransit;
using MassTransit.Definition;
using MassTransit.Topology.EntityNameFormatters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace BuyScout.Service
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

    public class BlaBlaSomeHandler :
        IConsumer<AddItemToListCommand>,
        IConsumer<SystemTickEvent>
    {
        private readonly ILogger<BlaBlaSomeHandler> _logger;

        public BlaBlaSomeHandler(ILogger<BlaBlaSomeHandler> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<AddItemToListCommand> context)
        {
            var message = context.Message;
            _logger.LogInformation("{MessageType} Title {Title}", message.GetType().Name, message.Title);
            return Task.CompletedTask;
        }

        public Task Consume(ConsumeContext<SystemTickEvent> context)
        {
            var message = context.Message;

            _logger.LogInformation("{MessageType}:{CorrelationId} at {Timestamp}", message.GetType().Name,
                context.CorrelationId, message.UtcTimestamp);
            return Task.CompletedTask;
        }
    }
}