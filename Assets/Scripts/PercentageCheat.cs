using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PercentageCheat : MonoBehaviour
{
    public bool _active;

    [SerializeField] private GameObject _percentageText;
    [SerializeField] private GameObject _percentageValue;


    void Start()
    {
        _active = FindObjectOfType<CheatMode>()._cheatMode;

        if (_active)
        {
            _percentageText.SetActive(true);
            _percentageValue.SetActive(true);
        }
    }


    void Update()
    {
        if(_active)
        {
            int perc = FindObjectOfType<CubeAdjust>().percentage;
            _percentageValue.GetComponent<TextMeshProUGUI>().SetText(perc + "%");
        }
    }
}
