using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver = false;
    [SerializeField]
    private bool _isCoOpMode = false;
    [SerializeField]
    private GameObject _pauseMenuPanel;
    
    private bool _isGamePaused = false;

    // Start is called before the first frame update
    void Start(){ }

    // Update is called once per frame
    void Update()
    {
        SpeedUpGame();
        CheckPause();
        CheckRestartGame();
        CheckBackToMainMenu();
    }

    private void SpeedUpGame()
    {
        Time.timeScale += (0.005f * Time.deltaTime);
    }

    private void CheckPause()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleGamePause();
        }
    }

    private void CheckRestartGame()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
        {
            SceneManager.LoadScene(1); // current game scene
        }
    }

    private void CheckBackToMainMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMainMenu();
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0); // main menu scene
    }

    public void ToggleGamePause()
    {
        _isGamePaused = !_isGamePaused;
        Time.timeScale = _isGamePaused ? 0 : 1;
        _pauseMenuPanel.SetActive(_isGamePaused);
    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    public bool IsCoOpMode()
    {
        return _isCoOpMode;
    }
}
