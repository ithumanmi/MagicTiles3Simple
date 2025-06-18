using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Text scoreText;
    public Text comboText;
    public GameObject gameOverPanel;
    public Button restartButton;
    public Button pauseButton;
    
    [Header("Score Display")]
    public Text perfectText;
    public Text goodText;
    public Text missText;
    
    private int perfectCount = 0;
    private int goodCount = 0;
    private int missCount = 0;
    
    void Start()
    {
        Debug.Log("UIManager: Bắt đầu khởi tạo");
        SetupUI();
        // Ẩn combo text khi bắt đầu
        if (comboText != null)
        {
            comboText.gameObject.SetActive(false);
        }
    }
    
    void SetupUI()
    {
        // Kiểm tra các UI elements
        Debug.Log($"UIManager: scoreText = {(scoreText != null ? "OK" : "NULL")}");
        Debug.Log($"UIManager: comboText = {(comboText != null ? "OK" : "NULL")}");
        Debug.Log($"UIManager: perfectText = {(perfectText != null ? "OK" : "NULL")}");
        Debug.Log($"UIManager: goodText = {(goodText != null ? "OK" : "NULL")}");
        Debug.Log($"UIManager: missText = {(missText != null ? "OK" : "NULL")}");
        
        // Setup restart button
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(() => GameManager.Instance.Restart());
        }
        
        // Setup pause button
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(PauseGame);
        }
        
        // Hide game over panel initially
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        
        UpdateScoreDisplay();
    }
    
    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
        else
        {
            Debug.LogWarning("UIManager: scoreText is null!");
        }
    }
    
    public void ShowCombo(int combo)
    {
        if (comboText != null)
        {
            // Hiển thị và cập nhật combo text
            comboText.gameObject.SetActive(true);
            comboText.text = "x " + combo.ToString();
            
            // Hủy coroutine cũ nếu có
            CancelInvoke("HideComboText");
            // Đặt lịch ẩn combo text sau 0.3s
            Invoke("HideComboText", 0.8f);
            
        }
        else
        {
            Debug.LogWarning("UIManager: comboText is null!");
        }
    }
    
    private void HideComboText()
    {
        if (comboText != null)
        {
            comboText.gameObject.SetActive(false);
        }
    }
    
    public void AddPerfect()
    {
        perfectCount++;
        UpdateScoreDisplay();
        Debug.Log($"UIManager: Perfect count = {perfectCount}");
    }
    
    public void AddGood()
    {
        goodCount++;
        UpdateScoreDisplay();
        Debug.Log($"UIManager: Good count = {goodCount}");
    }
    
    public void AddMiss()
    {
        missCount++;
        UpdateScoreDisplay();
        Debug.Log($"UIManager: Miss count = {missCount}");
    }
    
    void UpdateScoreDisplay()
    {
        if (perfectText != null)
            perfectText.text = "Perfect: " + perfectCount;
        if (goodText != null)
            goodText.text = "Good: " + goodCount;
        if (missText != null)
            missText.text = "Miss: " + missCount;
    }
    
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
    
    void PauseGame()
    {
        Time.timeScale = Time.timeScale == 0f ? 1f : 0f;
    }
    
    public void ResetStats()
    {
        perfectCount = 0;
        goodCount = 0;
        missCount = 0;
        UpdateScoreDisplay();
        // Ẩn combo text khi reset
        if (comboText != null)
        {
            comboText.gameObject.SetActive(false);
        }
        Debug.Log("UIManager: Reset stats");
    }
} 