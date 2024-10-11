// SubtitleManager.cs

using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class SubtitleManager : MonoBehaviour
{
    public TextMeshProUGUI subtitleText;

    private List<SubtitleEntry> subtitles;
    private int currentSubtitleIndex = 0;
    private bool isPlaying = false;

    void Start()
    {
        subtitleText.text = "";
    }

    public void StartSubtitles(List<SubtitleEntry> subtitlesList)
    {
        subtitles = subtitlesList;
        currentSubtitleIndex = 0;
        isPlaying = true;
    }

    public void UpdateSubtitles(float currentTime)
    {
        if (!isPlaying || subtitles == null || currentSubtitleIndex >= subtitles.Count)
            return;

        SubtitleEntry currentSubtitle = subtitles[currentSubtitleIndex];

        if (currentTime >= currentSubtitle.startTime)
        {
            subtitleText.text = currentSubtitle.text;
            currentSubtitleIndex++;
        }
    }

    public void StopSubtitles()
    {
        isPlaying = false;
        subtitleText.text = "";
    }
}

[System.Serializable]
public class SubtitleEntry
{
    public float startTime;
    public string text;

    public SubtitleEntry(float startTime, string text)
    {
        this.startTime = startTime;
        this.text = text;
    }
}
