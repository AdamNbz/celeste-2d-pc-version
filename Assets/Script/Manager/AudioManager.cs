using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource playerSFXSource;


    [Header("Audio Clips")]
    [SerializeField] private List<AudioClip> bgmClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> sfxClips = new List<AudioClip>();

    private float bgmVolume = 1f;
    private float sfxVolume = 1f;

    private const string BGM_VOLUME_KEY = "BGMVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudio();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAudio()
    {
        bgmVolume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 1f);
        sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);

        UpdateVolumes();
    }

    #region Volume Control

    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, bgmVolume);
        UpdateVolumes();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, sfxVolume);
        UpdateVolumes();
    }

    private void UpdateVolumes()
    {
        if (bgmSource != null)
            bgmSource.volume = bgmVolume;

        if (sfxSource != null)
            sfxSource.volume = sfxVolume;
        if (playerSFXSource != null)
            playerSFXSource.volume = sfxVolume;
    }

    public float GetBGMVolume() => bgmVolume;
    public float GetSFXVolume() => sfxVolume;

    #endregion

    #region BGM Control

    public void PlayBGM(string clipName, bool loop = true)
    {
        AudioClip clip = bgmClips.Find(c => c.name == clipName);
        if (clip != null)
        {
            bgmSource.clip = clip;
            bgmSource.loop = loop;
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning($"BGM clip '{clipName}' not found!");
        }
    }

    public void PlayBGM(int clipIndex, bool loop = true)
    {
        if (clipIndex >= 0 && clipIndex < bgmClips.Count)
        {
            bgmSource.clip = bgmClips[clipIndex];
            bgmSource.loop = loop;
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning($"BGM index {clipIndex} out of range!");
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PauseBGM()
    {
        bgmSource.Pause();
    }

    public void ResumeBGM()
    {
        bgmSource.UnPause();
    }

    public void FadeBGM(float duration, float targetVolume)
    {
        StartCoroutine(FadeCoroutine(bgmSource, duration, targetVolume));
    }

    #endregion

    #region SFX Control

    public void PlaySFX(string clipName)
    {
        AudioClip clip = sfxClips.Find(c => c.name == clipName);
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"SFX clip '{clipName}' not found!");
        }
    }
    public void PlayPlayerSFX(string clipName)
    {
        AudioClip clip = sfxClips.Find(c => c.name == clipName);
        if (clip != null)
        {
            playerSFXSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"SFX clip '{clipName}' not found!");
        }
    }

    public bool IsPlayerSFXEnd()
    {
        return !playerSFXSource.isPlaying;
    }
    public bool IsSFXEnd()
    {
        return !sfxSource.isPlaying;
    }
    public void PlaySFX(int clipIndex)
    {
        if (clipIndex >= 0 && clipIndex < sfxClips.Count)
        {
            sfxSource.PlayOneShot(sfxClips[clipIndex]);
        }
        else
        {
            Debug.LogWarning($"SFX index {clipIndex} out of range!");
        }
    }

    public void PlaySFXAtPoint(string clipName, Vector3 position)
    {
        AudioClip clip = sfxClips.Find(c => c.name == clipName);
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, position, sfxVolume);
        }
    }

    #endregion

    #region Helper Methods

    private System.Collections.IEnumerator FadeCoroutine(AudioSource source, float duration, float targetVolume)
    {
        float startVolume = source.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            source.volume = Mathf.Lerp(startVolume, targetVolume, t);
            yield return null;
        }

        source.volume = targetVolume;
    }

    public void MuteAll(bool mute)
    {
        bgmSource.mute = mute;
        sfxSource.mute = mute;
    }

    #endregion
}