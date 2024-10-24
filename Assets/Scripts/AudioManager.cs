using UnityEngine;
using System;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    
    [Range(0f, 1f)]
    public float volume = 1f;
    public bool loop;

    [HideInInspector]
    public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Settings")]
    [Tooltip("List of all sounds to be managed.")]
    public Sound[] sounds;

    private float originalBackgroundVolume;
    private Sound bgMusic;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Persist AudioManager across scenes

            // Ensure only one AudioListener exists
            AudioListener[] listeners = FindObjectsOfType<AudioListener>();
            if (listeners.Length > 1)
            {
                Debug.LogWarning("Multiple AudioListeners found. Removing duplicates.");
                for (int i = 1; i < listeners.Length; i++)
                {
                    Destroy(listeners[i].gameObject);
                }
            }
            else if (GetComponent<AudioListener>() == null)
            {
                gameObject.AddComponent<AudioListener>();
            }

            // Initialize all sounds
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.loop = s.loop;
                s.source.playOnAwake = false;
            }

            // Store the original volume of the background music
            bgMusic = Array.Find(sounds, sound => sound.name == "MuseumAmbiance");
            if (bgMusic != null)
            {
                originalBackgroundVolume = bgMusic.volume;
                Debug.Log("Background music 'MuseumAmbiance' volume stored as: " + originalBackgroundVolume);
            }
            else
            {
                Debug.LogError("Background music 'MuseumAmbiance' not found in AudioManager!");
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
        Debug.Log("AudioManager Start called. Playing 'MuseumAmbiance' without restarting.");
        PlayWithoutRestart("MuseumAmbiance");  // Start ambiance music
    }

    public void PlayWithoutRestart(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            if (s.name == "MuseumAmbiance")
            {
                if (!s.source.isPlaying)
                {
                    Debug.Log("Playing background music: " + name);
                    s.source.Play();
                }
                else
                {
                    Debug.LogWarning("Background music: " + name + " is already playing!");
                }
            }
            else
            {
                if (!s.source.isPlaying)
                {
                    Debug.Log("Playing sound: " + name);
                    s.source.Play();
                }
                else
                {
                    Debug.LogWarning("Sound: " + name + " is already playing!");
                }
            }
        }
        else
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
    }

    public void DimBackgroundMusic(float dimFactor)
    {
        if (bgMusic != null)
        {
            float newVolume = originalBackgroundVolume * dimFactor;
            Debug.Log($"Dimming 'MuseumAmbiance' to: {newVolume}");
            bgMusic.source.volume = newVolume;  // Dim volume without stopping
        }
        else
        {
            Debug.LogError("Background music 'MuseumAmbiance' not found!");
        }
    }

    public void RestoreBackgroundMusic(float restoreFactor)
    {
        if (bgMusic != null)
        {
            float restoredVolume = originalBackgroundVolume * restoreFactor;
            Debug.Log($"Restoring 'MuseumAmbiance' volume to: {restoredVolume}");
            bgMusic.source.volume = restoredVolume;  // Restore original volume
        }
        else
        {
            Debug.LogError("Background music 'MuseumAmbiance' not found!");
        }
    }

    public void PlayNarration(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            if (s.name == "MuseumAmbiance")
            {
                Debug.LogWarning("Attempted to play 'MuseumAmbiance' as narration. Ignoring.");
                return;
            }
            if (!s.source.isPlaying)
            {
                Debug.Log("Playing narration sound: " + name);
                s.source.Play();
            }
            else
            {
                Debug.LogWarning("Narration sound: " + name + " is already playing!");
            }
        }
        else
        {
            Debug.LogWarning("Narration sound: " + name + " not found!");
        }
    }
    
    public AudioSource GetAudioSource(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            return s.source;
        }
        else
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return null;
        }
    }
}
