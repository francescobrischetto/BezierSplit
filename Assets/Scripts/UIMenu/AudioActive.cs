using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioActive : MonoBehaviour
{
    public bool _audioMute = false;

    public void SetAudioMode(bool var)
    {
        FindObjectOfType<AudioManager>()._audioSource.mute = var;
    }
}
