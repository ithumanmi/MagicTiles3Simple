using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Game Settings")]
    public int score = 0;
    public float comboResetTime = 10f;
    private float lastHitTime = 0f;
    public int combo = 0;
    //public int maxCombo = 0;


    [Header("Scoring")]
    public int perfectScore = 100;
    public int goodScore = 50;
    public int missScore = 0;
    public float comboMultiplier = 0.1f; // Each combo adds 10% to the base score
    
    [Header("Star Thresholds")]
    public int[] starThresholds; // Mốc điểm cho từng sao
    
    [Header("UI References")]
    public UIManager uiManager;
    public ProgressBarWithStarsSlider progressBarWithStars;

    public float beatInterval = 1f;
    public bool IsEasy;

    private bool isGameOver = false;
    private void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        // Tự động tìm UIManager nếu chưa được gán
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
            Debug.Log("GameManager: Tự động tìm UIManager: " + (uiManager != null ? "Thành công" : "Không tìm thấy"));
        }
        
        ResetGame();
    }
    
    void Update()
    {
        if (combo > 0 && Time.time - lastHitTime > comboResetTime)
        {
            combo = 0;
        
        }
    }
    
    public void ResetGame()
    {
        score = 0;
        combo = 0;
        lastHitTime = Time.time;
        isGameOver = false;
        Time.timeScale = 1f;
        
        if (uiManager != null)
        {
            uiManager.ResetStats();
            uiManager.UpdateScore(score);
        }
        else
        {
            Debug.LogWarning("GameManager: UIManager is null!");
        }
        if (progressBarWithStars != null)
        {
            progressBarWithStars.UpdateProgress(0);
        }
    }
    
    public void AddCombo(int _combo)
    { 
        if (isGameOver) return;
        combo = combo + _combo;
        score += combo;
        lastHitTime = Time.time;
        if (uiManager != null)
        {
            uiManager.UpdateScore(score);
            if(combo > 1)
            {
                uiManager.ShowCombo(combo);
            }
        }

        if (progressBarWithStars != null)
        {
            progressBarWithStars.UpdateProgress(score);
        }

        // Check for win condition
        if (starThresholds != null && starThresholds.Length > 0 && score >= starThresholds[starThresholds.Length - 1])
        {
            Win();
        }
    }

    
    public void GameOver()
    {
        if (isGameOver) return;
        MusicManager.Instance.PlaySFX(MusicManager.Instance.loseClip);
        MusicManager.Instance?.StopMusic(); // Dừng nhạc nền khi game over
        isGameOver = true;
        Time.timeScale = 0f;
        
        if (uiManager != null)
        {
            uiManager.ShowGameOver();
        }
        

    }
    
    public void Win()
    {
        if (isGameOver) return;

        isGameOver = true;
        Time.timeScale = 0f; // Pause the game

        if (uiManager != null)
        {
            uiManager.ShowWin(); // Assuming UIManager has a ShowWin method
        }

        MusicManager.Instance.PlaySFX(MusicManager.Instance.winClip);
        MusicManager.Instance?.StopMusic(); // Dừng nhạc nền khi win
    }
    
    public void Restart()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    
    public void PauseGame()
    {
        Time.timeScale = Time.timeScale == 0f ? 1f : 0f;
    }
    
   
} 