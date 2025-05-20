using UnityEngine;
using System.Collections.Generic;

public class CuttingManager : MonoBehaviour
{
    public static CuttingManager Instance { get; private set; }
    public List<GameObject> KesobjList = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
