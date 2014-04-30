using System;
using System.Threading.Tasks;

namespace BuddyMessage.Core
{
    public class LoginViewModel : BaseViewModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public async Task Login()
        {
            if (string.IsNullOrEmpty(Username))
                throw new Exception("Username is blank.");

            if (string.IsNullOrEmpty(Password))
                throw new Exception("Password is blank.");

            IsBusy = true;
            try
            {
                settings.User = await service.Login(Username, Password);
                settings.Save();
				//service.LoadData();
            }
            finally
            {
                IsBusy = false;
            }
        }

		public async Task RegisterPush(string deviceToken)
		{
			if (settings.User == null)
				throw new Exception("User is null");

			await service.RegisterPush(settings.User.Id, deviceToken);
		}
    }
}

