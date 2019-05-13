namespace OAuth_JWT.API.Services
{
    using System.Configuration;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using SendGrid;
    using SendGrid.Helpers.Mail;

    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await configSendGridasync(message);
        }

        // Use NuGet to install SendGrid (Basic C# client lib) 
        private async Task configSendGridasync(IdentityMessage message)
        {
            var myMessage = new SendGridMessage();

            myMessage.AddTo(message.Destination);
            myMessage.From = new EmailAddress("taiseer@bitoftech.net", "Taiseer Joudeh");
            myMessage.Subject = message.Subject;
            myMessage.PlainTextContent = message.Body;
            myMessage.HtmlContent = message.Body;

            string apiKey = ConfigurationManager.AppSettings["emailService:Account"];
            SendGridClientOptions clientOptions = new SendGridClientOptions() { ApiKey = apiKey };

            SendGridClient client = new SendGridClient(clientOptions);

            // Send the email.
            if (client != null)
            {
                await client.SendEmailAsync(myMessage);
            }
            else
            {
                await Task.FromResult(0);
            }
        }
    }
}