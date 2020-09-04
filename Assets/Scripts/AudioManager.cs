using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public static AudioManager Instance { get => _instance; }

    public AudioClip buttonClickedSound;
    public AudioClip placeBuildingSound;
    public AudioClip removeBuildingSound;
    public AudioSource effectAudioSource;

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            _instance = this;
        }
    }

    public void PlayPlaceBuildingSound()
    {
        effectAudioSource.Stop();
        effectAudioSource.clip = placeBuildingSound;
        effectAudioSource.Play();
    }

    public void PlayDemolitionSound()
    {
        effectAudioSource.Stop();
        effectAudioSource.clip = removeBuildingSound;
        effectAudioSource.Play();
    }

    public void PlayButtonClickedSound()
    {
        effectAudioSource.Stop();
        effectAudioSource.clip = buttonClickedSound;
        effectAudioSource.Play();
    }


}
