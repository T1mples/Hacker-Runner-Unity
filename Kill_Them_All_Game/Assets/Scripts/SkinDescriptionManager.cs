using UnityEngine;
using System.Collections;

public class SkinDescriptionManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _descriptions;
    [SerializeField] private CanvasGroup _canvasGroupSkinDescriptions;

    private void Start()
    {
        UpdateDescriptionOnShopOpen();
    }

    private void UpdateDescriptionOnShopOpen()
    {
        int selectedSkinIndex = PlayerPrefs.GetInt("SelectedScin", 0);
        UpdateDescription(selectedSkinIndex);
    }

    public void UpdateDescription(int index)
    {
        for (int i = 0; i < _descriptions.Length; i++)
        {
            _descriptions[i].SetActive(i == index);
        }
    }

    public void OnSkinChanged(int index)
    {
        UpdateDescription(index);
    }

    public void StartFadeIn()
    {
        StartCoroutine(FadeInCanvas());
    }

    public void DeactivateCanvas()
    {
        _canvasGroupSkinDescriptions.gameObject.SetActive(false);
    }

    private IEnumerator FadeInCanvas()
    {
        _canvasGroupSkinDescriptions.gameObject.SetActive(true);
        float duration = 1.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            _canvasGroupSkinDescriptions.alpha = Mathf.Clamp01(elapsedTime / duration);
            yield return null;
        }

        _canvasGroupSkinDescriptions.alpha = 1f;
    }
}
