using UnityEngine;

public class ActiveEnemyManager : MonoBehaviour
{
    public static ActiveEnemyManager Instance { get; private set; }

    public AttackEnemies ActiveEnemy { get; private set; }

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

    public void SetActiveEnemy(AttackEnemies enemy)
    {
        ActiveEnemy = enemy;
    }
}
