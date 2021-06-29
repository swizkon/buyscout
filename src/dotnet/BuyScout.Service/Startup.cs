using System.Threading.Tasks;
using BuyScout.Service.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BuyScout API", Version = "v1" });
            });
            
            services.AddSignalR();

            services.AddCors(options => {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                });
            });

            services.AddHostedService<EmailHostedService>();
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

            if(env.IsProduction())
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();

            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseWebSockets();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapHub<TestHub>("/hubs/testHub");
            });
        }
    }

    //public interface ITestHubClient
    //{
    //    Task ReceiveMessage(string user, string message);

    //    Task Broadcast(string user, string message);
    //}

    //public class TestHub : Hub<ITestHubClient>
    //{
    //    public async Task SendMessage(string user, string message)
    //    {
    //        await Clients.All.ReceiveMessage(user, message);
    //    }

    //    public async Task Broadcast(string user, string message)
    //    {
    //        await Clients.All.Broadcast(user, message);
    //    }

    //    public override async Task OnConnectedAsync()
    //    {
    //        await Groups.AddToGroupAsync(Context.ConnectionId, "All Connected Users");
    //        await base.OnConnectedAsync();
    //    }
    //}
}
