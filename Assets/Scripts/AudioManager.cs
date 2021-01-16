using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<CheatMode>
{
    public AudioSource _audioSource;

    public void Play()
    {
        if(!_audioSource.isPlaying)
            _audioSource.Play();
    }
}
