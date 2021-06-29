using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BuyScout.Service.Services
{
    public class EmailHostedService : IHostedService
    {
        private static readonly TimeSpan Interval = TimeSpan.FromSeconds(5);

        private readonly ILogger<EmailHostedService> _logger;

        private Task _processingTask;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public EmailHostedService(
            ILogger<EmailHostedService> logger)
        {
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
