using System;
using System.Threading.Tasks;

namespace BuddyMessage.Core
{
    public interface IWebService
    {
        Task<User> Login(string username, string password);

        Task<User> Register(User user);

		Task<User[]> GetFriends(string userId);

		Task<User> AddFriend(string userId, string username);

		Task<Conversation[]> GetConversations(string userId);

		Task<Conversation> CreateConversation(Conversation conversation);

		Task<Message[]> GetMessages(string conversationId);

        Task<Message> SendMessage(Message message);

		Task RegisterPush(string userId, string deviceToken);
    }
}

