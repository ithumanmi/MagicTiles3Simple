using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public CanvasGroup settingsCanvasGroup;
    public RectTransform settingsRect;
    public Button toggleMusicButton;
    public Button toggleSFXButton;
    public Button restartButton;
    public Text musicStatusText;
    public Text sfxStatusText;

    public AudioSource musicSource; // Nhạc nền
    public AudioSource[] sfxSources; // Các AudioSource cho hiệu ứng

    private bool musicOn = true;
    private bool sfxOn = true;

    void Start()
    {
        toggleMusicButton.onClick.AddListener(ToggleMusic);
        toggleSFXButton.onClick.AddListener(ToggleSFX);
        restartButton.onClick.AddListener(RestartGame);
        UpdateUI();
    }

    public void ToggleMusic()
    {
        musicOn = !musicOn;
        if (musicSource != null)
            musicSource.mute = !musicOn;
        UpdateUI();
    }

    public void ToggleSFX()
    {
        sfxOn = !sfxOn;
        foreach (var sfx in sfxSources)
        {
            if (sfx != null)
                sfx.mute = !sfxOn;
        }
        UpdateUI();
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    void UpdateUI()
    {
        if (musicStatusText != null)
            musicStatusText.text = musicOn ? "Nhạc: Bật" : "Nhạc: Tắt";
        if (sfxStatusText != null)
            sfxStatusText.text = sfxOn ? "Âm thanh: Bật" : "Âm thanh: Tắt";
    }

    public void ToggleSettingsPanel()
    {
        if (settingsPanel != null && settingsCanvasGroup != null && settingsRect != null)
        {
            bool willShow = !settingsPanel.activeSelf;

            if (willShow)
            {
                settingsPanel.SetActive(true);
                settingsCanvasGroup.alpha = 0;
                settingsRect.localScale = Vector3.one * 0.7f;
                settingsCanvasGroup.DOFade(1, 0.25f);
                settingsRect.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
                Time.timeScale = 0f;
            }
            else
            {
                settingsCanvasGroup.DOFade(0, 0.2f);
                settingsRect.DOScale(0.7f, 0.2f).SetEase(Ease.InBack)
                    .OnComplete(() => {
                        settingsPanel.SetActive(false);
                        Time.timeScale = 1f;
                    });
            }
        }
    }
} 