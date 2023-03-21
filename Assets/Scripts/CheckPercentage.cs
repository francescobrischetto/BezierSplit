using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckPercentage : MonoBehaviour
{
    public bool correct;

    [SerializeField] private Slider _slider;
    [SerializeField] private GameManager _manager;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _restartPanel;
    [SerializeField] private GameObject _retryPanel;

    [SerializeField] private TextMeshProUGUI _winPerc;
    [SerializeField] private TextMeshProUGUI _losePerc;

    public void OnClickCheckPercentage()
    {
        CubeAdjust cubeAdj = GameObject.FindGameObjectWithTag("Cube").GetComponent<CubeAdjust>();

        if ((_slider.value >= cubeAdj.percentage - (int)cubeAdj.margin && _slider.value <= cubeAdj.percentage + (int)cubeAdj.margin) ||
            (100 - _slider.value >= cubeAdj.percentage - (int)cubeAdj.margin && 100 - _slider.value <= cubeAdj.percentage + (int)cubeAdj.margin))
        {
            correct = true;
            EnableWinMessage();
        }
        else
        {
            correct = false;
            if (FindObjectOfType<GameManager>()._lives > 1)
                EnableRetryMessage();
            else
            {
                UnityEngine.Debug.Log("FATTO");
                _manager.SetLivesToZero();
                EnableRestartMessage();
            }
                
        }
    }

    public void EnableWinMessage()
    {
        _winPanel.SetActive(true);
        int perc = FindObjectOfType<CubeAdjust>().percentage;
        _winPerc.SetText("La percentuale era: " + perc + "%-" + (100-perc) + "%");
    }

    public void EnableRetryMessage()
    {
        _retryPanel.SetActive(true);
    }

    public void EnableRestartMessage()
    {
        _restartPanel.SetActive(true);
        int perc = FindObjectOfType<CubeAdjust>().percentage;
        _losePerc.SetText("La percentuale era: " + perc + "%-" + (100 - perc) + "%");
    }
}
