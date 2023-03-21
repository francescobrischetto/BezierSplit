using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdditionalToolsWindow : MonoBehaviour
{
    [SerializeField] private GameObject _rightUIPanel;

    public void DisplayPanel()
    {
        if(_rightUIPanel.activeInHierarchy)
        {
            _rightUIPanel.SetActive(false);
        } else
        {
            _rightUIPanel.SetActive(true);
        }
    }

    public void ResetRightPanel()
    {
        Toggle[] toggles = _rightUIPanel.gameObject.GetComponentsInChildren<Toggle>();
        foreach (Toggle t in toggles)
        {
            t.isOn = false;
        }
    }
}
