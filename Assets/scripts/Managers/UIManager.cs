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
    public GameObject winPanel;
    public Button restartButton;

    [SerializeField] private TextScalingEffect[] _textScalingEffect;

    private TextScalingEffect _lastTextScalingEffect;

    [Header("Combo Particle Effects")]
    [SerializeField] private ParticleSystem[] comboParticles;
    private ParticleSystem _lastPlayedParticle;


    void Start()
    {
        SetupUI();
        // Ẩn combo text khi bắt đầu
        if (comboText != null)
        {
            comboText.transform.localScale = Vector3.zero;
        }
    }
    
    void SetupUI()
    {
        
        // Setup restart button
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(() => GameManager.Instance.Restart());
        }
       
        
        // Hide game over panel initially
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        
        // Hide win panel initially
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
    }
    
    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();

            // Tạo gradient ngẫu nhiên
            TMP_ColorGradient randomGradient = new TMP_ColorGradient();
            randomGradient.topLeft = Random.ColorHSV();
            randomGradient.topRight = Random.ColorHSV();
            randomGradient.bottomLeft = Random.ColorHSV();
            randomGradient.bottomRight = Random.ColorHSV();

            scoreText.colorGradientPreset = randomGradient;
            scoreText.enableVertexGradient = true;

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
        MusicManager.Instance.PlaySFX(MusicManager.Instance.combosfx); 
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
  
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
          
            if (restartButton != null)
            {
                restartButton.onClick.RemoveAllListeners();
                restartButton.onClick.AddListener(() => GameManager.Instance.Restart());
                restartButton.gameObject.SetActive(true);
            }
        }
    }

    public void ShowWin()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            // Bạn có thể thêm các logic khác cho màn hình win ở đây
            // ví dụ: gán lại listener cho nút restart nếu có
        }
    }
    
    public void ResetStats()
    {
      
        // Ẩn combo text khi reset
        if (comboText != null)
        {
            comboText.gameObject.SetActive(false);
        }   
    }
} 