using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderPercentages : MonoBehaviour
{
    private int _currentValue;

    [SerializeField] private TextMeshProUGUI _topPerc;
    [SerializeField] private TextMeshProUGUI _bottomPerc;
    [SerializeField] private Slider _slider;

    // Update is called once per frame
    void Update()
    {
        _currentValue = (int)_slider.value;
        _topPerc.SetText("" + _currentValue + " %");
        _bottomPerc.text = "" + (100-_currentValue) + " %";
    }
}
