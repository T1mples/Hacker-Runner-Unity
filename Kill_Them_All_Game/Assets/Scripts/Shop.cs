using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    [SerializeField] private ScoreManager _scoreManagerScript;
    [SerializeField] private WarningsShop _warningsShopScript;
    [SerializeField] private PanelManager _panelManagerScript;
    [SerializeField] private SkinDescriptionManager _skinDescriptionManagerScript;

    [SerializeField] private GameObject[] _scin;
    [SerializeField] private ScinBlueprint[] _scins;

    [SerializeField] private Button _buyButton;

    [SerializeField] private AudioSource _bought;
    [SerializeField] private AudioSource _notEnough;

    private int _scinIndex = 0;
    private int _lastUnlockedSkinIndex;

    private void Start()
    {
        foreach (ScinBlueprint sck in _scins)
        {
            if (sck.Price == 0)
                sck.IsUnlocked = true;
            else
                sck.IsUnlocked = PlayerPrefs.GetInt(sck.Name, 0) == 0 ? false : true;

            if (sck.IsUnlocked)
                _lastUnlockedSkinIndex = Array.IndexOf(_scins, sck);
        }
        _scinIndex = PlayerPrefs.GetInt("SelectedScin", 0);
        SetActiveSkin(_scinIndex);
        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
        UpdateScoreText();
    }

    private void SetActiveSkin(int index)
    {
        foreach (GameObject skin in _scin)
        {
            skin.SetActive(false);
        }
        _scin[index].SetActive(true);
    }

    private void UpdateUI()
    {
        ScinBlueprint c = _scins[_scinIndex];
        if (c.IsUnlocked)
        {
            if (Language.Instance.CurrentLanguage == "ru")
            {
                _buyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Выбрано";
            }else
            {
                _buyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Selected";
            }
            _buyButton.interactable = false;
        }
        else
        {
            _buyButton.GetComponentInChildren<TextMeshProUGUI>().text = c.Price.ToString();
            _buyButton.interactable = true;
        }
    }

    private void UpdateScoreText()
    {
        if (Language.Instance.CurrentLanguage == "ru")
        {
            _scoreText.text = "Счёт: " + _scoreManagerScript.GetScore().ToString();
        }else
        {
            _scoreText.text = "Score: " + _scoreManagerScript.GetScore().ToString();
        }
    }

    public void ChangeNext()
    {
        _scin[_scinIndex].SetActive(false);

        _scinIndex++;
        if (_scinIndex == _scin.Length)
            _scinIndex = 0;

        SetActiveSkin(_scinIndex);

        if (_scins[_scinIndex].IsUnlocked)
        {
            PlayerPrefs.SetInt("SelectedScin", _scinIndex);
            PlayerPrefs.Save();

            _panelManagerScript.OnSkinChanged(_scinIndex);
            _skinDescriptionManagerScript.OnSkinChanged(_scinIndex);
        }

        _skinDescriptionManagerScript.UpdateDescription(_scinIndex);
    }

    public void ChangePrevious()
    {
        _scin[_scinIndex].SetActive(false);

        _scinIndex--;
        if (_scinIndex == -1)
            _scinIndex = _scin.Length - 1;

        SetActiveSkin(_scinIndex);

        if (_scins[_scinIndex].IsUnlocked)
        {
            PlayerPrefs.SetInt("SelectedScin", _scinIndex);
            PlayerPrefs.Save();

            _panelManagerScript.OnSkinChanged(_scinIndex);
            _skinDescriptionManagerScript.OnSkinChanged(_scinIndex);
        }

        _skinDescriptionManagerScript.UpdateDescription(_scinIndex);
    }

    public void UnLockScin()
    {
        ScinBlueprint c = _scins[_scinIndex];
        int currentScore = _scoreManagerScript.GetScore();

        if (_scinIndex > 0)
        {
            ScinBlueprint previousSkin = _scins[_scinIndex - 1];
            if (!previousSkin.IsUnlocked)
            {
                if (Language.Instance.CurrentLanguage == "ru")
                {
                    _warningsShopScript.StartFadeWarningCoroutine("Предыдущее оружие не куплено!");
                }else
                {
                    _warningsShopScript.StartFadeWarningCoroutine("The previous weapon is not purchased!");
                }
                _notEnough.Play();
                return;
            }
        }

        if (currentScore >= c.Price && !c.IsUnlocked)
        {
            c.IsUnlocked = true;
            _lastUnlockedSkinIndex = _scinIndex;
            _scoreManagerScript.AddScore(-c.Price);

            _bought.Play();
            PlayerPrefs.SetInt(c.Name, 1);
            PlayerPrefs.SetInt("SelectedScin", _scinIndex);
            PlayerPrefs.Save();
            UpdateUI();
            SetActiveSkin(_scinIndex);

            PlayerPrefs.SetInt("LastUnlockedSkinIndex", _lastUnlockedSkinIndex);
            PlayerPrefs.SetInt("CurrentSkinIndex", _scinIndex);
            PlayerPrefs.Save();
        }
        else
        {
            if (Language.Instance.CurrentLanguage == "ru")
            {
                _warningsShopScript.StartFadeWarningCoroutine("Недостаточно очков для покупки!");
            }else
            {
                _warningsShopScript.StartFadeWarningCoroutine("Not enough points to buy!");
            }
            _notEnough.Play();
        }
    }


    public int GetLastUnlockedSkinIndex()
    {
        return _lastUnlockedSkinIndex;
    }

    public int GetCurrentSkinIndex()
    {
        return _scinIndex;
    }

    public void SetCurrentSkinIndex(int index)
    {
        _scinIndex = index;
        SetActiveSkin(_scinIndex);
        UpdateUI();
    }

    public void SetActiveLastUnlockedSkin()
    {
        SetActiveSkin(_lastUnlockedSkinIndex);
    }
}
