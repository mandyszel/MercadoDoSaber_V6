using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider volumeSlider;

    void Start()
    {
        // Inicializa com o volume atual da música
        if (BackgroundMusicManager.instance != null)
        {
            volumeSlider.value = BackgroundMusicManager.instance.GetVolume();
        }

        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        if (BackgroundMusicManager.instance != null)
        {
            BackgroundMusicManager.instance.SetVolume(volume);
        }
    }
}
