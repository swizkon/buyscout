using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
//using BuyScout.API.Messaging;
using BuyScout.Contracts;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BuyScout.API.Services
{
    public class EmailHostedService : IHostedService
    {
        private static readonly TimeSpan Interval = TimeSpan.FromSeconds(5);

        private readonly IBus _bus;
        private readonly ILogger<EmailHostedService> _logger;

        private Task _processingTask;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public EmailHostedService(
            IBus bus,
            ILogger<EmailHostedService> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _processingTask = ProcessEmail(_cancellationTokenSource.Token);
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_processingTask != null)
            {
                try
                {
                    _cancellationTokenSource.Cancel();
                }
                finally
                {
                    await Task.WhenAny(_processingTask, Task.Delay(Timeout.Infinite, cancellationToken));
                }
            }
        }

        private async Task ProcessEmail(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(Interval, cancellationToken);

                    _logger.LogInformation("{Service} ProcessEmail", GetType().Name);

                    await _bus.Publish(new SystemTickEvent(), context =>
                    {
                        context.CorrelationId = Guid.NewGuid();
                        // context.
                    }, cancellationToken);
                    
                    await _bus.Publish(new SystemHeartbeatEvent
                    {
                        Name = typeof(Startup).Namespace,
                        UtcTimestamp = DateTime.UtcNow
                    }, context =>
                    {
                        context.CorrelationId = Guid.NewGuid();
                        // context.
                    }, cancellationToken);

                    //using var message = new MailMessage(
                    //    from: "noreply@buyscout.net", 
                    //    to: "user@domain.com",
                    //    subject: "This is the way",
                    //    body: "And here goes the body...");
                    ////message.IsBodyHtml
                    //await new SmtpClient("localhost", 1025).SendMailAsync(message, cancellationToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error when processing bank transfers");
                }
            }
        }
    }
}
