using UnityEngine;
using TMPro;
using System.Runtime.InteropServices;

public class ScoreManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void AddCoinsExtern();

    [SerializeField] private TextMeshProUGUI _scoreText;

    private int _score;


    private void Start()
    {
        LoadScore();
        UpdateScoreText();
    }

    private void LoadScore()
    {
        _score = PlayerPrefs.GetInt("Score", 0);
    }

    private void UpdateScoreText()
    {
        if (Language.Instance.CurrentLanguage == "ru")
        {
            _scoreText.text = "Ñ÷¸ò: " + _score.ToString();
        }else
        {
            _scoreText.text = "Score: " + _score.ToString();
        }
    }

    public void AddCoinsExternBtn()
    {
        AddCoinsExtern();
    }
    
    public void AddScoreInReward()
    {
        AddScore(35);
    }

    public void AddScore(int amount)
    {
        _score += amount;
        PlayerPrefs.SetInt("Score", _score);
        PlayerPrefs.Save();
        UpdateScoreText();
    }

    public int GetScore()
    {
        return _score;
    }
}
