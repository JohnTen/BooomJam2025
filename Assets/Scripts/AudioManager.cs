using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource;

    [Header("Audio Clips")]
    public AudioClip normalBgm;
    public AudioClip bossIntro;
    public AudioClip bossLoop;
    public AudioClip flyIntro;
    public AudioClip flyLoop;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)

        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        musicSource.clip = normalBgm;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        var sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.PlayOneShot(clip);
        Destroy(sfxSource, clip.length);
    }

    public void ChangeBGMToBoss()
    {
        var newSource = musicSource.gameObject.AddComponent<AudioSource>();

        newSource.PlayOneShot(bossIntro);   
        newSource.clip = bossLoop;
        newSource.PlayScheduled(AudioSettings.dspTime + bossIntro.length);
        newSource.volume = 0;
        newSource.DOFade(musicSource.volume, 1f);

        musicSource.DOFade(0, 1f).OnComplete(() => {
            Destroy(musicSource);
            musicSource = newSource;
        });
    }

    public void ChangeBGMToFly()
    {
        var newSource = musicSource.gameObject.AddComponent<AudioSource>();

        newSource.PlayOneShot(flyIntro);   
        newSource.clip = flyLoop;
        newSource.PlayScheduled(AudioSettings.dspTime + flyIntro.length);
        newSource.volume = 0;
        newSource.DOFade(musicSource.volume, 1f);

        musicSource.DOFade(0, 1f).OnComplete(() => {
            Destroy(musicSource);
            musicSource = newSource;
        });
    }
}
