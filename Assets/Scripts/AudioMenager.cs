using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    private float musicVolume = 1f;
    private float sfxVolume = 1f;

    void Awake()
    {
        // Garante que só exista um SoundManager persistente
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadVolumeSettings();

            if (!musicSource.isPlaying)
            {
                musicSource.loop = true;
                musicSource.Play();
            }
        }
        else
        {
            Destroy(gameObject); // Evita duplicatas
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = musicVolume;
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        sfxSource.volume = sfxVolume;
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    private void LoadVolumeSettings()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
            musicSource.loop = true;
        }

        if (sfxSource != null)
            sfxSource.volume = sfxVolume;
    }

    public void PlayClickSound()
    {
        if (sfxSource != null && sfxSource.clip != null)
            sfxSource.PlayOneShot(sfxSource.clip);
    }
}
