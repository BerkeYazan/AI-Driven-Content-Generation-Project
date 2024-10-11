// VideoSceneManager.cs

using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class VideoSceneManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string mainSceneName = "MainScene";

    public SubtitleManager subtitleManager;
    public List<SubtitleEntry> subtitles;

    public string narrationSoundName;

    void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        videoPlayer.loopPointReached += OnVideoEnd;

        // Play narration audio
        FindObjectOfType<AudioManager>().Play(narrationSoundName);

        // Start subtitles
        subtitleManager.StartSubtitles(subtitles);
    }

    void Update()
    {
        // Sync subtitles with video playback time
        float videoTime = (float)videoPlayer.time;
        subtitleManager.UpdateSubtitles(videoTime);
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(mainSceneName);
    }
}
