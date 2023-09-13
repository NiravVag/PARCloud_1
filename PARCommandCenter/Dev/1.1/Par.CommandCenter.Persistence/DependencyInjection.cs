using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Persistence.Data;
using Par.Data;
using Par.Data.Context;
using System;

namespace Par.CommandCenter.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<FullContext>(options =>
               options.UseSqlServer(
                   configuration.GetConnectionString("DefaultConnection")));

            ////services.AddDbContext<ApplicationDbContext>(options =>
            ////{
            ////    var dbConnection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"))
            ////    {
            ////        AccessToken = new AzureServiceTokenProvider().GetAccessTokenAsync("https://database.windows.net/").Result
            ////    };
            ////    options.UseSqlServer(dbConnection);
            ////}
            ////);

            //services.AddDbContext<ApplicationDbContext>(options =>
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

            //services.AddDbContext<FullContext>(options =>
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
            //         sqlServerOptionsAction: sqlOptions =>
            //         {
            //             sqlOptions.EnableRetryOnFailure(
            //             maxRetryCount: 10,
            //             maxRetryDelay: TimeSpan.FromSeconds(30),
            //             errorNumbersToAdd: null);
            //         }
            //     );
            //}, ServiceLifetime.Scoped);

            services.AddScoped<IApplicationDbContext>(x => x.GetService<ApplicationDbContext>());

            services.AddScoped<IFullContext>(x => x.GetService<FullContext>());

            return services;
        }
    }
}
