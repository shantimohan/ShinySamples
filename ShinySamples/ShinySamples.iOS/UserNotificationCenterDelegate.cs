﻿using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using UserNotifications;

namespace ShinySamples.iOS
{
    public class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate

    {
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {

            // Tell system to display the notification anyway or use

            // `None` to say we have handled the display locally.

            completionHandler(UNNotificationPresentationOptions.Alert);
        }

    }
}