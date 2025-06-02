using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IO;
using Microsoft.Extensions.Logging;
using USPSystem.Models;

namespace USPSystem.Filters
{
    public class GradeRecheckFilter : ActionFilterAttribute
    {
        public GradeRecheckFilter()
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var model = context.ActionArguments.Values.FirstOrDefault(v => v is RecheckApplicationModel) as RecheckApplicationModel;
            
            if (model != null)
            {
                var controller = context.RouteData.Values["controller"];
                var action = context.RouteData.Values["action"];
                var userName = context.HttpContext.User?.Identity?.Name ?? "Anonymous";
                var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string logMessage = $"[{time}] GRADE RECHECK APPLICATION - User: {userName} - Course: {model.CourseCode} - Grade: {model.CurrentGrade}";
                
                // Log to console
                Console.WriteLine(logMessage);
                
                // Log to file system
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                Directory.CreateDirectory(logPath); // Ensure directory exists
                System.IO.File.AppendAllText(Path.Combine(logPath, "grade_rechecks.txt"), logMessage + Environment.NewLine);
                
                // ILogger can be resolved from DI container when needed
                // Use dependency injection in the controller instead
            }
            
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];
            var userName = context.HttpContext.User?.Identity?.Name ?? "Anonymous";
            var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var status = context.Exception == null ? "Success" : "Error";

            string logMessage = $"[{time}] GRADE RECHECK APPLICATION - User: {userName} - Completed {controller}/{action} - Status: {status}";
            
            // Log to console
            Console.WriteLine(logMessage);
            
            // Log to file system
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            Directory.CreateDirectory(logPath); // Ensure directory exists
            System.IO.File.AppendAllText(Path.Combine(logPath, "grade_rechecks.txt"), logMessage + Environment.NewLine);
            
            base.OnActionExecuted(context);
        }
    }
} 