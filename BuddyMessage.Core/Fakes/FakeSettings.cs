using System;

namespace BuddyMessage.Core
{
    public class FakeSettings : ISettings
    {
        public User User { get; set; }

        public void Save() { }
    }
}

