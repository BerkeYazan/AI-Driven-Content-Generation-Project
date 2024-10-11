// AudioManager.cs

using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sounds;

    void Awake()
    {
        // Singleton pattern to ensure only one AudioManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.loop = s.loop;
            }
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        Play("Collaborative Ambitions");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
            s.source.Play();
        else
            Debug.LogWarning("Sound: " + name + " not found!");
    }

    public void PlayNarration(string name)
    {
        Play(name);
    }

    public AudioSource GetAudioSource(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
            return s.source;
        else
            return null;
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
