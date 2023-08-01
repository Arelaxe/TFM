using UnityEngine;
using UnityEngine.UI;

public class AudioPage : MonoBehaviour
{
    [SerializeField]
    private Slider effectsSlider;

    [SerializeField]
    private AudioClip effectClip;

    [SerializeField]
    private Slider musicSlider;

    [Space]
    [SerializeField]
    private AudioSource audioSource;

    private void OnEnable()
    {
        effectsSlider.value = Mathf.Sqrt(SoundManager.Instance.EffectsVolume) * effectsSlider.maxValue;

        musicSlider.value = Mathf.Sqrt(SoundManager.Instance.MusicVolume) * musicSlider.maxValue;
        audioSource.ignoreListenerPause = true;

        AudioListener.pause = true;
    }

    private void OnDisable()
    {
        AudioListener.pause = false;
    }

    public void UpdateEffectsVolume()
    {
        if (AudioListener.pause)
        {
            audioSource.PlayOneShot(effectClip, GetVolume(effectsSlider));
        }
    }

    public void SelectMusicSlider()
    {
        audioSource.volume = GetSelectedMusicVolume();
        audioSource.Play();
    }

    public void DeselectMusicSlider()
    {
        audioSource.volume = GetSelectedMusicVolume();
        audioSource.Stop();
    }

    public void UpdateMusicVolume()
    {
        audioSource.volume = GetSelectedMusicVolume();
    }

    public void Save()
    {
        SoundManager.Instance.SaveEffectsVolume(GetVolume(effectsSlider));
        SoundManager.Instance.SaveMusicVolume(GetSelectedMusicVolume());
    }

    private float GetSelectedMusicVolume()
    {
        return GetVolume(musicSlider);
    }

    private float GetVolume(Slider slider)
    {
        float selectedMusicVolume = slider.value / slider.maxValue;
        return selectedMusicVolume * selectedMusicVolume;
    }
}
