using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialMenuScript : MonoBehaviour
{
    private int _current;
    private List<GameObject> _texts = new List<GameObject>();

    [SerializeField] private GameObject _text0;
    [SerializeField] private GameObject _text1;
    [SerializeField] private GameObject _text2;
    [SerializeField] private GameObject _text3;
    [SerializeField] private TextMeshProUGUI _status;


    void Awake()
    {
        _current = 0;

        _texts.Add(_text0);
        _texts.Add(_text1);
        _texts.Add(_text2);
        _texts.Add(_text3);

        ShowCurrentText();
    }


    private void ShowCurrentText()
    {
        for(int i=0; i<4; i++)
        {
            if (i == _current)
                _texts[i].SetActive(true);
            else
                _texts[i].SetActive(false);
        }
        _status.SetText((_current+1) + "/4");
    }


    public void NextText()
    {
        _current += 1;
        _current %= 4;
        ShowCurrentText();
    }


    public void PreviousText()
    {
        _current += 3;
        _current %= 4;
        ShowCurrentText();
    }


    public void ResetText()
    {
        _current = 0;
        ShowCurrentText();
    }
}
