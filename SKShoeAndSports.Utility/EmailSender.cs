using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SKShoeAndSports.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailOptions emailOptions;
        public EmailSender(IOptions<EmailOptions> options)
        {
            emailOptions = options.Value;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await Execute(emailOptions.SendGridKey, subject, htmlMessage, email);
        }

        private async Task Execute(string sendGridKey, string subject, string message, string email)
        {
            var client = new SendGridClient(sendGridKey);
            var from = new EmailAddress("bogzey@hotmail.com", "SK Shoe And Sports");
            var to = new EmailAddress(email, "End User");
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", message);
            await client.SendEmailAsync(msg);
        }
    }
}
