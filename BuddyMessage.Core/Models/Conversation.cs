using System;

namespace BuddyMessage.Core
{
    public class Conversation
    {
		public string Id { get; set; }

		public string MyId { get; set; }

		public string UserId { get; set; }

        public string Username { get; set; }

        public string LastMessage { get; set; }
    }
}

