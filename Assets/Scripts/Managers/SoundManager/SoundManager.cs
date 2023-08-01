using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public static string VolumeEffects = "VolumeEffects";
    public static string VolumeMusic = "VolumeMusic";

    [SerializeField]
    private float updateMusicSpeed;

    private AudioSource audioSource;

    private float effectsVolume;
    private float musicVolume;

    private bool fadingMusic;

    protected override void LoadData()
    {
        effectsVolume = PlayerPrefs.GetFloat(VolumeEffects, 1f);
        musicVolume = PlayerPrefs.GetFloat(VolumeMusic, 1f);

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        LoadMusicScene();
    }

    public void LoadMusicScene()
    {
        StartCoroutine(LoadMusicSceneCoroutine());
    }

    public IEnumerator LoadMusicSceneCoroutine()
    {
        GameObject[] musicScenes = GameObject.FindGameObjectsWithTag(GlobalConstants.TagMusicScene);
        if (musicScenes.Length > 0)
        {
            GameObject musicScene = musicScenes.Length > 1 ? musicScenes[1] : musicScenes[0];
            AudioClip musicClip = musicScene.GetComponent<MusicScene>().MusicClip;
            if (musicClip)
            {
                if (!audioSource.clip || !musicClip.name.Equals(audioSource.clip.name))
                {
                    if (audioSource.clip)
                    {
                        yield return StartCoroutine(FadeMusicSceneCoroutine(true));
                        audioSource.Stop();
                    }
                   
                    audioSource.clip = musicClip;
                    audioSource.Play();
                    yield return StartCoroutine(FadeMusicSceneCoroutine(false));
                }
            }
            else
            {
                if (audioSource.clip)
                {
                    yield return StartCoroutine(FadeMusicSceneCoroutine(true));
                    audioSource.Stop();
                    audioSource.clip = null;
                }
            }
        }
    }

    public IEnumerator FadeMusicSceneCoroutine(bool fadeDown)
    {
        if (!fadingMusic)
        {
            fadingMusic = true;

            if (fadeDown)
            {
                yield return StartCoroutine(FadeMusic(0, GetMusicSceneFadeSpeed()));
            }
            else
            {
                yield return StartCoroutine(FadeMusic(musicVolume, GetMusicSceneFadeSpeed()));
            }

            fadingMusic = false;
        }
    }

    private float GetMusicSceneFadeSpeed()
    {
        return SceneLoadManager.Instance.FadingSpeed * 2;
    }

    public void UpdateMusicVolume(float percentage, bool down = true, bool fading = true)
    {
        float updateAmount = audioSource.volume * (percentage / 100);
        float targetVolume = down ? audioSource.volume - updateAmount : audioSource.volume + updateAmount;
        targetVolume = Mathf.Clamp(targetVolume, 0, 1);

        if (fading)
        {
            StartCoroutine(FadeMusic(targetVolume, updateMusicSpeed));
        }
        else
        {
            audioSource.volume = targetVolume;
        }
    }

    public void BackToDefaultMusicVolume(bool fading = true)
    {
        if (fading)
        {
            StartCoroutine(FadeMusic(musicVolume, updateMusicSpeed));
        }
        else
        {
            audioSource.volume = musicVolume;
        }
    }

    public IEnumerator FadeMusic(float targetVolume, float fadeSpeed)
    {
        bool volumeDown = targetVolume < audioSource.volume;
        if (volumeDown)
        {
            while (audioSource.volume > targetVolume)
            {
                float newVolume = audioSource.volume - fadeSpeed * Time.deltaTime;
                audioSource.volume = Mathf.Clamp(newVolume, targetVolume, 1);
                yield return null;
            }
        }
        else
        {
            while (audioSource.volume < targetVolume)
            {
                float newVolume = audioSource.volume + fadeSpeed * Time.deltaTime;
                audioSource.volume = audioSource.volume = Mathf.Clamp(newVolume, 0, targetVolume);
                yield return null;
            }
        }
    }

    public void SaveEffectsVolume(float volume)
    {
        effectsVolume = volume;
        PlayerPrefs.SetFloat(VolumeEffects, effectsVolume);
    }

    public void SaveMusicVolume(float volume)
    {
        musicVolume = volume;
        audioSource.volume = musicVolume;
        PlayerPrefs.SetFloat(VolumeMusic, musicVolume);
    }

    public float EffectsVolume { get => effectsVolume; }
    public float MusicVolume { get => musicVolume; }
}
