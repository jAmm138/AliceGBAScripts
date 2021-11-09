using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager Instance;
    void Awake()
    {
        Instance = this;

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    void Update()
    {
        Volume();
    }

    #region Update

    void Volume()
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            
        }
    }

    #endregion

    #region Controls

    public void Play_AudioSource(AudioSource ass)
    {
        ass.Play();
    }
    public void Stop_AudioSource(AudioSource ass)
    {
        ass.Stop();
    }

    public void Play(string soundName)
    {
        if (GetAudioByString(soundName) == null)
        {
            Debug.Log(GetAudioByString(soundName) + " audio not found.");
            return;
        }

        GetAudioByString(soundName).source.Play();
    }

    public void Stop(string soundName)
    {
        if (GetAudioByString(soundName) == null)
        {
            Debug.Log(GetAudioByString(soundName) + " audio not found.");
            return;
        }

        GetAudioByString(soundName).source.Stop();
    }

    #endregion

    #region Helper Method

    public Sound GetAudioByString(string audio)
    {
        Sound r = null;

        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].soundName == audio)
            {
                r = sounds[i];
            }
        }

        return r;
    }

    public Sound GetSoundByClip(AudioClip ac)
    {
        Sound r = null;

        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].clip == ac)
            {
                r = sounds[i];
            }
        }

        return r;
    }

    #endregion
}
