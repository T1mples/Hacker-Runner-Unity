using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class VictoryManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ShowAdvEndGame();

    [SerializeField] private WaveManager _waveManagerScript;

    [SerializeField] private CanvasGroup _canvasWin;

    [SerializeField] private GameObject _canvasScore;
    [SerializeField] private GameObject _canvasBar;
    [SerializeField] private GameObject _canvasMobileControls;

    [SerializeField] private AudioSource _audioWin;

    [SerializeField] private float _duration;

    private string _currentSceneName;


    private void Start()
    {
        _currentSceneName = SceneManager.GetActiveScene().name;
    }

    private IEnumerator FadeInCanvas()
    {
        PlayerDeathManager.IsDeath = true;

        _canvasWin.gameObject.SetActive(true);
        _canvasWin.alpha = 0f;

        float elapsedTime = 0f;

        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;
            _canvasWin.alpha = Mathf.Clamp01(elapsedTime / _duration);
            yield return null;
        }

        _canvasWin.alpha = 1f;
    }

    public void OpenVictory()
    {
        if (SceneManagerSingleton.Instance.CurrentSceneIndex == 2)
        {
            _canvasMobileControls.SetActive(false);
        }

        _canvasScore.SetActive(false);
        _canvasBar.SetActive(false);

        _audioWin.Play();

        StartCoroutine(FadeInCanvas());
    }

    public void ShowAdvEndGameBtn()
    {
        _audioWin.Stop();
        ShowAdvEndGame();
    }

    public void ContinueAfterCloseVictory()
    {
        SceneManager.LoadScene(_currentSceneName);
        _waveManagerScript.StartNextWave();
    }
}
