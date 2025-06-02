using UnityEngine;
using UnityEngine.UI;

public class SFXVolumeController : MonoBehaviour
{
    public Slider sfxSlider;

    void Start()
    {
        if (SoundEffectsManager.instance != null)
        {
            sfxSlider.value = SoundEffectsManager.instance.GetVolume();
        }

        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetSFXVolume(float volume)
    {
        if (SoundEffectsManager.instance != null)
        {
            SoundEffectsManager.instance.SetVolume(volume);
        }
    }
}
