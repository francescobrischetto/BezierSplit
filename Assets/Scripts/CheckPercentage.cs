using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPercentage : MonoBehaviour
{
    public Slider s;
    public CubeAdjust c;
    public void OnClickCheckPercentage()
    {
        if ((s.value >= c.percentage - c.margin && s.value <= c.percentage + c.margin) || (100 - s.value >= c.percentage - c.margin && 100 - s.value <= c.percentage + c.margin))
        {
            Debug.Log("Win");
        }
        else Debug.Log("Lose");
    }

}
