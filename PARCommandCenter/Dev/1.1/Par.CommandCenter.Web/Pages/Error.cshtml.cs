using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Common.Exceptions;
using System;
using System.Diagnostics;

namespace Par.CommandCenter.Web.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {
        private readonly ILogger<ErrorModel> _logger;

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string ExceptionMessage { get; set; }

        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            var exceptionHandlerPathFeature =
            HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionHandlerPathFeature?.Error is InvalidOperationException)
            {
                var error = exceptionHandlerPathFeature?.Error as InvalidOperationException;

                if (error.Message.Contains("index.html"))
                {
                    ExceptionMessage = "You don't have permission to view this page";
                }
                else
                {
                    ExceptionMessage = "Oops! Something went wrong! If you need further assistance, please contact the administrator";
                }

                _logger.LogError(error.Message);
            }

            if (exceptionHandlerPathFeature?.Error is UserNotFoundException)
            {
                var error = exceptionHandlerPathFeature?.Error as UserNotFoundException;

                ExceptionMessage = error.Message;
                _logger.LogError(ExceptionMessage);
            }

            if (exceptionHandlerPathFeature?.Path == "/index")
            {
                ExceptionMessage += " from home page";
            }
        }
    }
}
