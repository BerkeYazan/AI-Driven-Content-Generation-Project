using UnityEngine;
using System.Collections.Generic;

public class MainSceneManager : MonoBehaviour
{
    [Header("Subtitle Settings")]
    [Tooltip("Reference to the SubtitleManager component.")]
    public SubtitleManager subtitleManager;

    [Tooltip("List of subtitles to display in the main scene.")]
    public List<SubtitleEntry> subtitles;

    void Start()
    {
        // Initialize subtitles if applicable
        if (subtitleManager != null && subtitles != null && subtitles.Count > 0)
        {
            subtitleManager.StartSubtitles(subtitles);
            Debug.Log("MainSceneManager: Subtitles started.");
        }
        else
        {
            Debug.LogWarning("MainSceneManager: SubtitleManager is not assigned or subtitles list is empty.");
        }

        // Since there is no narration in the museum stuff, no audio playback is required here
    }

    void Update()
    {
        // Update subtitles based on the current time
        if (subtitleManager != null)
        {
            float currentTime = Time.time;
            subtitleManager.UpdateSubtitles(currentTime);
        }
    }
}
