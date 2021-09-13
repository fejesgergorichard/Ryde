using System;

namespace Saving
{
    [Serializable]
    public class SaveData
    {
        public int UnlockedLevel;
        public int TotalCollectedCoins;

        public SavedSettings Settings;

        public SaveData()
        {

        }

        public SaveData(bool test)
        {
            UnlockedLevel = 4;
            TotalCollectedCoins = 1234;
            Settings = new SavedSettings() 
            { 
                IsMusicMuted = false, 
                IsScreenRotationEnabled = false, 
                IsSoundEffectsMuted = false
            };
        }
    }


    [Serializable]
    public class SavedSettings
    {
        public bool IsMusicMuted;
        public bool IsSoundEffectsMuted;
        public bool IsScreenRotationEnabled;
    }
}
