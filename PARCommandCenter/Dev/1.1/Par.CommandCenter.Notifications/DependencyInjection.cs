using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Par.CommandCenter.Notifications.Jobs;
using Par.CommandCenter.Notifications.Services;
using Par.CommandCenter.Notifications.Services.Email;
using Quartz;

namespace Par.CommandCenter.Notifications
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddNotifications(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IEmailConfiguration>(configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
            services.AddTransient<IEmailService, EmailService>();

            services.AddScoped<ICommunicationService, CommunicationService>();

            services.AddScoped<IVPNNotificationService, VPNNotificationService>();

            ////services.AddScoped<IRouterNotificationService, RouterNotificationService>();

            services.AddScoped<IControllerNotificationService, ControllerNotificationService>();

            services.AddScoped<IServerOperationNotificationService, ServerOperationNotificationService>();

            services.AddScoped<IInterfaceNotificationService, InterfaceNotificationService>();

            var quartzOptions = configuration.GetSection("Quartz").Get<QuartzOptions>();

            services.AddSingleton<QuartzOptions>(quartzOptions);


            // if you are using persistent job store, you might want to alter some options
            services.Configure<QuartzOptions>(options =>
            {
                options.Scheduling.IgnoreDuplicates = true; // default: false
                options.Scheduling.OverWriteExistingData = true; // default: true
            });


            services.AddScoped<NotificationsJob>();

            ////services.AddScoped<ReminderNotificationsJob>();

            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();

                //// Register the job, loading the schedule from configuration
                ////q.AddJobAndTrigger<ReminderNotificationsJob>(configuration);

                // Register the job, loading the schedule from configuration
                q.AddJobAndTrigger<NotificationsJob>(configuration);
            });


            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);



            ////var serviceProvider = services.BuildServiceProvider();

            ////_ = ScheduleJob(serviceProvider);

            return services;

        }

        ////private static async Task ScheduleJob(IServiceProvider serviceProvider)
        ////{
        ////    var props = new NameValueCollection
        ////    {
        ////        { "quartz.serializer.type", "binary" }
        ////    };

        ////    var factory = new StdSchedulerFactory(props);

        ////    var sched = await factory.GetScheduler();

        ////    sched.JobFactory = new JobFactory(serviceProvider);

        ////    await sched.Start();

        ////    //var job = JobBuilder.Create<NotificationsJob>()
        ////    //    .WithIdentity("healthCheckNotificationsJob")
        ////    //    .Build();

        ////    //var trigger = TriggerBuilder.Create()
        ////    //    .WithIdentity("healthCheckNotificationTrigger")
        ////    //    .StartNow()
        ////    //    .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(10)))
        ////    //    //.WithDailyTimeIntervalSchedule(x => x.WithInterval(15, IntervalUnit.Minute))
        ////    //    //.WithDailyTimeIntervalSchedule(x => x.WithInterval(5, IntervalUnit.Minute))
        ////    //    .WithDailyTimeIntervalSchedule(x => x.WithInterval(2, IntervalUnit.Minute))
        ////    ////.WithSimpleSchedule(x => x
        ////    ////    .WithIntervalInSeconds(5)
        ////    ////    .RepeatForever())
        ////    //.Build();

        ////    //await sched.ScheduleJob(job, trigger);

        ////    var job2 = JobBuilder.Create<ReminderNotificationsJob>()
        ////        .WithIdentity("healthCheckReminderNotificationsJob")
        ////        .Build();

        ////    var trigger2 = TriggerBuilder.Create().WithCronSchedule("0 27 8 ? * *").Build();

        ////    await sched.ScheduleJob(job2, trigger2);
        ////}
    }
}
