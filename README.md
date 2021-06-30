# ShinySamples
Local Notifications using Shiny NuGet package

I did some experiments to set local notifications as I described in the Xamarin Forms forum thread [How to schedule a local notification or set an Alarm in Xamarin Android?](https://forums.xamarin.com/discussion/155888/how-to-schedule-a-local-notification-or-set-an-alarm-in-xamarin-android#latest). I also uploaded my trial project [LocalNotifications_XA](https://github.com/shantimohan/LocalNotifications_XA) to GitHub. That was not a perfect one as I was facing issues.

Then I found the NuGet package **Xam.Plugins.Notifier** by James Montemagno. It was quite straight forward and I started using it.

But this plugin has become unusable when I switched to use AndroidX as forced by Xamarin.Forms ver 5.0 upgrade. I did migrate my project to XF 5.0 successfully but the this plugin due to its reference to ***Xamarin.Android.Support.V4***, now my XF solution is having build issues. I have to build the Android project twice. First time the build fails due to legacy references and build successfully only on 2n run.

Though this is not a show stopper it is quite annoying. I started searching for a solution to run it without any problems. Then I noticed a reference to [Shiny 2.0](https://allanritchie.com/posts/shiny20) in the YouTube video of [Xamarin Community Standup - .NET MAUI Single Project with Jake Kirsch](https://www.youtube.com/watch?v=AQHZQ8p6FCA) at about 5:00.

This solution is the result of what is described in [Shiny.NET](https://shinylib.net/). I have tested this in Android and iOS. Testing in UWP is not yet done.

1. As per requirement creation of only one channel is sufficient. Of course this can be modified to create as many channels as needed.
1. I have made provision to list channels, list pending messages and also to delete a pending message. The later two methods will be quite useful.
1. Previous method of using the Xam.Plugins.Notifier had a draw back on Android that it the device is rebooted all existing pending notificatios were also cleared. In using Shiny, so far I have tested only on emulators and the pending notifications were not lost on rebooting the emulators. I am yet to confirm it on a real device.

You can send me any feedback on this solution.
