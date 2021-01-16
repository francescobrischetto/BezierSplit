using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnCreateSetCheatMode : MonoBehaviour
{
    public Toggle _toggle;

    private void Start()
    {
        _toggle.isOn = FindObjectOfType<CheatMode>()._cheatMode;
    }

    void Update()
    {
        FindObjectOfType<CheatMode>()._cheatMode = _toggle.isOn;
    }
}
