using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using MimeKit;
using System.Net.Mail;
using HandlebarsDotNet;
using BrenkaloWebStoreApi.Models;

namespace BrenkaloWebStoreApi.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly EmailSettings _emailSettings;

        public EmailService(IConfiguration configuration, IWebHostEnvironment environment, ILogger<EmailService> logger)
        {
            _logger = logger;
            _environment = environment;

            // Load EmailSettings from configuration
            _emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>();
            _emailSettings.Password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");
            if (string.IsNullOrWhiteSpace(_emailSettings.Password))
            {
                _logger.LogError("Email password is not set in the environment variables.");
                throw new InvalidOperationException("Email password is required.");
            }
        }

        public MimeMessage BuildMessageBody(string recipient, string newPassword, string subject = "Password Reset Notification")
        {
            var email = new MimeMessage();

            // Add sender and recipient
            email.From.Add(new MailboxAddress("Brenkalo", _emailSettings.Email));
            email.To.Add(MailboxAddress.Parse(recipient));

            email.Subject = subject;

            // Load the HTML template
            string templatePath = Path.Combine(_environment.ContentRootPath, "Templates", "PasswordReset.html");
            string template = File.ReadAllText(templatePath);

            // Prepare template data
            var templateData = new
            {
                heading = "Brenkalo Password Reset",
                newPassword = newPassword,
                resetLink = "http://172.21.159.90:5173/user/login"
            };

            // Compile the template
            var compiledTemplate = Handlebars.Compile(template);
            var emailBody = compiledTemplate(templateData);

            // Build the MIME body
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = emailBody
            };
            email.Body = bodyBuilder.ToMessageBody();

            return email;
        }

        public async Task SendMessageAsync(MimeMessage message)
        {
            using var client = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.Email, _emailSettings.Password);

                await client.SendAsync(message);
                _logger.LogInformation("Message sent successfully to {Recipient}", string.Join(", ", message.To.Select(r => r.Name)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending the email");
                throw;
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }
    }
}