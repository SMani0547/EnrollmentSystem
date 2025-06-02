using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace USPSystem.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body);
        Task<bool> SendRecheckStatusUpdateEmailAsync(string to, string studentName, string courseCode, string courseName, string status);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _fromEmail;
        private readonly string _fromName;
        private readonly IEmailLoggingService _emailLogger;

        public EmailService(IConfiguration configuration, IEmailLoggingService emailLogger)
        {
            _configuration = configuration;
            _emailLogger = emailLogger;
            
            // Try to get settings from configuration
            _smtpServer = configuration["EmailSettings:SmtpServer"] ?? "smtp-relay.brevo.com";
            
            if (int.TryParse(configuration["EmailSettings:SmtpPort"], out int port))
            {
                _smtpPort = port;
            }
            else
            {
                _smtpPort = 587;
            }
            
            _smtpUsername = configuration["EmailSettings:SmtpUsername"] ?? "8e69e3001@smtp-brevo.com";
            _smtpPassword = configuration["EmailSettings:SmtpPassword"] ?? "b7sdYLjWSFZGOMgr";
            
            // Using sender details from the image
            _fromEmail = configuration["EmailSettings:FromEmail"] ?? "s11146291@student.usp.ac.fj";
            _fromName = configuration["EmailSettings:FromName"] ?? "USP System";
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromEmail, _fromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                
                mailMessage.To.Add(to);

                using var client = new SmtpClient(_smtpServer, _smtpPort)
                {
                    Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                    EnableSsl = true
                };

                await client.SendMailAsync(mailMessage);
                
                // Log the email
                _emailLogger.LogEmailSent(to, subject);
                
                Console.WriteLine($"Email sent successfully to {to}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendRecheckStatusUpdateEmailAsync(string to, string studentName, string courseCode, string courseName, string status)
        {
            var subject = $"Update on Your Grade Recheck Request for {courseCode}";
            
            var body = $@"
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    h1 {{ color: #005580; }}
                    .status {{ font-weight: bold; font-size: 16px; color: #005580; }}
                    .footer {{ margin-top: 30px; font-size: 12px; color: #666; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <h1>Grade Recheck Status Update</h1>
                    <p>Dear {studentName},</p>
                    <p>This is to inform you that the status of your grade recheck request for <strong>{courseCode}: {courseName}</strong> has been updated.</p>
                    <p>Current status: <span class='status'>{status}</span></p>
                    <p>
                        {GetStatusMessage(status)}
                    </p>
                    <p>If you have any questions, please contact the Academic Office.</p>
                    <p>Thank you,</p>
                    <p>USP Academic Services</p>
                    <div class='footer'>
                        <p>This is an automated message. Please do not reply to this email.</p>
                        <p>University of the South Pacific &copy; 2025</p>
                    </div>
                </div>
            </body>
            </html>";

            // Log before sending
            _emailLogger.LogEmailSent(to, subject, $"Student: {studentName}, Course: {courseCode}, Status: {status}");

            return await SendEmailAsync(to, subject, body);
        }

        private string GetStatusMessage(string status)
        {
            return status.ToLower() switch
            {
                "pending" => "Your request is in our queue and will be processed soon.",
                "inprogress" => "Your request is currently being reviewed by our academic staff.",
                "completed" => "The review of your grade has been completed. Please check your updated grade in the student portal.",
                "rejected" => "After careful consideration, your recheck request has been rejected. For more details, please contact the Academic Office.",
                _ => "Please check your student portal for more information."
            };
        }
    }
} 