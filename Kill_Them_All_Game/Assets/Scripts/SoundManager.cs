using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Button _soundOnButton;
    [SerializeField] private Button _soundOffButton;

    [SerializeField] private List<AudioSource> _audioSources = new List<AudioSource>();

    public static bool IsSoundOff = false;

    private const string SoundPrefKey = "SoundEnabled";

    private void Start()
    {
        bool isSoundEnabled = PlayerPrefs.GetInt(SoundPrefKey, 1) == 1;
        SetSoundState(isSoundEnabled);
    }

    private void SetSoundState(bool isSoundEnabled)
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.mute = !isSoundEnabled;
        }

        _soundOnButton.gameObject.SetActive(isSoundEnabled);
        _soundOffButton.gameObject.SetActive(!isSoundEnabled);

        IsSoundOff = isSoundEnabled;

        PlayerPrefs.SetInt(SoundPrefKey, isSoundEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void TurnOnSound()
    {
        SetSoundState(true);
    }

    public void TurnOffSound()
    {
        SetSoundState(false);
    }
}
