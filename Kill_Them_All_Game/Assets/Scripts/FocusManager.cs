using System.Collections.Generic;
using UnityEngine;

public class FocusManager : MonoBehaviour
{
    public static FocusManager Instance { get; private set; }

    public List<AudioSource> AudioSources = new List<AudioSource>();
    private bool _isPaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Сохраняет объект при загрузке новой сцены
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateAudioSources();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        UpdateAudioSources();

        foreach (AudioSource audioSource in AudioSources)
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Pause();
                _isPaused = true;
            }
        }

        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        if (_isPaused)
        {
            foreach (AudioSource audioSource in AudioSources)
            {
                if (audioSource != null && audioSource.clip != null)
                {
                    audioSource.UnPause();
                }
            }
        }

        Time.timeScale = 1f;
    }

    private void UpdateAudioSources()
    {
        AudioSources.Clear();
        AudioSources.AddRange(FindObjectsByType<AudioSource>(FindObjectsInactive.Include, FindObjectsSortMode.None));
    }

    public void RegisterAudioSource(AudioSource audioSource)
    {
        if (audioSource != null && !AudioSources.Contains(audioSource))
        {
            AudioSources.Add(audioSource);
        }
    }

    public void RemoveAudioSource(AudioSource audioSource)
    {
        if (audioSource != null && AudioSources.Contains(audioSource))
        {
            AudioSources.Remove(audioSource);
        }
    }
}
