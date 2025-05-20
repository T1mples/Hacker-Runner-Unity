using UnityEngine;

public class AttackButton : MonoBehaviour
{
    public void OnAttackButtonPressed()
    {
        if (ActiveEnemyManager.Instance.ActiveEnemy != null)
        {
            ActiveEnemyManager.Instance.ActiveEnemy.AttackByButton();
        }
    }
}
