using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPercentage : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private GameManager _manager;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _losePanel;

    public void OnClickCheckPercentage()
    {
        CubeAdjust cubeAdj = GameObject.FindGameObjectWithTag("Cube").GetComponent<CubeAdjust>();

        if ((_slider.value >= cubeAdj.percentage - (int)cubeAdj.margin && _slider.value <= cubeAdj.percentage + (int)cubeAdj.margin) ||
            (100 - _slider.value >= cubeAdj.percentage - (int)cubeAdj.margin && 100 - _slider.value <= cubeAdj.percentage + (int)cubeAdj.margin))
        {
            _winPanel.SetActive(true);
            Debug.Log("Win");
        }
        else
        {
            _losePanel.SetActive(true);
            Debug.Log("Lose");
        }
    }

}
