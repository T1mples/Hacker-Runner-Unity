using UnityEngine;

public class ScinControll : MonoBehaviour
{
    [SerializeField] private int _scinIndex = 0;
    [SerializeField] private GameObject[] _scins;

    private void Start()
    {
        _scinIndex = PlayerPrefs.GetInt("SelectedScin", 0);
        foreach (GameObject sck in _scins)
            sck.SetActive(false);

        _scins[_scinIndex].SetActive(true);
    }
}
