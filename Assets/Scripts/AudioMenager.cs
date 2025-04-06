using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Slider musicSlider;
    public Slider sfxSlider;
    public AudioClip backgroundMusic;
    public AudioClip buttonClickSound;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    void Awake()
    {
        // Garante que apenas um AudioManager exista
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Adiciona os AudioSources se não existirem
        AudioSource[] sources = GetComponents<AudioSource>();
        if (sources.Length < 2)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            musicSource = sources[0];
            sfxSource = sources[1];
        }

        // Configuração do AudioSource da música
        musicSource.loop = true;
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            if (!musicSource.isPlaying)
            {
                musicSource.Play();
            }
        }

        // Carrega volumes salvos
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    void Start()
    {
        // Aplica o volume salvo aos sliders, se existirem
        if (musicSlider != null) 
        {
            musicSlider.value = musicSource.volume;
            musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
        }
        
        if (sfxSlider != null) 
        {
            sfxSlider.value = sfxSource.volume;
            sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);
        }

        RegisterButtons();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void UpdateMusicVolume(float value)
    {
        musicSource.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }

    void UpdateSFXVolume(float value)
    {
        sfxSource.volume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();
    }

    void RegisterButtons()
    {
        Button[] buttons = FindObjectsByType<Button>(FindObjectsSortMode.None);
        foreach (Button btn in buttons)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => PlayButtonClickSound());
        }
    }

    public void PlayButtonClickSound()
    {
        if (sfxSource != null && buttonClickSound != null)
        {
            sfxSource.PlayOneShot(buttonClickSound);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RegisterButtons();

        // Garante que os sliders sejam atualizados ao entrar em novas cenas
        if (musicSlider != null) musicSlider.value = musicSource.volume;
        if (sfxSlider != null) sfxSlider.value = sfxSource.volume;
    }
}

