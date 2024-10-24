using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoSceneManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string mainSceneName = "MainScene"; 
    public string narrationSoundName;

    void Start()
    {
        Debug.Log("VideoSceneManager Start called.");

        if (AudioManager.instance != null)
        {
            Debug.Log("Playing narration and dimming background music in VideoScene.");
            AudioManager.instance.DimBackgroundMusic(0.05f);  // Dim the background music to 5%
            if (!string.IsNullOrEmpty(narrationSoundName) && narrationSoundName != "MuseumAmbiance")
            {
                AudioManager.instance.PlayNarration(narrationSoundName);  // Play the specific narration for this painting
            }
            else
            {
                Debug.LogWarning("narrationSoundName is either not set or set to 'MuseumAmbiance', which should not be used as narration.");
            }
        }
        else
        {
            Debug.LogError("AudioManager.instance not found in the scene.");
        }

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
            videoPlayer.Play();  // Start playing the video
        }
        else
        {
            Debug.LogError("VideoPlayer not found in the scene.");
        }
    }
    
    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video has ended. Returning to Main Scene.");

        // Use SceneTransitionManager to transition back
        if (SceneTransitionManager.instance != null)
        {
            SceneTransitionManager.instance.TransitionToScene(mainSceneName, false);
        }
        else
        {
            Debug.LogError("SceneTransitionManager.instance not found in the scene.");

            SceneManager.LoadScene(mainSceneName);
        }
    }
}