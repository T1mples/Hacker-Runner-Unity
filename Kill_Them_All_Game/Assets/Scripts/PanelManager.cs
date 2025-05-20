using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _panels;

    private void Start()
    {
        UpdatePanelOnStart();
    }

    private void UpdatePanelOnStart()
    {
        int selectedSkinIndex = PlayerPrefs.GetInt("SelectedScin", 0);
        UpdatePanel(selectedSkinIndex);
    }

    public void UpdatePanel(int index)
    {
        for (int i = 0; i < _panels.Length; i++)
        {
            _panels[i].SetActive(i == index);
        }
    }

    public void OnSkinChanged(int index)
    {
        UpdatePanel(index);
    }
}
