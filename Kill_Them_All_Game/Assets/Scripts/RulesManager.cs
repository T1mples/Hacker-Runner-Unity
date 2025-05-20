using System.Collections;
using UnityEngine;

public class RulesManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasRules;

    [SerializeField] private GameObject _canvasSettings;


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

    public void OpenRules()
    {
        _canvasSettings.SetActive(false);
        _canvasRules.gameObject.SetActive(true);
        StartCoroutine(FadeInCanvasGroup(_canvasRules, 1.5f));
    }

    public void CloseRules()
    {
        _canvasRules.gameObject.SetActive(false);
        _canvasSettings.SetActive(true);
    }
}
