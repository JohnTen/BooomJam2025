using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource;

    [Header("Audio Clips")]
    public AudioClip normalBgm;

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

}
