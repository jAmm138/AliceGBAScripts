using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioLevels : MonoBehaviour
{
    [Header("Volume")]
    [Range(0f, 1f)]
    public float masterVol, soundVol, musicVol, ambienceVol, voiceVol;

    [Header("Mixer Groups")]
    public AudioMixerGroup masterVolumeGroup, soundGroup, musicGroup, ambienceGroup, voiceGroup;

    public static AudioLevels Instance;

    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
