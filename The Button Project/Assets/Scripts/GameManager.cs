using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{   
    [Header("References")]
    [SerializeField] GameObject pausePanel;
    [SerializeField] Canvas mainMenuCanvas;
    [SerializeField] GameObject gameUICanvas;
    [SerializeField] Image extraLivesPlus;
    [SerializeField] Image lifePips;
    [SerializeField] EnemySpawner spawner;
    [SerializeField] Animator cameraAnim;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text highScoreText;



    [Header("Gameplay Customization")]
    [SerializeField] int currentLives;
    [SerializeField] int maxLives = 20;

    bool isPaused = false;
    bool atMainMenu = true;
    int score = 0;
    int highScore;
    
    public static GameManager Instance { get; private set; }
    public bool IsPaused => isPaused;
    


    private void Awake()
    {
        Instance = this;
        //pausePanel.SetActive(false);
        currentLives = maxLives;
        mainMenuCanvas.enabled = true;
    }

    private void Start()
    {
        scoreText.text = score.ToString();
        highScore = PlayerPrefs.GetInt("HighScore");
        highScoreText.text = highScore.ToString("n0");  // Adds comma to seperate 1000s
    }

    private void Update()
    {
        ProcessHealthStuff();
        UpdateGameUI();

        if (Input.GetKeyDown(KeyCode.Escape) && !atMainMenu)
            ProcessPauseMenu();

        //TODO: Delete this after PlayerPrefs stuff is set in stone
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            PlayerPrefs.DeleteAll();
            highScore = 0;
            highScoreText.text = highScore.ToString("n0");
        }
    }

    public void ProcessPauseMenu()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false);
        }
    }

    private void UpdateGameUI()
    {
        if (currentLives > maxLives)
            extraLivesPlus.enabled = true;
        else
            extraLivesPlus.enabled = false;

        double pipWidth = 8 + ((currentLives -1) * 5.85);   // 5.85 is width of each pip. 
        var pipTransform = lifePips.rectTransform;
        pipTransform.sizeDelta = new Vector2(Mathf.Clamp((float)pipWidth, 0, 120), pipTransform.sizeDelta.y);
    }

    private void ProcessHealthStuff()
    {
        if (currentLives <= 0)
        {
            // TODO: Add Gameover Screen here
            SceneManager.LoadScene(0);
        }
    }

    public void LowerHealth(int value)
    {
        currentLives -= value;
    }

    public void AddHealth(int value)
    {
        currentLives += value;
    }

    public void QuitTheGame()
    {
        Debug.Log("Quitting game.");
        Application.Quit();
    }

    public void StartNewGame()
    {
        mainMenuCanvas.enabled = false;
        gameUICanvas.SetActive(true);
        atMainMenu = false;
        cameraAnim.Play("Game Camera");
        spawner.ToggleEnemySpawn();
    }

    public void OpenOptionsMenu()
    {
        // TODO: Implement Options Menu

        Debug.Log("Open Options Menu.");
    }

    public void IncreaseScore(int value)
    {
        score += value;

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            //PlayerPrefs.Save();
        }

        scoreText.text = score.ToString("n0");
        highScoreText.text = highScore.ToString("n0");
    }
}
