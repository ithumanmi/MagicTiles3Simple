using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    [Header("Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Clips")]
    public AudioClip defaultMusic;
    public AudioClip winClip;
    public AudioClip loseClip;
    public AudioClip clickTileClip;
    public AudioClip slideTileHoldClip;
    public AudioClip combosfx;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Tự động tìm, tạo và gán AudioSources
            AudioSource[] sources = GetComponents<AudioSource>();

            if (sources.Length == 0)
            {
                // Nếu chưa có, tạo mới cả hai
                musicSource = gameObject.AddComponent<AudioSource>();
                sfxSource = gameObject.AddComponent<AudioSource>();
            }
            else if (sources.Length == 1)
            {
                // Nếu có một, dùng nó cho nhạc và tạo cái mới cho SFX
                musicSource = sources[0];
                sfxSource = gameObject.AddComponent<AudioSource>();
            }
            else // sources.Length >= 2
            {
                // Nếu có 2 hoặc nhiều hơn, gán 2 cái đầu tiên
                musicSource = sources[0];
                sfxSource = sources[1];
            }

            // Cấu hình các thuộc tính cần thiết
            musicSource.loop = true;
            musicSource.playOnAwake = false;
            musicSource.ignoreListenerPause = true;

            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
            sfxSource.ignoreListenerPause = true;

            if (defaultMusic != null)
            {
                musicSource.clip = defaultMusic;
                musicSource.Play();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Khi scene chính (build index 0) được tải, phát nhạc
        if (scene.buildIndex == 0)
        {
            PlayMusic(defaultMusic);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource != null && clip != null)
        {
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }

    public void SetVolume(float volume)
    {
        if (musicSource != null)
            musicSource.volume = Mathf.Clamp01(volume);
    }

    public void ToggleMusic()
    {
        if (musicSource != null)
            musicSource.mute = !musicSource.mute;
    }

    // Tự động thêm vào project nếu chưa có
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void AutoCreate()
    {
        if (FindObjectOfType<MusicManager>() == null)
        {
            GameObject go = new GameObject("MusicManager");
            go.AddComponent<MusicManager>();
        }
    }
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
} 