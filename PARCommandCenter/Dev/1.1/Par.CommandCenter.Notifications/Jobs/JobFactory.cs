//using Microsoft.Extensions.DependencyInjection;
//using Quartz;
//using Quartz.Spi;
//using System;


//namespace Par.CommandCenter.Notifications.Jobs
//{
//    public class JobFactory : IJobFactory
//    {
//        private readonly IServiceProvider _serviceProvider;

//        public JobFactory(IServiceProvider serviceProvider)
//        {
//            _serviceProvider = serviceProvider;
//        }

//        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
//        {
//            throw new NotImplementedException();
//        }

//        public void ReturnJob(IJob job)
//        {
//            throw new NotImplementedException();
//        }

//        //public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
//        //{
//        //    return _serviceProvider.GetService<NotificationsJob>();
//        //}

//        //public void ReturnJob(IJob job)
//        //{
//        //    var disposable = job as IDisposable;
//        //    disposable?.Dispose();
//        //}
//    }
//}
