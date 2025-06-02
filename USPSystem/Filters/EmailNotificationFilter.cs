using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace USPSystem.Filters
{
    public class EmailNotificationFilter : ActionFilterAttribute
    {
        public EmailNotificationFilter()
        {
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];
            var userName = context.HttpContext.User?.Identity?.Name ?? "Anonymous";
            var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string logMessage = $"[{time}] EMAIL NOTIFICATION - User: {userName} - Email notification sent from {controller}/{action}";
            
            // Log to console
            Console.WriteLine(logMessage);
            
            // Log to file system
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            Directory.CreateDirectory(logPath); // Ensure directory exists
            System.IO.File.AppendAllText(Path.Combine(logPath, "email_notifications.txt"), logMessage + Environment.NewLine);
            
            base.OnActionExecuted(context);
        }
    }
} 