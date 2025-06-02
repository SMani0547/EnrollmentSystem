using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace USPSystem.Services
{
    public interface IEmailLoggingService
    {
        void LogEmailSent(string recipientEmail, string subject, string? userId = null);
    }

    public class EmailLoggingService : IEmailLoggingService
    {
        private readonly ILogger<EmailLoggingService> _logger;

        public EmailLoggingService(ILogger<EmailLoggingService> logger)
        {
            _logger = logger;
        }

        public void LogEmailSent(string recipientEmail, string subject, string? userId = null)
        {
            var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var userInfo = string.IsNullOrEmpty(userId) ? "Anonymous" : userId;
            
            string logMessage = $"[{time}] EMAIL SENT - Recipient: {recipientEmail} - Subject: {subject} - User: {userInfo}";
            
            // Log to console
            Console.WriteLine(logMessage);
            
            // Log to file system
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            Directory.CreateDirectory(logPath); // Ensure directory exists
            System.IO.File.AppendAllText(Path.Combine(logPath, "email_logs.txt"), logMessage + Environment.NewLine);
            
            // Log to ILogger
            _logger.LogInformation(logMessage);
        }
    }
} 