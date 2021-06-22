using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shiny;
using Shiny.Notifications;

namespace ShinySamples
{
    public class NotificationDelegate : INotificationDelegate
    {
		public NotificationDelegate()
        {

        }

		public Task OnEntry(NotificationResponse response)
		{
			return Task.CompletedTask;
		}
	}
}
