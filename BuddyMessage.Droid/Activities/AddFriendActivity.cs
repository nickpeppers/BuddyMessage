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
using BuddyMessage.Core;

namespace BuddyMessage.Droid
{
    [Activity(Label = "AddFriendActivity")]			
	public class AddFriendActivity : BaseActivity<FriendViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

			SetContentView (Resource.Layout.AddFriend);

			var addFriendEditText = FindViewById<EditText> (Resource.Id.AddFriendEditText);
			var addFriendButton = FindViewById<Button> (Resource.Id.AddFriendButton);

			addFriendButton.Click += async (sender, e) => 
			{
				if(string.IsNullOrEmpty(addFriendEditText.Text))
					return;
				else
				{
					try
					{
						viewModel.Username = addFriendEditText.Text;
						await viewModel.AddFriend();
					}
					catch (Exception exc)
					{

					}
					finally
					{
						Finish();
					}
				}
			};
        }
    }
}

