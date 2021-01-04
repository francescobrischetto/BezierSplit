using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
