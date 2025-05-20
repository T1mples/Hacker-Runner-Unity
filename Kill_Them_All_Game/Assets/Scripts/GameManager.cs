using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void RateGame();
    
    [DllImport("__Internal")]
    private static extern void ShowAdvCloseShop();

    [SerializeField] private List<GameObject> _skins = new List<GameObject>();

    [SerializeField] private GameObject _cameraWalk;
    [SerializeField] private GameObject _cameraShop;

    [SerializeField] private CanvasGroup _canvasControlRules;

    [SerializeField] private GameObject _canvasScore;
    [SerializeField] private GameObject _canvasMain;
    [SerializeField] private GameObject _canvasShop;
    [SerializeField] private GameObject _canvasBar;
    [SerializeField] private GameObject _canvasMobileControls;

    [SerializeField] private Shop _shopScript;
    [SerializeField] private PanelManager _panelManagerScript;
    [SerializeField] private SkinDescriptionManager _skinDescriptionManagerScript;

    [SerializeField] private Light _directionalLight;
    [SerializeField] private float _lightTransitionDuration;

    [SerializeField] private AudioSource _audioBG;

    public static bool IsGameStarted = false;
    public static bool IsShopOpen = false;


    private void Start()
    {
        IsGameStarted = false;
        IsShopOpen = false;

        int selectedSkinIndex = PlayerPrefs.GetInt("SelectedScin", 0);
        ActivateSkin(selectedSkinIndex);
    }

    private void ActivateSkin(int index)
    {
        foreach (GameObject skin in _skins)
        {
            skin.SetActive(false);
        }
        _skins[index].SetActive(true);
    }

    private IEnumerator ChangeLightIntensity(float from, float to, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            _directionalLight.intensity = Mathf.Lerp(from, to, elapsedTime / duration);
            yield return null;
        }

        _directionalLight.intensity = to;
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

    public void StartGame()
    {        
        AudioManager.Instance.StopAudioBG();

        bool isFirstRun = PlayerPrefs.GetInt("IsFirstRun", 1) == 1;
        int currentSceneIndex = SceneManagerSingleton.Instance.CurrentSceneIndex;

        if (isFirstRun && currentSceneIndex == 1)
        {
            _canvasMain.SetActive(false);
            _canvasBar.SetActive(true);
            _canvasControlRules.gameObject.SetActive(true);
            StartCoroutine(FadeCanvasGroup(_canvasControlRules, 0f, 1f, 1.5f));
            PlayerPrefs.SetInt("IsFirstRun", 0);
            PlayerPrefs.Save();
        }
        else if (currentSceneIndex == 1)
        {
            _canvasMain.SetActive(false);
            _canvasBar.SetActive(true);
            IsGameStarted = true;
        }
        else if (currentSceneIndex == 2)
        {
            _canvasMain.SetActive(false);
            _canvasBar.SetActive(true);
            _canvasMobileControls.SetActive(true);
            IsGameStarted = true;
        }
    }

    public void CloseControlRulesAndContinue()
    {
        _canvasControlRules.gameObject.SetActive(false);
        IsGameStarted = true;
    }

    public void OpenShop()
    {
        IsShopOpen = true;

        _cameraWalk.SetActive(false);
        _cameraShop.SetActive(true);

        _canvasMain.SetActive(false);
        _canvasShop.SetActive(true);

        int currentSkinIndex = PlayerPrefs.GetInt("CurrentSkinIndex", 0);
        _shopScript.SetCurrentSkinIndex(currentSkinIndex);

        if (SceneManagerSingleton.Instance.CurrentSceneIndex == 1)
        {
            StartCoroutine(ChangeLightIntensity(0f, 0.95f, _lightTransitionDuration));
        }

        _skinDescriptionManagerScript.StartFadeIn();
        _skinDescriptionManagerScript.UpdateDescription(currentSkinIndex);
    }

    public void ShowAdvCloseShopBtn()
    {
        _audioBG.Stop();
        ShowAdvCloseShop();
    }

    public void CloseShopExtern()
    {
        _audioBG.Play();
        CloseShop();
    }

    public void CloseShop()
    {
        IsShopOpen = false;

        _cameraShop.SetActive(false);
        _cameraWalk.SetActive(true);

        _canvasShop.SetActive(false);
        _canvasMain.SetActive(true);

        int currentSkinIndex = PlayerPrefs.GetInt("SelectedScin", 0);
        ActivateSkin(currentSkinIndex);

        if (SceneManagerSingleton.Instance.CurrentSceneIndex == 1)
        {
            StartCoroutine(ChangeLightIntensity(0.95f, 0f, _lightTransitionDuration));
        }

        _skinDescriptionManagerScript.DeactivateCanvas();
        _panelManagerScript.UpdatePanel(currentSkinIndex);
    }

    public void RateGameBtn()
    {
        RateGame();
    }
}
