using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Par.CommandCenter.Application.Common.Behaviours;
using System.Linq;
using System.Reflection;


namespace Par.CommandCenter.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {

            Assembly a = typeof(DependencyInjection).Assembly;
            var parCommandAssemblyName = a.GetReferencedAssemblies().FirstOrDefault(x => x.Name == "Par.Command");
            Assembly parCommandAssembly = Assembly.Load(parCommandAssemblyName);


            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddMediatR(parCommandAssembly);


            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            services.AddSingleton(configuration);

            return services;
        }
    }
}
