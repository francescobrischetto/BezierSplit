using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckPercentage : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private GameManager _manager;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _losePanel;

    [SerializeField] private TextMeshProUGUI _winPerc;
    [SerializeField] private TextMeshProUGUI _losePerc;

    public void OnClickCheckPercentage()
    {
        CubeAdjust cubeAdj = GameObject.FindGameObjectWithTag("Cube").GetComponent<CubeAdjust>();

        if ((_slider.value >= cubeAdj.percentage - (int)cubeAdj.margin && _slider.value <= cubeAdj.percentage + (int)cubeAdj.margin) ||
            (100 - _slider.value >= cubeAdj.percentage - (int)cubeAdj.margin && 100 - _slider.value <= cubeAdj.percentage + (int)cubeAdj.margin))
        {
            _winPanel.SetActive(true);
        }
        else
        {
            _losePanel.SetActive(true);
        }
    }

    public void OnClickPercentageSetText()
    {
        int perc = FindObjectOfType<CubeAdjust>().percentage;
        _winPerc.SetText("La percentuale era: " + perc + "%");
        _losePerc.SetText("La percentuale era: " + perc + "%");
    }
}
