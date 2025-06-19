using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public GameObject gameOverPanel;
    public Button restartButton;
    public Button pauseButton;
    public TextMeshProUGUI gameOverText;

    [SerializeField] private TextScalingEffect[] _textScalingEffect;
    [Header("Score Display")]
    public Text perfectText;
    public Text goodText;
    public Text missText;
    
    private int perfectCount = 0;
    private int goodCount = 0;
    private int missCount = 0;

    private TextScalingEffect _lastTextScalingEffect;

    [Header("Combo Particle Effects")]
    [SerializeField] private ParticleSystem[] comboParticles;
    private ParticleSystem _lastPlayedParticle;

    [Header("Star Count Display")]
    public TextMeshProUGUI starCountText;
    public Transform starImage;
    public float starShowDuration = 1f;

    void Start()
    {
        Debug.Log("UIManager: Bắt đầu khởi tạo");
        SetupUI();
        // Ẩn combo text khi bắt đầu
        if (comboText != null)
        {
            comboText.transform.localScale = Vector3.zero;
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
            // Hiệu ứng nhúng nhảy khi cộng điểm
            scoreText.transform.DOKill();
            scoreText.transform.localScale = Vector3.one * 0.7f;
            scoreText.transform.DOScale(1.2f, 0.15f)
                .SetEase(Ease.OutBack)
                .OnComplete(() => {
                    scoreText.transform.DOScale(1f, 0.15f).SetEase(Ease.InOutSine);
                });
        }
        else
        {
            Debug.LogWarning("UIManager: scoreText is null!");
        }
    }
    
    public void ShowCombo(int combo)
    {
        // Nếu hiệu ứng cũ đang play, dừng lại trước khi play hiệu ứng mới
        if (_lastTextScalingEffect != null)
        {
            _lastTextScalingEffect.Stop();
        }
        var index = Random.Range(0, _textScalingEffect.Length - 1);
        _lastTextScalingEffect = _textScalingEffect[index];
        _lastTextScalingEffect.Play();

        // Play random combo particle
        if (comboParticles != null && comboParticles.Length > 0)
        {
            // Stop last played particle if still playing
            if (_lastPlayedParticle != null && _lastPlayedParticle.isPlaying)
            {
                _lastPlayedParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            int randIdx = Random.Range(0, comboParticles.Length);
            _lastPlayedParticle = comboParticles[randIdx];
            // Đặt vị trí particle trùng với vị trí của _lastTextScalingEffect
            if (_lastTextScalingEffect != null)
            {
                _lastPlayedParticle.transform.position = _lastTextScalingEffect.transform.position;
            }
            _lastPlayedParticle.gameObject.SetActive(true);
            _lastPlayedParticle.Play();
        }

        if (comboText != null)
        {
            comboText.gameObject.SetActive(true);
            comboText.text = "x " + combo.ToString();

            // Hủy tween cũ nếu có
            comboText.transform.DOKill();
            comboText.transform.localScale = Vector3.one * 0.7f;

            // Scale pop lên 1.2 rồi về 1
            comboText.transform.DOScale(1.5f, 0.15f)
                .SetEase(Ease.OutBack)
                .OnComplete(() => {
                    comboText.transform.DOScale(1f, 0.15f).SetEase(Ease.InOutSine);
                });

            CancelInvoke("HideComboText");
            Invoke("HideComboText", 1f);
        }
        else
        {
            Debug.LogWarning("UIManager: comboText is null!");
        }
    }
    
    void HideComboText()
    {
        if (comboText != null)
        {
            comboText.transform.DOKill();
            comboText.transform.DOScale(0, 0.15f).SetEase(Ease.InBack);
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
            if (gameOverText != null)
            {
                gameOverText.text = "Game Over!";
                gameOverText.gameObject.SetActive(true);
            }
            if (restartButton != null)
            {
                restartButton.onClick.RemoveAllListeners();
                restartButton.onClick.AddListener(() => GameManager.Instance.Restart());
                restartButton.gameObject.SetActive(true);
            }
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

    public void ShowStarCountAtStarImage(int starCount)
    {
        if (starCountText != null && starImage != null)
        {
            starCountText.text = "x " + starCount.ToString();
            starCountText.gameObject.SetActive(true);

            // Đặt vị trí starCountText trùng với starImage
            starCountText.rectTransform.position = starImage.position;

            // Hiệu ứng scale pop
            starCountText.transform.DOKill();
            starCountText.transform.localScale = Vector3.one * 0.7f;
            starCountText.transform.DOScale(1.2f, 0.15f)
                .SetEase(Ease.OutBack)
                .OnComplete(() => {
                    starCountText.transform.DOScale(1f, 0.15f).SetEase(Ease.InOutSine);
                });

            CancelInvoke("HideStarShow");
            Invoke("HideStarShow", starShowDuration);
        }
    }

    private void HideStarShow()
    {
        if (starCountText != null)
        {
            starCountText.gameObject.SetActive(false);
            starCountText.transform.localScale = Vector3.one;
        }
    }
} 