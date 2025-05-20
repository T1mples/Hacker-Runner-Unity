using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource _audioBG;

    [SerializeField] private AudioSource _audioClick;
    [SerializeField] private AudioSource _audioSwordHit;
    [SerializeField] private AudioSource _audioCut;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void PauseAudioBG()
    {
        _audioBG.Pause();
    }

    public void ResumeAudioBG()
    {
        _audioBG.UnPause();
    }
    
    public void StopAudioBG()
    {
        _audioBG.Stop();
    }
    
    public void PlayAudioSwordHit()
    {
        _audioSwordHit.Play();
    }
    
    public void PlayAudioCut()
    {
        _audioCut.Play();
    }
    
    public void PlayAudioClick()
    {
        _audioClick.Play();
    }
}
