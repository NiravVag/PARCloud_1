using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Par.CommandCenter.Application.Common.Claims;
using Par.CommandCenter.Application.Common.Exceptions;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities;
using Par.CommandCenter.Identity.ConfigurationOptions;
using Par.CommandCenter.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Par.CommandCenter.Identity
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration configuration)
        {
            var commandCenterSecurityGroupOptions = configuration.GetSection("CommandCenterSecurityGroupOptions").Get<CommandCenterSecurityGroupOptions>();

            services.AddSingleton<CommandCenterSecurityGroupOptions>(commandCenterSecurityGroupOptions);

            //services.AddDbContext<UserDbContext>(options =>
            //{
            //    var conn = new SqlConnectionStringBuilder(configuration.GetConnectionString("DefaultConnection"))
            //    {
            //        ConnectRetryCount = 5,
            //        ConnectRetryInterval = 2,
            //        MaxPoolSize = 600,
            //        MinPoolSize = 5,
            //    };

            //    var dbConnection = new SqlConnection(conn.ToString())
            //    {
            //        AccessToken = new AzureServiceTokenProvider().GetAccessTokenAsync("https://database.windows.net/").Result
            //    };

            //    options.UseSqlServer(dbConnection,
            //        sqlServerOptionsAction: sqlOptions =>
            //        {
            //            sqlOptions.EnableRetryOnFailure(
            //            maxRetryCount: 10,
            //            maxRetryDelay: TimeSpan.FromSeconds(30),
            //            errorNumbersToAdd: null);
            //        }
            //    );
            //}, ServiceLifetime.Scoped);


            services.AddDbContext<UserDbContext>(options =>

             options.UseSqlServer(
                 configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUserDbContext>(x => x.GetService<UserDbContext>());

            
            //string[] scopes = new string[] { "api://2775b3c0-7c92-44c6-8f0c-cc753e48f816/.default" };
            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
           .AddMicrosoftIdentityWebApp( options =>
           {
               configuration.GetSection("AzureAd").Bind(options);

               options.Events = new OpenIdConnectEvents
               {
                   
                   OnTokenValidated = async ctx =>
                   {
                       // Access Token
                       //var accessToken = ctx.SecurityToken.RawData;

                       bool isAuthorizedUser = ctx.Principal.IsInRole(commandCenterSecurityGroupOptions.CommandCenterUsersGroupObjectId);

                       if (!isAuthorizedUser)
                       {
                           throw new UserNotFoundException("You need permissions to access PAR Command Center.");
                       }


                       //Get the user's unique identifier
                       string oid = ctx.Principal.FindFirstValue(ParClaimTypes.OBJECT_GUID);

                       string userPrincipalName = ctx.Principal.FindFirstValue(ParClaimTypes.PREFERRED_USERNAME);

                       if (string.IsNullOrWhiteSpace(userPrincipalName))
                       {
                           throw new UserNotFoundException("You need permissions to access PAR Command Center.");
                       }

                       var _dbContext = ctx.HttpContext.RequestServices.GetRequiredService<UserDbContext>();

                       var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == userPrincipalName.ToUpper() && !u.Deleted);

                       if (user == null)
                       {
                           throw new UserNotFoundException("You need permissions to access PAR Command Center.");
                       }
                       else
                       {
                           var query = from t in _dbContext.UserApplicationTenantSettings
                                       where t.Deleted == false
                                       where t.UserId == user.Id
                                       select new Tenant
                                       {
                                           Id = t.TenantId,
                                           Deleted = t.Deleted,
                                       };

                           var tenantIds = await query.Select(t => t.Id).ToListAsync();


                           if (string.IsNullOrEmpty(user.AzureAdObjectId))
                           {
                               user.AzureAdObjectId = oid;
                               await _dbContext.SaveChangesAsync();
                           }

                           var claims = new List<Claim>
                            {
                                    new Claim(ParClaimTypes.USER_ID, user.Id.ToString()),

                                    new Claim(ParClaimTypes.TENANT_IDS, string.Join<int>(",", tenantIds))
                            };

                           var appIdentity = new ClaimsIdentity(claims);

                           ctx.Principal.AddIdentity(appIdentity);
                       }
                   }
               };

           })
           .EnableTokenAcquisitionToCallDownstreamApi()          
           .AddInMemoryTokenCaches();

            //services.Configure<MicrosoftIdentityOptions>(options =>
            //{
            //    options.Events = new OpenIdConnectEvents
            //    {
            //        OnTokenValidated = async ctx =>
            //        {
            //            bool isAuthorizedUser = ctx.Principal.IsInRole(commandCenterSecurityGroupOptions.CommandCenterUsersGroupObjectId);

            //            if (!isAuthorizedUser)
            //            {
            //                throw new UserNotFoundException("You need permissions to access PAR Command Center.");
            //            }


            //            //Get the user's unique identifier
            //            string oid = ctx.Principal.FindFirstValue(ParClaimTypes.OBJECT_GUID);

            //            string userPrincipalName = ctx.Principal.FindFirstValue(ParClaimTypes.PREFERRED_USERNAME);

            //            if (string.IsNullOrWhiteSpace(userPrincipalName))
            //            {
            //                throw new UserNotFoundException("You need permissions to access PAR Command Center.");
            //            }

            //            var _dbContext = ctx.HttpContext.RequestServices.GetRequiredService<UserDbContext>();

            //            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == userPrincipalName.ToUpper() && !u.Deleted);

            //            if (user == null)
            //            {
            //                throw new UserNotFoundException("You need permissions to access PAR Command Center.");
            //            }
            //            else
            //            {
            //                var query = from t in _dbContext.UserApplicationTenantSettings
            //                            where t.Deleted == false
            //                            where t.UserId == user.Id
            //                            select new Tenant
            //                            {
            //                                Id = t.TenantId,
            //                                Deleted = t.Deleted,
            //                            };

            //                var tenantIds = await query.Select(t => t.Id).ToListAsync();


            //                if (string.IsNullOrEmpty(user.AzureAdObjectId))
            //                {
            //                    user.AzureAdObjectId = oid;
            //                    await _dbContext.SaveChangesAsync();
            //                }

            //                var claims = new List<Claim>
            //                {
            //                        new Claim(ParClaimTypes.USER_ID, user.Id.ToString()),

            //                        new Claim(ParClaimTypes.TENANT_IDS, string.Join<int>(",", tenantIds))
            //                };

            //                var appIdentity = new ClaimsIdentity(claims);

            //                ctx.Principal.AddIdentity(appIdentity);
            //            }
            //        }
            //    };
            //});

            //services.AddAuthentication(o =>
            //{
            //    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    o.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            //    o.DefaultForbidScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //})
            //.AddCookie(options =>
            //{
            //    options.ExpireTimeSpan = TimeSpan.FromHours(24);
            //})
            //.AddOpenIdConnect(o =>
            //{
            //    configuration.GetSection("Authentication").Bind(o);

            //    o.Events = new OpenIdConnectEvents
            //    {
            //        OnTokenValidated = async ctx =>
            //        {
            //            bool isAuthorizedUser = ctx.Principal.IsInRole(commandCenterSecurityGroupOptions.CommandCenterUsersGroupObjectId);

            //            if (!isAuthorizedUser)
            //            {
            //                throw new UserNotFoundException("You need permissions to access PAR Command Center.");
            //            }


            //            //Get the user's unique identifier
            //            string oid = ctx.Principal.FindFirstValue(ParClaimTypes.ObjectGuid);

            //            string userPrincipalName = ctx.Principal.FindFirstValue(ClaimTypes.Name);

            //            if (string.IsNullOrWhiteSpace(userPrincipalName))
            //            {
            //                throw new UserNotFoundException("You need permissions to access PAR Command Center.");
            //            }

            //            var _dbContext = ctx.HttpContext.RequestServices.GetRequiredService<UserDbContext>();

            //            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == userPrincipalName.ToUpper() && !u.Deleted);

            //            if (user == null)
            //            {
            //                throw new UserNotFoundException("You need permissions to access PAR Command Center.");
            //            }
            //            else
            //            {
            //                var query = from t in _dbContext.UserApplicationTenantSettings
            //                            where t.Deleted == false
            //                            where t.UserId == user.Id
            //                            select new Tenant
            //                            {
            //                                Id = t.TenantId,
            //                                Deleted = t.Deleted,
            //                            };

            //                var tenantIds = await query.Select(t => t.Id).ToListAsync();


            //                if (string.IsNullOrEmpty(user.AzureAdObjectId))
            //                {
            //                    user.AzureAdObjectId = oid;
            //                    await _dbContext.SaveChangesAsync();
            //                }

            //                var claims = new List<Claim>
            //                {
            //                    new Claim(ParClaimTypes.UserId, user.Id.ToString()),

            //                    new Claim(ParClaimTypes.tenantIds, string.Join<int>(",", tenantIds))
            //                };

            //                var appIdentity = new ClaimsIdentity(claims);

            //                ctx.Principal.AddIdentity(appIdentity);
            //            }
            //        }
            //    };
            //});

            return services;
        }
    }
}
