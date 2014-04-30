using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using BuddyMessage.Core;

namespace BuddyMessage.Core
{
	public class AzureWebService : IWebService
    {
		MobileServiceClient client = new MobileServiceClient(
            "https://buddymsg.azure-mobile.net/", 
            "BlTXdkXdkXmBWjUClvtGdnhlTlQzxp24"
		);

		public AzureWebService()
		{
			CurrentPlatform.Init();
		}

		public async Task LoadData()
		{
			var users = client.GetTable<User>();
			var friends = client.GetTable<Friend>();
			var conversations = client.GetTable<Conversation>();
			var messages = client.GetTable<Message>();

			var userList = await client.GetTable<User>().Where (u => u.Username == "Nick").ToListAsync();
			var me = userList [0];
			userList = await client.GetTable<User>().Where (u => u.Username == "Chuck").ToListAsync ();
			var friend = userList [0]; 

			var conversation = new Conversation { MyId = me.Id, UserId = friend.Id, Username = friend.Username, LastMessage = "HEY!" };

			await conversations.InsertAsync(conversation);
			await messages.InsertAsync(new Message
			{
				ConversationId = conversation.Id,
				UserId = friend.Id,
				Username = friend.Username,
				Text = "What's up?",
			   	Date = DateTime.Now.AddSeconds(-60),
			});
			await messages.InsertAsync(new Message
			{
				ConversationId = conversation.Id,
				UserId = me.Id,
				Username = me.Username,
				Text = "Not much",
			   	Date = DateTime.Now.AddSeconds(-30),
			});
			await messages.InsertAsync(new Message
			{
				ConversationId = conversation.Id,
				UserId = friend.Id,
				Username = friend.Username,
				Text = "HEY!",
			   	Date = DateTime.Now,
			});
		}

		public async Task<User> Login(string username, string password)
		{
            User user = new User();
            var users = await client.GetTable<User>().Where(u => u.Username == username).ToListAsync();
            if(users.Count > 0)
            {
                user = users[0];
            }
            else
            {
			    user = new User { Username = username, Password = password };
                await client.GetTable<User>().InsertAsync(user);
            }
			return user;
		}

		public async Task<User> Register(User user)
		{
			await client.GetTable<User>().InsertAsync(user);
			return user;
		}

		public async Task<User[]> GetFriends(string userId)
		{
			var list = await client.GetTable<Friend>().Where(f => f.MyId == userId).ToListAsync();
			return list.Select(f => new User { Id = f.UserId, Username = f.Username}).ToArray();
		}

		public async Task<User> AddFriend(string userId, string username)
		{
			Friend friend = new Friend { MyId = userId, Username = username };
			var users = await client.GetTable<User>().Where(u => u.Username == username).ToListAsync();
			if(users.Count > 0)
			{
				friend.UserId = users [0].Id;
			}
			await client.GetTable<Friend>().InsertAsync(friend);
			return new User { Id = friend.UserId, Username = friend.Username };
		}

		public async Task<Conversation[]> GetConversations(string userId)
		{
			var list = await client.GetTable<Conversation>().Where(c => c.MyId == userId || c.UserId == userId).ToListAsync();
			return list.ToArray();
		}

		public async Task<Conversation> CreateConversation(Conversation conversation)
		{
			await client.GetTable<Conversation>().InsertAsync(conversation);
			var conversations = await client.GetTable<Conversation> ().Where (c => c.MyId == conversation.MyId && c.UserId == conversation.UserId).ToListAsync();
			return conversations[0];
		}

		public async Task<Message[]> GetMessages(string conversationId)
		{
			var list = await client.GetTable<Message>().Where(m => m.ConversationId == conversationId).ToListAsync();
			return list.ToArray();
		}

		public async Task<Message> SendMessage(Message message)
		{
			await client.GetTable<Message>().InsertAsync(message);
			return message;
		}

		public async Task RegisterPush(string userId, string deviceToken)
		{
			await client.GetTable<Device>().InsertAsync(new Device { UserId = userId, DeviceToken = deviceToken });
		}
    }
}

