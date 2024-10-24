using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CanvasGroup))]
public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager instance;

    [Header("Transition Settings")]
    [Tooltip("CanvasGroup used for fade transitions.")]
    public CanvasGroup fadeCanvasGroup;

    [Tooltip("Duration of the fade when entering a painting scene (in seconds).")]
    public float fadeOutDuration = 1f;  // Quicker fade-out

    [Tooltip("Duration of the fade when exiting a painting scene (in seconds).")]
    public float fadeInDuration = 3f;   // Slower fade-in

    [Tooltip("Target volume for background music when dimmed.")]
    [Range(0f, 1f)]
    public float dimmedVolume = 0.1f;   // Dim to 10%

    [Tooltip("Target volume for background music when restored.")]
    [Range(0f, 1f)]
    public float restoredVolume = 1.0f; // Restore to 100%

    void Awake()
    {
        // Implement Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        Debug.Log("SceneTransitionManager Start called.");
        StartCoroutine(FadeIn());  // Fade into the scene
    }

    public void TransitionToScene(string sceneName, bool isPainting)
    {
        Debug.Log($"Transitioning to scene: {sceneName} | isPainting: {isPainting}");
        StartCoroutine(FadeOut(sceneName, isPainting));
    }

    IEnumerator FadeIn()
    {
        if (fadeCanvasGroup == null)
        {
            Debug.LogError("FadeCanvasGroup is not assigned in the SceneTransitionManager.");
            yield break;
        }

        fadeCanvasGroup.gameObject.SetActive(true);
        fadeCanvasGroup.alpha = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeOutDuration)  // Using fadeOutDuration for FadeIn to maintain consistency
        {
            fadeCanvasGroup.alpha = 1f - (elapsedTime / fadeOutDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeCanvasGroup.alpha = 0f;
        fadeCanvasGroup.gameObject.SetActive(false);
    }

    IEnumerator FadeOut(string sceneName, bool isPainting)
    {
        if (fadeCanvasGroup == null)
        {
            Debug.LogError("FadeCanvasGroup is not assigned in the SceneTransitionManager.");
            yield break;
        }

        // Determine fade duration based on transition type
        float currentFadeDuration = isPainting ? fadeOutDuration : fadeInDuration;

        fadeCanvasGroup.gameObject.SetActive(true);
        fadeCanvasGroup.alpha = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < currentFadeDuration)
        {
            fadeCanvasGroup.alpha = elapsedTime / currentFadeDuration;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeCanvasGroup.alpha = 1f;

        // Adjust background music volume after fade out
        if (AudioManager.instance != null)
        {
            if (isPainting)
            {
                Debug.Log("Dimming background music for painting scene.");
                AudioManager.instance.DimBackgroundMusic(dimmedVolume);  // Dim to 10%
            }
            else
            {
                Debug.Log("Restoring background music for museum.");
                AudioManager.instance.RestoreBackgroundMusic(restoredVolume);  // Restore to 100%
            }
        }
        else
        {
            Debug.LogWarning("AudioManager.instance not found. Skipping background music adjustment.");
        }

        // Load the new scene
        SceneManager.LoadScene(sceneName);
    }
}
