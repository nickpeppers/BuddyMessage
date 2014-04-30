using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using BuddyMessage.Core;

namespace BuddyMessage.Droid
{
	[Service]
	public class PushHandlerService : PushHandlerServiceBase
	{
		public PushHandlerService() : base (PushConstants.ProjectNumber) 
		{

		}

		protected async override void OnRegistered (Context context, string registrationId)
		{
			Console.WriteLine("Push successfully registered!");

			var loginViewModel = ServiceContainer.Resolve<LoginViewModel>();
			try
			{
				await loginViewModel.RegisterPush(registrationId);
			}
			catch (Exception exc)
			{
				Console.WriteLine("Error registering push: " + exc);
			}
		}

		protected override void OnMessage (Context context, Intent intent)
		{
			//Pull out the notification details
			string title = intent.Extras.GetString("title");
			string message = intent.Extras.GetString("message");

			//Create a new intent
			intent = new Intent(this, typeof(ConversationsActivity));

			//Create the notification
			var notification = new Notification(Android.Resource.Drawable.SymActionEmail, title);
			notification.Flags = NotificationFlags.AutoCancel;
			notification.SetLatestEventInfo(this, title, message, PendingIntent.GetActivity(this, 0, intent, 0));

			//Send the notification through the NotificationManager
			var notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;
			notificationManager.Notify(1, notification);
		}

		protected override void OnUnRegistered(Context context, string registrationId)
		{
			Console.WriteLine("Push unregistered!");
		}

		protected override void OnError (Context context, string errorId)
		{
			Console.WriteLine("Push error: " + errorId);
		}
	}
}

