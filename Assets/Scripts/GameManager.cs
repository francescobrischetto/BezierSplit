using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int _currentLevel;
    private GameObject _currentSurface;

    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Transform _sceneCenter;
    [SerializeField] private GameObject _surfacePrefab;

    private void Start()
    {
        RestartGame();
    }

    public void LoadNextLevel()
    {
        Destroy(_currentSurface);

        _currentLevel += 1;
        _levelText.SetText("Livello " + _currentLevel);

        _currentSurface = Instantiate(_surfacePrefab, _sceneCenter.position, _sceneCenter.rotation);
        if(_currentLevel < 5)
            _currentSurface.GetComponentInChildren<CubeAdjust>().SetEasy();
        else if (_currentLevel < 10)
            _currentSurface.GetComponentInChildren<CubeAdjust>().SetMedium();
        else if (_currentLevel < 15)
            _currentSurface.GetComponentInChildren<CubeAdjust>().SetHard();
        else
            _currentSurface.GetComponentInChildren<CubeAdjust>().SetImpossible();
        _currentSurface.SetActive(true);

        _currentSurface.GetComponent<Turnable>().AdjustPosition();
        StartCoroutine(DelayTimer());
    }

    public void RestartGame()
    {
        if(_currentSurface != null)
            Destroy(_currentSurface);

        _currentLevel = 1;
        _levelText.SetText("Livello " + _currentLevel);

        _currentSurface = Instantiate(_surfacePrefab, _sceneCenter.position, _sceneCenter.rotation);
        _currentSurface.GetComponentInChildren<CubeAdjust>().SetEasy();
        _currentSurface.SetActive(true);

        _currentSurface.GetComponent<Turnable>().AdjustPosition();
        StartCoroutine(DelayTimer());
    }

    IEnumerator DelayTimer()
    {
        yield return new WaitForSeconds(0.1f);

        _currentSurface.GetComponent<Transform>().position = _sceneCenter.position;
        yield return null;
    }
}
