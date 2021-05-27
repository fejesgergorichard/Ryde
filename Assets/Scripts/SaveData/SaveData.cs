[System.Serializable]
public class SaveData
{
    public int UnlockedLevel;
    public int TotalCollectedCoins;

    public SavedSettings Settings;

    public SaveData()
    {

    }
}


[System.Serializable]
public class SavedSettings
{
    public bool IsMusicMuted;
    public bool IsSoundEffectsMuted;
    public bool IsScreenRotationEnabled;
}
