using System.Collections;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasSettings;

    [SerializeField] private GameObject _canvasScore;
    [SerializeField] private GameObject _canvasMain;


    private IEnumerator FadeInCanvasGroup(CanvasGroup canvasGroup, float duration)
    {
        float elapsedTime = 0f;
        canvasGroup.alpha = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    public void OpenSettings()
    {
        _canvasMain.SetActive(false);
        _canvasScore.SetActive(false);
        _canvasSettings.gameObject.SetActive(true);
        StartCoroutine(FadeInCanvasGroup(_canvasSettings, 1.5f));
    }

    public void CloseSettings()
    {
        _canvasSettings.gameObject.SetActive(false);
        _canvasScore.SetActive(true);
        _canvasMain.SetActive(true);
    }
}
