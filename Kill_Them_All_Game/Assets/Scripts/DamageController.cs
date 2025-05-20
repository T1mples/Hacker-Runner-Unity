using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageController : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _fillImage;
    [SerializeField] private TextMeshProUGUI _percentageText;

    [SerializeField] private PlayerDeathManager _playerDeathManagerScript;

    [SerializeField] private AudioSource _audioZombieHit;
    [SerializeField] private AudioSource _audioDeathPlayer;

    [SerializeField] private float _fillChangeDuration;
    [SerializeField] private float _damageFadeDuration;

    public float TimeBeforeFadeDamage;

    private float _fillAmount = 1.0f;
    private int _health = 100;

    private void Start()
    {
        _canvasGroup.gameObject.SetActive(false);
        _canvasGroup.alpha = 0;
    }

    public void StartFadeCoroutine()
    {
        StartCoroutine(FadeCanvas());
    }

    private IEnumerator FadeCanvas()
    {
        _canvasGroup.gameObject.SetActive(true);

        yield return Fade(0, 1, _damageFadeDuration);

        yield return Fade(1, 0, _damageFadeDuration);

        _canvasGroup.gameObject.SetActive(false);
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            _canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _canvasGroup.alpha = endAlpha;
    }

    public void UpdateHealth()
    {
        _audioZombieHit.Play();

        _health -= 20;
        _fillAmount = _health / 100.0f;

        if (_health <= 0)
        {
            _health = 0;
            _audioDeathPlayer.Play();
            _playerDeathManagerScript.OnPlayerDeath();
        }

        StartCoroutine(UpdateFillAmountCoroutine());
        StartCoroutine(UpdatePercentageTextCoroutine(_health));
    }

    private IEnumerator UpdateFillAmountCoroutine()
    {
        float startFill = _fillImage.fillAmount;
        float elapsedTime = 0;

        while (elapsedTime < _fillChangeDuration)
        {
            _fillImage.fillAmount = Mathf.Lerp(startFill, _fillAmount, elapsedTime / _fillChangeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _fillImage.fillAmount = _fillAmount;
    }

    private IEnumerator UpdatePercentageTextCoroutine(int targetPercentage)
    {
        int startPercentage = int.Parse(_percentageText.text.Replace("%", ""));
        float elapsedTime = 0;

        while (elapsedTime < _fillChangeDuration)
        {
            int currentPercentage = (int)Mathf.Lerp(startPercentage, targetPercentage, elapsedTime / _fillChangeDuration);
            _percentageText.text = currentPercentage + "%";
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _percentageText.text = targetPercentage + "%";
    }
}
