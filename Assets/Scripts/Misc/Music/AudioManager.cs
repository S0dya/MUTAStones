using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : SingletonMonobehaviour<AudioManager>
{
    [Header("Sounds")]
    [SerializeField] KeyValueSound[] kvSounds;

    Dictionary<string, EventInstance> eventInstancesDict = new Dictionary<string, EventInstance>();
    
    //initialization
    void Start()
    {
        foreach (KeyValueSound kvSound in kvSounds) eventInstancesDict.Add(kvSound.name, CreateInstance(kvSound.sound));
    }
    EventInstance CreateInstance(EventReference sound)
    {
        return RuntimeManager.CreateInstance(sound);
    }

    //main methods
    public void PlayOneShot(string sound) => eventInstancesDict[sound].start();
    public void PlayOneShot(EventInstance sound) => sound.start();

    //settings
    public void SetVolume(int index, float volume)
    {
        RuntimeManager.GetBus((index == 0? "bus:/MUSIC" : "bus:/SFX")).setVolume(volume);
        Settings.musicStats[index] = volume;
    }

    public void ToggleMusic(bool toggle)
    {
        Settings.isMusicOn = toggle;
        RuntimeManager.GetBus("bus:/").setVolume(toggle ? 1 : 0);
    }
}

[System.Serializable]
class KeyValueSound
{
    [SerializeField] public string name;
    [SerializeField] public EventReference sound;
}

[System.Serializable]
class KeyValuesSounds
{
    [SerializeField] public string name;
    [SerializeField] public EventReference[] sounds;
}
