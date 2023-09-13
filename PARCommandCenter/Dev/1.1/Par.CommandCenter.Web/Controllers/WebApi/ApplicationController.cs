using Azure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSwag.Annotations;
using Par.CommandCenter.Application.Handlers.Users.Queries.GetCurrentUserApplicationTenants;
using Par.CommandCenter.Application.Interfaces;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Par.CommandCenter.Web.Controllers.WebApi
{
    [Authorize]
    public class ApplicationController : BaseController
    {
        private readonly ICurrentUserService _currentUserService;

        public readonly IConfiguration _configuration;

        public readonly IApplicationDbContext _dbContext;

        public ApplicationController(IMediator mediator, IApplicationDbContext dbContext, ICurrentUserService currentUserService, IConfiguration configuration) : base(mediator)
        {
            _currentUserService = currentUserService;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [HttpGet]
        [OpenApiOperation(nameof(State), "Retrieve the initial application state", "The call intended to retrieve the command center app initial state and do any checks before the app start.")]
        public async Task<IActionResult> State()
        {
            var userApplicationTenants = (await Mediator.Send(new GetCurrentUserApplicationTenantsQuery())).UserApplicationTenants;
            var userName = string.IsNullOrWhiteSpace(_currentUserService.UPN) ? _currentUserService.PreferredUsername : _currentUserService.UPN;
            return await Task.FromResult(this.Ok(new
            {
                UserId = _currentUserService.UserId,
                UserName = _currentUserService.Name,
                Email = userName,
                TenantIds = userApplicationTenants?.Select(x => x.TenantId) ?? Enumerable.Empty<int>(),
                //tenantIds = Array.Empty<int>(),
                IsAuthorized = _currentUserService.IsAuthenticated,
                //IsUserFirstLogin = true,
                IsUserFirstLogin = !userApplicationTenants?.Any() ?? true,
            })).ConfigureAwait(false);
        }

        [HttpGet]
        [OpenApiOperation(nameof(HostingEnvironment), "Retrieve the application hosting environment Name.", "This call is intended to get the application hosting environment name")]
        public async Task<IActionResult> HostingEnvironment()
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            return await Task.FromResult(
                this.Ok(new
                {
                    Environment = environment,
                }
                )).ConfigureAwait(false);
        }


        [HttpGet]
        [OpenApiOperation(nameof(HostingInfo), "Retrieve the application server hosting information.", "This call is intended to get the application hosting information for different regions and test the application disaster recovery.")]
        public async Task<IActionResult> HostingInfo()
        {
            string sqlMessagePrimary;
            DateTime sqlDateTimePrimary;
           
            try
            {
                var conn = new SqlConnection("Server=sql-mi-prod1-fog.f0310bdf3e8f.database.windows.net;");

                var managedIdentityClientId = _configuration["ManagedIdentityClientId"];
                var options = new DefaultAzureCredentialOptions
                {
                    ManagedIdentityClientId = managedIdentityClientId,
                };
                var credential = new Azure.Identity.DefaultAzureCredential(options);


                var token = credential.GetToken(new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/.default" }));

                conn.AccessToken = token.Token;
                conn.Open();

                SqlCommand command = new SqlCommand("SELECT 'Hello from SQL Managed Instance in the EAST!' AS SqlMessage, GETUTCDATE() AS CurrentTime;", conn);

                SqlDataReader reader = command.ExecuteReader();
                reader.Read();

                sqlMessagePrimary = reader["SqlMessage"].ToString();
                _ = DateTime.TryParse($"{reader["CurrentTime"]}", out sqlDateTimePrimary);
                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                sqlMessagePrimary = $"Connection to primary SQL FOG endpoint failed: Exception: {ex.Message}";
                sqlDateTimePrimary = DateTime.MinValue;
            }

            string sqlMessageSecondary;
            DateTime sqlDateTimeSecondary;
            try
            {
                var conn = new SqlConnection("Server=sql-mi-prod1-fog.secondary.f0310bdf3e8f.database.windows.net;");

                var managedIdentityClientId = _configuration["ManagedIdentityClientId"];
                var options = new DefaultAzureCredentialOptions
                {
                    ManagedIdentityClientId = managedIdentityClientId
                };
                var credential = new Azure.Identity.DefaultAzureCredential(options);
                var token = credential.GetToken(new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/.default" }));
                conn.AccessToken = token.Token;
                conn.Open();

                SqlCommand command = new SqlCommand("SELECT 'Hello from SQL Managed Instance in the WEST!' AS SqlMessage, GETUTCDATE() AS CurrentTime;", conn);

                SqlDataReader reader = command.ExecuteReader();
                reader.Read();

                sqlMessageSecondary = reader["SqlMessage"].ToString();

                _ = DateTime.TryParse($"{reader["CurrentTime"]}", out sqlDateTimeSecondary);
               

                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                sqlMessageSecondary = $"Connection to secondary SQL FOG endpoint failed: Exception: {ex.Message}";
                sqlDateTimeSecondary = DateTime.MinValue;
            }

            string machineName = Environment.MachineName;
            string dnsHostName = System.Net.Dns.GetHostName();
            string serverTime = DateTime.Now.ToLocalTime().ToString("yyyy-MM-ddTHH:MM:ss.fffzzzz");
            string region = Environment.GetEnvironmentVariable("REGION_NAME");
            string resourceGroup = Environment.GetEnvironmentVariable("WEBSITE_RESOURCE_GROUP");
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");            
            return await Task.FromResult(
                this.Ok(new
                {
                    SqlMessagePrimary = sqlMessagePrimary,
                    SqlDateTimePrimary = sqlDateTimePrimary.ToString("MM/dd/yyyy hh:mm:ss"),
                    SqlMessageSecondary = sqlMessageSecondary,
                    SqlDateTimeSecondary = sqlDateTimeSecondary.ToString("MM/dd/yyyy hh:mm:ss"),
                    MachineName = machineName,
                    DnsHostName = dnsHostName,
                    ServerTime = serverTime,
                    Region = region,
                    ResourceGroup = resourceGroup,
                    Environment = environment,
                }
                )).ConfigureAwait(false);
        }
    }
}
