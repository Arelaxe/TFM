using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SoundManager : Singleton<SoundManager>
{
    public static string VolumeEffects = "VolumeEffects";
    public static string VolumeMusic = "VolumeMusic";

    [SerializeField]
    private AudioSource effectsAudioSource;
    [SerializeField]
    private AudioSource musicAudioSource;
    [SerializeField]
    private float updateMusicSpeed;

    [Space]
    [SerializeField]
    private AudioClip selectedButton;
    [SerializeField]
    private AudioClip clickedButton;
    [SerializeField]
    private AudioClip pickupItem;
    [SerializeField]
    private AudioClip pickupDocument;

    private float effectsVolume;
    private float musicVolume;

    private bool fadingMusic;

    protected override void LoadData()
    {
        effectsAudioSource.ignoreListenerPause = true;
        effectsVolume = PlayerPrefs.GetFloat(VolumeEffects, 0.5f);
        effectsAudioSource.volume = effectsVolume;

        musicVolume = PlayerPrefs.GetFloat(VolumeMusic, 0.5f);
    }

    private void Start()
    {
        LoadMusicScene();
    }

    public void LoadMusicScene()
    {
        StartCoroutine(LoadMusicSceneCoroutine());
    }

    public void PlayAudioSource(AudioSource audioSource, float volumePercentange = 100)
    {
        audioSource.volume = effectsVolume * (volumePercentange / 100);
        audioSource.Play();
    }

    public void PlayEffectOneShot(AudioClip clip)
    {
        effectsAudioSource.PlayOneShot(clip);
    }

    public void PlayEffect(AudioClip clip)
    {
        effectsAudioSource.clip = clip;
        effectsAudioSource.Play();
    }

    public void PlaySelectedButton()
    {
        PlayEffect(selectedButton);
    }

    public void PlayClickedButton()
    {
        PlayEffect(clickedButton);
    }

    public void PlayPickupItem()
    {
        PlayEffect(pickupItem);
    }

    public void PlayPickupDocument()
    {
        PlayEffect(pickupDocument);
    }

    public void StopSound()
    {
        effectsAudioSource.Stop();
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
                if (!musicAudioSource.clip || !musicClip.name.Equals(musicAudioSource.clip.name))
                {
                    if (musicAudioSource.clip)
                    {
                        yield return StartCoroutine(FadeMusicSceneCoroutine(true));
                        musicAudioSource.Stop();
                    }
                   
                    musicAudioSource.clip = musicClip;
                    musicAudioSource.Play();
                    yield return StartCoroutine(FadeMusicSceneCoroutine(false));
                }
            }
            else
            {
                if (musicAudioSource.clip)
                {
                    yield return StartCoroutine(FadeMusicSceneCoroutine(true));
                    musicAudioSource.Stop();
                    musicAudioSource.clip = null;
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
        float updateAmount = musicAudioSource.volume * (percentage / 100);
        float targetVolume = down ? musicAudioSource.volume - updateAmount : musicAudioSource.volume + updateAmount;
        targetVolume = Mathf.Clamp(targetVolume, 0, 1);

        if (fading)
        {
            StartCoroutine(FadeMusic(targetVolume, updateMusicSpeed));
        }
        else
        {
            musicAudioSource.volume = targetVolume;
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
            musicAudioSource.volume = musicVolume;
        }
    }

    public IEnumerator FadeMusic(float targetVolume, float fadeSpeed)
    {
        bool volumeDown = targetVolume < musicAudioSource.volume;
        if (volumeDown)
        {
            while (musicAudioSource.volume > targetVolume)
            {
                float newVolume = musicAudioSource.volume - fadeSpeed * Time.deltaTime;
                musicAudioSource.volume = Mathf.Clamp(newVolume, targetVolume, 1);
                yield return null;
            }
        }
        else
        {
            while (musicAudioSource.volume < targetVolume)
            {
                float newVolume = musicAudioSource.volume + fadeSpeed * Time.deltaTime;
                musicAudioSource.volume = musicAudioSource.volume = Mathf.Clamp(newVolume, 0, targetVolume);
                yield return null;
            }
        }
    }

    public void SaveEffectsVolume(float volume)
    {
        effectsVolume = volume;
        effectsAudioSource.volume = effectsVolume;
        PlayerPrefs.SetFloat(VolumeEffects, effectsVolume);
    }

    public void SaveMusicVolume(float volume)
    {
        musicVolume = volume;
        musicAudioSource.volume = musicVolume;
        PlayerPrefs.SetFloat(VolumeMusic, musicVolume);
    }

    public void AddSounds(Button button)
    {
        EventTrigger eventTrigger = button.gameObject.GetComponent<EventTrigger>();
        if (!eventTrigger)
        {
            eventTrigger = button.gameObject.AddComponent<EventTrigger>();
        }
        EventTrigger.Entry entry = new();
        entry.eventID = EventTriggerType.Select;
        entry.callback.AddListener((data) => { PlaySelectedButton(); });
        eventTrigger.triggers.Add(entry);

        button.onClick.AddListener(delegate { PlayClickedButton(); });
    }

    public float EffectsVolume { get => effectsVolume; }
    public float MusicVolume { get => musicVolume; }
}
