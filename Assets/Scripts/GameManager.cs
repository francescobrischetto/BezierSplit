using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int _lives;

    private int _currentLevel;
    private int _highScore;
    private GameObject _currentSurface;

    [SerializeField] private TextMeshProUGUI _currentScoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private TextMeshProUGUI _livesText;
    [SerializeField] private Transform _sceneCenter;
    [SerializeField] private GameObject _surfacePrefab;
    [SerializeField] private GameObject _exitWindow;
    [SerializeField] private AdditionalToolsWindow _additionalToolWindow;

    private void Start()
    {
        RestartGame();
        _highScore = 0;
        _lives = 3;
        _livesText.SetText("Vite: " + _lives);
    }

    public void Update()
    {

        if (Input.GetButtonDown("Cancel"))
        {
            UnityEngine.Debug.Log("Button pressed");
            if (_exitWindow.activeSelf)
                _exitWindow.SetActive(false);
            else
                _exitWindow.SetActive(true);
        }
    }


    public void LoadNextLevel()
    {
        _additionalToolWindow.ResetRightPanel();
        Destroy(_currentSurface);

        _currentLevel += 1;
        _currentScoreText.SetText("Score: " + _currentLevel);
        if(_currentLevel >= _highScore)
        {
            _highScore = _currentLevel;
            _highScoreText.SetText("High Score: " + _currentLevel);
        }

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
        _additionalToolWindow.ResetRightPanel();
        if (_currentSurface != null)
            Destroy(_currentSurface);

        _currentLevel = 0;
        _lives = 3;
        _livesText.SetText("Vite: " + _lives);
        _currentScoreText.SetText("Score: " + _currentLevel);

        if (_currentLevel >= _highScore)
            _highScoreText.SetText("High Score: " + _currentLevel);

        _currentSurface = Instantiate(_surfacePrefab, _sceneCenter.position, _sceneCenter.rotation);
        _currentSurface.GetComponentInChildren<CubeAdjust>().SetEasy();
        _currentSurface.SetActive(true);

        _currentSurface.GetComponent<Turnable>().AdjustPosition();
        StartCoroutine(DelayTimer());
    }

    public void NextAttempt()
    {
        bool check = FindObjectOfType<CheckPercentage>().correct;
        if (check)
            LoadNextLevel();
        else
        {
            _lives--;
            _livesText.SetText("Vite: " + _lives);
            if (_lives <= 0)
                RestartGame();
        }   
    }

    public void SetLivesToZero()
    {
        _livesText.SetText("Vite: 0");
    }

    IEnumerator DelayTimer()
    {
        yield return new WaitForSeconds(0.1f);

        _currentSurface.GetComponent<Transform>().position = _sceneCenter.position;
        yield return null;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
