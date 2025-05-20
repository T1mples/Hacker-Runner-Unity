using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveText;

    private int currentWave = 1;

    [DllImport("__Internal")]
    private static extern void SetToLeaderboard(int value);

    private void Start()
    {
        if (PlayerPrefs.HasKey("Wave"))
        {
            currentWave = PlayerPrefs.GetInt("Wave");
        }

        UpdateWaveText();
        UpdateLeaderboard();
    }

    private void UpdateWaveText()
    {
        if (Language.Instance.CurrentLanguage == "ru")
        {
            waveText.text = "Волна: " + currentWave.ToString();
        }
        else
        {
            waveText.text = "Wave: " + currentWave.ToString();
        }
    }

    public void StartNextWave()
    {
        currentWave++;
        PlayerPrefs.SetInt("Wave", currentWave);
        UpdateWaveText();
        UpdateLeaderboard();
    }

    private void UpdateLeaderboard()
    {
        SetToLeaderboard(currentWave);
    }
}
