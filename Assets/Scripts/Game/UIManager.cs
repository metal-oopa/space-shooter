using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _bestScoreText;

    [SerializeField]
    private Image _livesImg;

    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private Text _gameOverText;

    [SerializeField]
    private Text _restartText;

    private int _bestScore;
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        _scoreText.text = "Score: 0";
        _livesImg.sprite = _liveSprites[3];
        ShowTitleScreen();

        GameObject GameManager = GameObject.Find("Game_Manager");
        if (GameManager == null)
        {
            Debug.LogError("Game Manager Object is NULL.");
        }

        _gameManager = GameManager.GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("Game Manager Component is NULL.");
        }

        LoadBestScore();
    }

    // Update is called once per frame
    void Update() { }

    private void LoadBestScore()
    {
        _bestScore = PlayerPrefs.GetInt("BestScore", 0);
        _bestScoreText.text = "Best: " + _bestScore.ToString();
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        currentLives = Mathf.Clamp(currentLives, 0, 3);
        _livesImg.sprite = _liveSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    public void UpdateBestScore(int currentScore)
    {
        _bestScore = Math.Max(currentScore, _bestScore);
        _bestScoreText.text = "Best: " + _bestScore.ToString();

        PlayerPrefs.SetInt("BestScore", _bestScore);
    }

    private void GameOverSequence()
    {
        ShowGameOver();
        StartCoroutine(GameOverFlickerRoutine());
        _gameManager.GameOver();
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void ShowTitleScreen()
    {
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
    }

    public void ShowGameOver()
    {
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
    }
}
