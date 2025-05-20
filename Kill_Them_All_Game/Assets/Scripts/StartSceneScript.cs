using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneScript : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void OpenSceneByDeviceType();

    [SerializeField] private CanvasGroup _canvasGroup;

    [SerializeField] private float _fadeDuration;
    [SerializeField] private float _displayDuration;


    private void Start()
    {
        StartCoroutine(FadeCanvasGroupAndChangeScene());
    }

    private IEnumerator FadeCanvasGroupAndChangeScene()
    {
        _canvasGroup.gameObject.SetActive(true);

        yield return StartCoroutine(FadeCanvasGroup(_canvasGroup, 0f, 1f, _fadeDuration));

        yield return new WaitForSeconds(_displayDuration);

        yield return StartCoroutine(FadeCanvasGroup(_canvasGroup, 1f, 0f, _fadeDuration));

        _canvasGroup.gameObject.SetActive(false);

        OpenSceneByDeviceType();
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }

    public void OpenScene1()
    {
        SceneManager.LoadScene(1);
    }
    
    public void OpenScene2()
    {
        SceneManager.LoadScene(2);
    }
}
