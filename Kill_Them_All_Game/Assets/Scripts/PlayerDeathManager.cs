using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class PlayerDeathManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ShowAdvCloseDeath();

    [SerializeField] private CanvasGroup _canvasDeath;

    [SerializeField] private GameObject _canvasScore;
    [SerializeField] private GameObject _canvasBar;
    [SerializeField] private GameObject _canvasMobileControls;

    [SerializeField] private float _fadeDuration;

    private string _currentSceneName;

    public static bool IsDeath = false;


    private void Start()
    {
        IsDeath = false;
        _currentSceneName = SceneManager.GetActiveScene().name;
    }

    private IEnumerator FadeInCanvas()
    {
        _canvasDeath.gameObject.SetActive(true);

        float elapsedTime = 0f;

        while (elapsedTime < _fadeDuration)
        {
            _canvasDeath.alpha = Mathf.Lerp(0.9f, 1, elapsedTime / _fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _canvasDeath.alpha = 1;

        IsDeath = true;
    }

    public void OnPlayerDeath()
    {
        if (SceneManagerSingleton.Instance.CurrentSceneIndex == 2)
        {
            _canvasMobileControls.SetActive(false);
        }

        _canvasBar.SetActive(false);
        _canvasScore.SetActive(false);

        GameManager.IsGameStarted = false;

        StartCoroutine(FadeInCanvas());
    }

    public void ShowAdvCloseDeathBtn()
    {
        ShowAdvCloseDeath();
    }

    public void CloseDeathPanel()
    {
        IsDeath = false;
        SceneManager.LoadScene(_currentSceneName);
    }
}
