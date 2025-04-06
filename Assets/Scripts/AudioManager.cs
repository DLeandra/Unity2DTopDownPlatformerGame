using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private AudioSource audioSource; // For sound effects
    private AudioSource musicSource; // For background music

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip crystalSound;
    public AudioClip spikeSound;
    public AudioClip portalSound; 


    [Header("Volume Controls (Editable in Inspector)")]
    [Range(0f, 1f)] public float musicVolume = 0.5f; // Default music volume
    [Range(0f, 1f)] public float sfxVolume = 1f;     // Default SFX volume

    private void Awake()
    {
        // Singleton setup
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

    private void Start()
    {
        // Add two Audio Sources
        audioSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();

        // Configure the music source
        musicSource.loop = true;
        PlayMusic(backgroundMusic);

        // Set initial volumes
        musicSource.volume = musicVolume;
        audioSource.volume = sfxVolume;
    }

    // Play sound effects
    public void PlaySound(AudioClip clip)
    {
        audioSource.volume = sfxVolume; // Use the SFX volume setting
        audioSource.PlayOneShot(clip);
    }

    // Play background music
    public void PlayMusic(AudioClip musicClip)
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }

        musicSource.clip = musicClip;
        musicSource.volume = musicVolume; // Use the music volume setting
        musicSource.Play();
    }

    private void OnValidate()
    {
        // Update audio source volumes when changes are made in the Inspector
        if (musicSource != null) musicSource.volume = musicVolume;
        if (audioSource != null) audioSource.volume = sfxVolume;
    }
}
