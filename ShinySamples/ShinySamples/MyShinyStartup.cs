using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shiny;

namespace ShinySamples
{
    public class MyShinyStartup : ShinyStartup
    {
        //public override void ConfigureLogging(ILoggingBuilder builder, IPlatform platform)
        //{
        //    builder.AddFirebase(LogLevel.Warning);
        //    builder.AddAppCenter("YourAppCenterKey", LogLevel.Warning);
        //}

        public override void ConfigureServices(IServiceCollection services, IPlatform platform)
        {

            // this is where you'll load things like BLE, GPS, etc - those are covered in other sections
            // things like the jobs, environment, power, are all installed automatically
            //services.UseNotifications <Shiny.Notifications.INotificationDelegate> ();
            services.UseNotifications();
        }
    }
}
