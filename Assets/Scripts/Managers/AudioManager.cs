using UnityEngine;
using System;
using System.Linq;
using Saving;
using UnityEngine.Audio;

[Serializable]
internal class Sound
{
    private AudioSource source;

    public string Name;
    public AudioClip Clip;

    [Range(0f, 1f)]
    public float Volume = 0.7f;

    [Range(0.2f, 2f)]
    public float DefaultPitch = 1f;


    private float pitch = 1f;
    public float Pitch {
        get { return pitch; }
        set
        {
            pitch = Math.Min(2f, Math.Max(value, 0f));
        }
    }


    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = Clip;
        source.pitch = DefaultPitch;
    }

    public void Play(bool loop = false)
    {
        source.loop = loop;
        source.volume = Volume;
        source.pitch = Pitch;
        source.Play();
    }
    public void Play()
    {
        source.volume = Volume;
        source.pitch = Pitch;
        source.Play();
    }
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField]
    internal Sound[] Sounds;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one AudioManager in the scene!");
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        for (int i = 0; i < Sounds.Length; i++)
        {
            GameObject go = new GameObject("Sound_" + i + "_" + Sounds[i].Name);
            go.transform.SetParent(Instance.transform);
            Sounds[i].SetSource(go.AddComponent<AudioSource>());
        }
    }

    public static void PlaySound(string name)
    {
        Instance.PlaySoundLoop(name, false);
    }
    
    public static void PlaySound(string name, float pitch)
    {
        Instance.PlaySoundPitch(name, pitch);
    }

    public static void PlaySound(string name, bool loop)
    {
        Instance.PlaySoundLoop(name, loop);
    }

    public void PlayMusic()
    {
        Instance.PlaySoundLoop("Abstract-ambience", true);
    }

    public static void PlaySuspensionSound()
    {
        int count = Instance.Sounds.Count(x => x.Name.ToLower().Contains("suspension"));
        System.Random r = new System.Random();
        int suspensionSoundNumber = r.Next(count);
        string suspensionSoundName = "Suspension" + suspensionSoundNumber;
        PlaySound(suspensionSoundName);
    }

    #region Private methods

    private void PlaySoundLoop(string name, bool loop)
    {
        PlaySoundInner(GetSoundByName(name), loop);
    }

    private void PlaySoundPitch(string name, float pitch)
    {
        var sound = GetSoundByName(name);
        sound.Pitch = pitch;
        PlaySoundInner(sound, false);
    }

    private Sound GetSoundByName(string name)
    {
        return Sounds.FirstOrDefault(x => x.Name == name);
    }

    private void PlaySoundInner(Sound sound, bool loop)
    {
        if (!SaveSystem.SaveData.Settings.IsSoundEffectsMuted)
        {
            sound.Play(loop);
            Debug.Log($"Playing sound: {sound.Name}");
        }
    }

    #endregion

}
