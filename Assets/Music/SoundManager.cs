using UnityEngine;
using System.Collections.Generic;
using TouchControlsKit;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;  // The AudioSource component
    public List<AudioClip> audioClips;  // List of AudioClips to choose from
    private AudioClip currentClip;
    // Static instance of the BackgroundMusic to ensure only one instance exists
    private static SoundManager instance = null;

    void Awake()
    {
        // Check if an instance of this object already exists
        if (instance == null)
        {
            // If not, this is the first instance - keep it alive
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed when loading a new scene
        }
        else
        {
            // If an instance already exists, destroy this duplicate
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }
        void Update()
    {

        if(TCKInput.GetAction( "music", EActionEvent.Click))
        {
            PauseOrResume();
        }
        
        if(TCKInput.GetAction( "music change", EActionEvent.Click))
        {
            PlayRandomSound();
        }
    }

    // Play a specific clip
    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    // Play a random sound from the list
    public void PlayRandomSound()
    {
        if (audioClips.Count > 0)
        {
            int randomIndex = Random.Range(0, audioClips.Count);
            currentClip = audioClips[randomIndex];
            PlaySound(currentClip);
        }
    }

    // Pause the currently playing audio
    public void PauseAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    // Resume playing the paused audio
    public void ResumeAudio()
    {
        if (!audioSource.isPlaying && audioSource.clip != null)
        {
            audioSource.UnPause();
        }
    }
    public void PauseOrResume()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }else if (!audioSource.isPlaying && audioSource.clip != null)
        {
            audioSource.UnPause();
        }

    }

    // Stop the audio
    public void StopAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}