using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace USPSystem.Filters
{
    public class LogActionFilter : ActionFilterAttribute
    {
        private readonly string _actionType;

        public LogActionFilter(string actionType)
        {
            _actionType = actionType;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];
            var userName = context.HttpContext.User?.Identity?.Name ?? "Anonymous";
            var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string logMessage = $"[{time}] User: {userName} - {_actionType} - Executing {controller}/{action}";
            
            // Log to console
            Console.WriteLine(logMessage);
            
            // Log to file system
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            Directory.CreateDirectory(logPath); // Ensure directory exists
            System.IO.File.AppendAllText(Path.Combine(logPath, "application_logs.txt"), logMessage + Environment.NewLine);
            
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];
            var userName = context.HttpContext.User?.Identity?.Name ?? "Anonymous";
            var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var status = context.Exception == null ? "Success" : "Error";

            string logMessage = $"[{time}] User: {userName} - {_actionType} - Completed {controller}/{action} - Status: {status}";
            
            // Log to console
            Console.WriteLine(logMessage);
            
            // Log to file system
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            Directory.CreateDirectory(logPath); // Ensure directory exists
            System.IO.File.AppendAllText(Path.Combine(logPath, "application_logs.txt"), logMessage + Environment.NewLine);
            
            base.OnActionExecuted(context);
        }
    }
} 