using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedResources.Helpers
{
    public static class ErrorHandler
    {
        public static ProblemDetails GetProblemDetailsAsync(Exception ex, string requestPath, IServiceScope serviceScope)
        {

            var logger = serviceScope.ServiceProvider.GetService<ILogger>();
            logger.LogError(0, ex, ex.Message);

#if DEBUG
            var problemDetails = new ProblemDetails()
            {
                Detail = ex.Message,
                Instance = requestPath
            };
#else
            var problemDetails = new ProblemDetails()
            {
                Detail = "Something went wrong.",
                Instance = requestPath
            };
#endif



            if (ex is KeyNotFoundException)
            {
                problemDetails.Title = "Resource not found.";
                problemDetails.Status = 404;
                problemDetails.Type = "https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/404";
            }
            else if (ex is ArgumentException)
            {
                problemDetails.Title = "Bad request";
                problemDetails.Status = 400;
                problemDetails.Type = "https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/400";
            }
            else if(ex is UnauthorizedAccessException)
            {
                problemDetails.Title = "Invalid credentials.";
                problemDetails.Status = 401;
                problemDetails.Type = "https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/401";
            }
            else
            {
                problemDetails.Title = "Unexpected error";
                problemDetails.Status = 500;
                problemDetails.Type = "https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/500";
            }

            return problemDetails;
        }
    }
}
