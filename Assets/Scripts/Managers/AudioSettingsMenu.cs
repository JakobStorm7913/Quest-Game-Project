using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (SoundFXManager.Instance != null)
        {
            masterSlider.value = SoundFXManager.Instance.masterVolume;
            musicSlider.value = SoundFXManager.Instance.musicVolume;
            sfxSlider.value = SoundFXManager.Instance.sfxVolume;
        }
    }

    public void OnMasterSliderChanged(float value)
    {
        if (SoundFXManager.Instance != null)
            SoundFXManager.Instance.SetMasterVolume(value);
    }

    public void OnMusicSliderChanged(float value)
    {
        if (SoundFXManager.Instance != null)
            SoundFXManager.Instance.SetMusicVolume(value);
    }

    public void OnSfxSliderChanged(float value)
    {
        if (SoundFXManager.Instance != null)
            SoundFXManager.Instance.SetSfxVolume(value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
