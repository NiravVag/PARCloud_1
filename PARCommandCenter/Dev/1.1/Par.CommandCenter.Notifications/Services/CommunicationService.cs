using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Domain.Entities.Email;
using Par.CommandCenter.Notifications.Services.Email;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Par.CommandCenter.Notifications.Services
{
    public interface ICommunicationService
    {
        [NotNull]
        Task SendMessage([NotNull] Notification notifiable);
    }

    public class CommunicationService : ICommunicationService
    {
        private readonly IEmailService _emailService;

        private readonly IEmailConfiguration _emailConfiguration;

        private IHostingEnvironment _hostingEnv;

        private readonly ILogger<CommunicationService> _logger;

        public CommunicationService(IEmailService emailService, IEmailConfiguration emailConfiguration, IHostingEnvironment hostingEnv, ILogger<CommunicationService> logger)
        {
            _logger = logger;

            _emailConfiguration = emailConfiguration;
            _emailService = emailService;

            _hostingEnv = hostingEnv;

            var templateFolderPath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Templates");

            // Configure RazerEngine and reassign over the static Engine with our new configuration.
            var config = new TemplateServiceConfiguration
            {
                Language = Language.CSharp,
                CachingProvider = new DefaultCachingProvider(t => { })
            };

            Engine.Razor = RazorEngineService.Create(config);

            if (Engine.Razor?.IsTemplateCached("_CustomerVPNConnectionDown", typeof(Notification)) != true)
            {
                Engine.Razor.AddTemplate(
                    "_CustomerVPNConnectionDown",
                    File.ReadAllText(templateFolderPath + "\\" + "_CustomerVPNConnectionDown.cshtml"));

                Engine.Razor.Compile("_CustomerVPNConnectionDown", typeof(Notification));
            }

            ////if (Engine.Razor?.IsTemplateCached("_CustomerRouterOfflineEmailTemplate", typeof(Notification)) != true)
            ////{
            ////    Engine.Razor.AddTemplate(
            ////        "_CustomerRouterOfflineEmailTemplate",
            ////        File.ReadAllText(templateFolderPath + "\\" + "_CustomerRouterOfflineEmailTemplate.cshtml"));

            ////    Engine.Razor.Compile("_CustomerRouterOfflineEmailTemplate", typeof(Notification));
            ////}

            ////if (Engine.Razor?.IsTemplateCached("_CustomerControllerOfflineEmailTemplate", typeof(Notification)) != true)
            ////{
            ////    Engine.Razor.AddTemplate(
            ////        "_CustomerControllerOfflineEmailTemplate",
            ////        File.ReadAllText(templateFolderPath + "\\" + "_CustomerControllerOfflineEmailTemplate.cshtml"));

            ////    Engine.Razor.Compile("_CustomerControllerOfflineEmailTemplate", typeof(Notification));
            ////}

            if (Engine.Razor?.IsTemplateCached("_ServerOperationEmailTemplate", typeof(Notification)) != true)
            {
                Engine.Razor.AddTemplate(
                    "_ServerOperationEmailTemplate",
                    File.ReadAllText(templateFolderPath + "\\" + "_ServerOperationEmailTemplate.cshtml"));

                Engine.Razor.Compile("_ServerOperationEmailTemplate", typeof(Notification));
            }

            if (Engine.Razor?.IsTemplateCached("_InterfaceEmailTemplate", typeof(Notification)) != true)
            {
                Engine.Razor.AddTemplate(
                    "_InterfaceEmailTemplate",
                    File.ReadAllText(templateFolderPath + "\\" + "_InterfaceEmailTemplate.cshtml"));

                Engine.Razor.Compile("_InterfaceEmailTemplate", typeof(Notification));
            }
        }

        public async Task SendMessage(Notification notification)
        {
            try
            {
                if (notification.Emails == null || !notification.Emails.Any())
                {
                    return;
                }

                List<EmailAddress> fromAddresses = new List<EmailAddress>() {
                        new EmailAddress()
                        {
                            Address = _emailConfiguration.CommandCenterEmailAddress,
                            Name = "PAR CommandCenter"
                        }
                    };


                var body = GenerateNotificationBody(notification);

                var environmentAdditionalText = (!_hostingEnv.IsProduction()) ? $" - {_hostingEnv.EnvironmentName}" : string.Empty;

                await _emailService.Send(
                    new EmailMessage()
                    {
                        FromAddresses = fromAddresses,
                        ToAddresses = notification.Emails,
                        Subject = notification.EmailSubject
                                  + ((notification.Tenant == null) ? string.Empty : $" - {notification.Tenant.Name}")
                                  + environmentAdditionalText,
                        Content = body
                    });

            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        /// <summary>
        /// Generates the notification message body via the <see cref="RazorEngineService"/>.
        /// </summary>
        /// <param name="notification">The model used when creating the notification.</param>
        /// <returns>A string representing the markup for the message body</returns>
        [NotNull]
        private static string GenerateNotificationBody(Notification notification)
        {
            var emailHtmlBody = Engine.Razor.Run(
                notification.TemplateName,
                typeof(Notification),
                notification);

            if (string.IsNullOrWhiteSpace(emailHtmlBody))
            {
                throw new InvalidOperationException("Email HTML body was null or whitespace.");
            }

            string premailedOutput = PreMailer.Net.PreMailer.MoveCssInline(emailHtmlBody, false, "#media-query")?.Html;

            return premailedOutput ?? string.Empty;
        }
    }
}
