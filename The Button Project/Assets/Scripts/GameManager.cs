using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{   
    [Header("References")]
    [SerializeField] GameObject pausePanel;
    [SerializeField] Canvas mainMenuCanvas;
    [SerializeField] GameObject gameUICanvas;
    [SerializeField] OptionsMenu optionsMenu;
    [SerializeField] AudioManager audioManager;
    [SerializeField] Image extraLivesPlus;
    [SerializeField] Image lifePips;
    [SerializeField] EnemySpawner spawner;
    [SerializeField] Animator cameraAnim;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text highScoreText;
    [SerializeField] GameObject[] powerupShields;
    [SerializeField] TMP_Text buffText;

    [Header("Gameplay Customization")]
    [SerializeField] int currentLives;
    [SerializeField] int maxLives = 20;

    bool isPaused = false;
    bool atMainMenu = true;
    bool shieldsActive = false;
    float shieldTimeLeft;
    int score = 0;
    int highScore;

    // PlayerPrefs
    public float masterVolumeValue;
    public float musicVolumeValue;
    public float sfxVolumeValue;
    public int difficulty;
    

    public static GameManager Instance { get; private set; }
    public bool IsPaused => isPaused;
    


    private void Awake()
    {
        Instance = this;
        currentLives = maxLives;
        mainMenuCanvas.enabled = true;
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnEnable()
    {
        ShieldPowerUp.OnPowerupAquired += ActivateShieldPowerup;
        LoadPlayerPrefs();
    }

    private void OnDisable()
    {
        ShieldPowerUp.OnPowerupAquired -= ActivateShieldPowerup;
    }

    private void Start()
    {
        scoreText.text = score.ToString();
        highScore = PlayerPrefs.GetInt("HighScore");
        highScoreText.text = highScore.ToString("n0");  // Adds comma to seperate 1000s
        audioManager.AdjustAudioFromPrefs();
    }

    private void Update()
    {
        ProcessHealthStuff();
        UpdateGameUI();

        buffText.text = shieldTimeLeft.ToString("n0");


        if (Input.GetKeyDown(KeyCode.Escape) && !atMainMenu)
            ProcessPauseMenu();

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ClearHighScore();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            ActivateShieldPowerup(10);
    }

    public void ClearHighScore()
    {
        highScore = 0;
        highScoreText.text = highScore.ToString("n0");
    }

    public void SavePlayerPrefs()
    {
        PlayerPrefs.SetFloat("Master Volume", masterVolumeValue);
        PlayerPrefs.SetFloat("Music Volume", musicVolumeValue);
        PlayerPrefs.SetFloat("SFX Volume", sfxVolumeValue);
        PlayerPrefs.SetInt("Difficulty", optionsMenu.GetDifficulty());
        PlayerPrefs.Save();

        audioManager.AdjustAudioFromPrefs();
    }

    public void LoadPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("Master Volume")
            && PlayerPrefs.HasKey("Music Volume")
            && PlayerPrefs.HasKey("SFX Volume")
            && PlayerPrefs.HasKey("Difficulty"))
        {
            masterVolumeValue = PlayerPrefs.GetFloat("Master Volume");
            musicVolumeValue = PlayerPrefs.GetFloat("Music Volume");
            sfxVolumeValue = PlayerPrefs.GetFloat("SFX Volume");
            difficulty = PlayerPrefs.GetInt("Difficulty");
        }
        else
            RestoreDefaultPrefs();
    }

    public void RestoreDefaultPrefs()
    {
        PlayerPrefs.SetFloat("Master Volume", 0.5f);    // 50%
        PlayerPrefs.SetFloat("Music Volume", 0.5f);     // 50%
        PlayerPrefs.SetFloat("SFX Volume", 0.5f);       // 50%
        PlayerPrefs.SetInt("Difficulty", 1);            //Normal
        
    }

    public void ProcessPauseMenu()
    {
        // TODO: Break this out into it's own script for pause menu
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
        // TODO Break this out into it's own script
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
        PlayerPrefs.Save();
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

    void ActivateShieldPowerup(float duration)
    {
        if (!shieldsActive)
            StartCoroutine(ShieldPowerup(duration));
        else if (shieldsActive)
            shieldTimeLeft += duration;
    }

    public IEnumerator ShieldPowerup(float duration)
    {
        foreach (var shield in powerupShields)
        {
            shield.SetActive(true);
            shieldsActive = true;
        }

        for(shieldTimeLeft = duration; shieldTimeLeft > 0; shieldTimeLeft -= Time.deltaTime)
            yield return null;

        foreach (var shield in powerupShields)
        {
            shield.SetActive(false);
            shieldsActive = false;
        }
    }
}
