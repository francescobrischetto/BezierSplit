using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnCreateSetCheatMode : MonoBehaviour
{
    public Toggle _toggle;

    void Start()
    {
        FindObjectOfType<CheatMode>()._cheatMode = _toggle.isOn;
    }
}
