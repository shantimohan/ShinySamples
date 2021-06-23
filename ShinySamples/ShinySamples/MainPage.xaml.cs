using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Shiny;
using Shiny.Notifications;

namespace ShinySamples
{
    public partial class MainPage : ContentPage
    {
		INotificationManager manager;
		string channelId = "Shiny Notification Channel";
		int messageId = 1;
		bool ChannelCreated = false;

		public MainPage()
        {
            InitializeComponent();

			ScheduleDate.Date = DateTime.Today;
			ScheduleTime.Time = DateTime.Now.TimeOfDay.Add(new TimeSpan(0, 10, 0));

			manager = ShinyHost.Resolve<INotificationManager>();
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();

			AccessState result = await manager.RequestAccess();
			if (result == AccessState.Available)
            {
				List<Channel> channelsList = await manager.GetChannels() as List<Channel>;

				if (channelsList.Count == 0)
                {
					ChannelStatus.Text += "No Channels created";
                }
				else
                {
					Channel chnl = channelsList.Where(x => x.Identifier == channelId).FirstOrDefault();

					if (chnl == null)
                    {
						ChannelStatus.Text += $"'{channelId}' not yet created";
                    }
					else
                    {
						ChannelStatus.Text += $"'{channelId}' is already created";
						ChannelCreated = true;
						CreateChannel.IsEnabled = false;
						ClearChannels.IsEnabled = !ClearChannels.IsEnabled;
						SendNotifiction.IsEnabled = true;
						ScheduleNotifiction.IsEnabled = true;

						var pendingList = await manager.GetPending();
						messageId = 0;

						foreach (Notification ntf in pendingList)
                        {
							if (ntf.Id > messageId)
								messageId = ntf.Id;
                        }

						messageId++;
					}
				}
			}
		}

        private async void CreateChannel_Clicked(object sender, EventArgs e)
		{
			AccessState result = await manager.RequestAccess();
			if (result == AccessState.Available && !ChannelCreated)
            {
				await manager.AddChannel(new Channel
				{
					Identifier = channelId,
					Description = channelId,
					Importance = ChannelImportance.Normal,
					//	Actions =
					//	{
					//		new ChannelAction
					//		{
					//			Identifier = "message",
					//			Title = "Leave a Message",
					//			ActionType = ChannelActionType.TextReply
					//		},
					//		new ChannelAction
					//		{
					//			Identifier = "freeticket",
					//			Title = "Free Ticket",
					//			ActionType = ChannelActionType.Destructive
					//		}
					//	}
				});

				List<Channel> channelsList = await manager.GetChannels() as List<Channel>;

				Channel chnl = channelsList.Where(x => x.Identifier == channelId).FirstOrDefault();

				if (chnl == null)
				{
					ChannelStatus.Text = $"Channel Status: '{channelId}' not created";
					ChannelCreated = false;
					CreateChannel.IsEnabled = true;
					ClearChannels.IsEnabled = !CreateChannel.IsEnabled;
					SendNotifiction.IsEnabled = false;
					ScheduleNotifiction.IsEnabled = false;
				}
				else
				{
					ChannelStatus.Text = $"Channel Status: '{channelId}' is created";
					ChannelCreated = true;
					CreateChannel.IsEnabled = false;
					ClearChannels.IsEnabled = !CreateChannel.IsEnabled;
					SendNotifiction.IsEnabled = true;
					ScheduleNotifiction.IsEnabled = true;
				}
			}
		}

		private async void ClearChannels_Clicked(object sender, EventArgs e)
		{
			AccessState result = await manager.RequestAccess();
			if (result == AccessState.Available)
			{
				await manager.ClearChannels();

				ChannelStatus.Text = $"Channel Status: All channels cleared";
				ChannelCreated = false;
				CreateChannel.IsEnabled = true;
				ClearChannels.IsEnabled = !CreateChannel.IsEnabled;
				SendNotifiction.IsEnabled = false;
				ScheduleNotifiction.IsEnabled = false;
			}
		}

		private async void SendNotifiction_Clicked(object sender, EventArgs e)
        {
			Button btn = (Button)sender;

			DateTime dt = ScheduleDate.Date;
			dt = new DateTime(dt.Year, dt.Month, dt.Day, ScheduleTime.Time.Hours, ScheduleTime.Time.Minutes, 0);

			string message = $"Id: {channelId}";
			if (btn.Text.StartsWith("Schedule"))
				message += $" Scheduled for {dt.ToShortDateString()} {dt.ToShortTimeString()}";

			AccessState result = await manager.RequestAccess();
            if (result == AccessState.Available)
            {
				Notification notification = new Notification
				{
					Title = "WELCOME TO Local Notifications by Shiny",
					Message = message,
					Channel = channelId,
					Id = messageId
				};

				if (btn.Text.StartsWith("Sch"))
					notification.ScheduleDate = dt;

				await manager.Send(notification);

				//if (btn.Text.StartsWith("Send"))
    //            {
				//	await manager.Send(
				//		"WELCOME TO Local Notifications by Shiny",
				//		message,
				//		channelId
				//	);
				//}
				//else
    //            {
				//	await manager.Send(
				//		"WELCOME TO Local Notifications by Shiny",
				//		message,
				//		channelId, dt
				//	);
				//}

				messageId++;
			}
		}


		private async void ListChannels_Clicked(object sender, EventArgs e)
        {
			AccessState result = await manager.RequestAccess();
			if (result == AccessState.Available)
            {
				var channelsList = await manager.GetChannels();
				List<string> list = new List<string>();
				
				foreach (Channel chnl in channelsList)
                {
					string desc = $"ID: {chnl.Identifier}";
					list.Add(desc);
                }

				NotificationsList.ItemsSource = list;
            }

		}

		private async void ListPending_Clicked(object sender, EventArgs e)
		{
			AccessState result = await manager.RequestAccess();
			if (result == AccessState.Available)
			{
				var pendingList = await manager.GetPending();
				List<string> list = new List<string>();

				foreach (Notification ntf in pendingList)
				{
					string desc = $"{ntf.Id} {ntf.Message.Substring(ntf.Message.IndexOf("Sch"))}";
					list.Add(desc);
				}

				NotificationsList.ItemsSource = list;
			}
		}

        private async void CancelMessage_Clicked(object sender, EventArgs e)
        {
			string[] s = NotificationsList.SelectedItem.ToString().Split(' ');
			int id = Convert.ToInt32(s[0]);

			AccessState result = await manager.RequestAccess();
			if (result == AccessState.Available)
            {
				await manager.Cancel(id);
            }

		}
    }
}
