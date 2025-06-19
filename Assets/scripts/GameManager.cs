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
    
    [Header("UI References")]
    public UIManager uiManager;
    public ProgressBarWithStarsSlider progressBarWithStars;
    
    [Header("Audio")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioClip perfectSound;
    public AudioClip goodSound;
    public AudioClip missSound;
    public AudioClip loseClip;
    public AudioClip winClip;
    public AudioClip clickTileClip;
    public AudioClip slideTileHoldClip;
    public AudioClip combosfx;
    
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
        // Phát lại nhạc nền nếu có
        if (MusicManager.Instance != null && MusicManager.Instance.defaultMusic != null)
        {
            MusicManager.Instance.PlayMusic(MusicManager.Instance.defaultMusic);
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
    }

    
    public void GameOver()
    {
        if (isGameOver) return;
        
        isGameOver = true;
        Time.timeScale = 0f;
        
        if (uiManager != null)
        {
            uiManager.ShowGameOver();
        }
        
        PlaySFX(loseClip);
        MusicManager.Instance?.StopMusic(); // Dừng nhạc nền khi game over
    }
    
    public void Win()
    {
        // ... logic win ...
        PlaySFX(winClip);
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
    
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
} 