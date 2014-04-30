using System;

namespace BuddyMessage.Core
{
    public interface ISettings
    {
        User User { get; set; }

        void Save();
    }
}

