using System.Runtime.InteropServices;
using UnityEngine;

public class Language : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void InitializeLang();

    public string CurrentLanguage;

    public static Language Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitLang();
    }

    private void InitLang()
    {
        InitializeLang();
    }

    public void SetLanguage(string lang)
    {
        CurrentLanguage = lang;
    }
}
