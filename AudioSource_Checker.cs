using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSource_Checker : MonoBehaviour
{
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if(audioSource != null)
        {
            //AudioManager.Instance.GetSoundByClip(audioSource.clip).sources.Add(audioSource);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
