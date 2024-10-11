// SceneTransitionManager.cs

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void TransitionToScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    IEnumerator FadeIn()
    {
        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;
        for (float t = fadeDuration; t >= 0; t -= Time.deltaTime)
        {
            color.a = t / fadeDuration;
            fadeImage.color = color;
            yield return null;
        }
        fadeImage.gameObject.SetActive(false);
    }

    IEnumerator FadeOut(string sceneName)
    {
        // Fade out audio
        StartCoroutine(FadeOutAudio());

        // Fade out screen
        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;
        for (float t = 0; t <= fadeDuration; t += Time.deltaTime)
        {
            color.a = t / fadeDuration;
            fadeImage.color = color;
            yield return null;
        }
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FadeOutAudio()
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            AudioSource bgMusic = audioManager.GetAudioSource("Collaborative Ambitions");
            if (bgMusic != null)
            {
                float startVolume = bgMusic.volume;

                while (bgMusic.volume > 0)
                {
                    bgMusic.volume -= startVolume * Time.deltaTime / fadeDuration;
                    yield return null;
                }

                bgMusic.Stop();
                bgMusic.volume = startVolume;
            }
        }
    }
}
