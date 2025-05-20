using TMPro;
using UnityEngine;
using System.Collections;

public class WarningsShop : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasWarningShop;
    [SerializeField] private TextMeshProUGUI _warningText;


    private IEnumerator FadeWarning(string message)
    {
        _warningText.text = message;
        _canvasWarningShop.gameObject.SetActive(true);

        float duration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            _canvasWarningShop.alpha = Mathf.Clamp01(elapsedTime / duration);
            yield return null;
        }

        _canvasWarningShop.alpha = 1f;

        yield return new WaitForSeconds(0.5f);

        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            _canvasWarningShop.alpha = Mathf.Clamp01(1f - (elapsedTime / duration));
            yield return null;
        }

        _canvasWarningShop.alpha = 0f;
        _canvasWarningShop.gameObject.SetActive(false);
    }

    public void StartFadeWarningCoroutine(string message)
    {
        StartCoroutine(FadeWarning(message));
    }
}
