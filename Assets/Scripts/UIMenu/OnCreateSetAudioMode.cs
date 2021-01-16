using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnCreateSetAudioMode : MonoBehaviour
{
    public Toggle _toggle;

    private void Start()
    {
        _toggle.isOn = FindObjectOfType<AudioManager>()._audioSource.mute;
        FindObjectOfType<AudioManager>().Play();
    }

    void Update()
    {
        AudioManager manager = FindObjectOfType<AudioManager>();
        if(manager != null)
        {
            manager._audioSource.mute = _toggle.isOn;
        }
    }
}
