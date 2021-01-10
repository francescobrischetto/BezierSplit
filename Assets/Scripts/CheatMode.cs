using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatMode : Singleton<CheatMode>
{
    public bool _cheatMode = false;

    public void SetCheatMode(bool var)
    {
        _cheatMode = var;
    }
}
