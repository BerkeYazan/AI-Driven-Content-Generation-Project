// MainSceneManager.cs

using UnityEngine;
using System.Collections.Generic;

public class MainSceneManager : MonoBehaviour
{
    public SubtitleManager subtitleManager;
    public List<SubtitleEntry> subtitles;

    public string narrationSoundName;

    private AudioSource narrationAudioSource;

    void Start()
    {
        // Play narration audio
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        audioManager.Play(narrationSoundName);
        narrationAudioSource = audioManager.GetAudioSource(narrationSoundName);

        // Start subtitles
        subtitleManager.StartSubtitles(subtitles);
    }

    void Update()
    {
        if (narrationAudioSource != null && narrationAudioSource.isPlaying)
        {
            float currentTime = narrationAudioSource.time;
            subtitleManager.UpdateSubtitles(currentTime);
        }
    }
}
