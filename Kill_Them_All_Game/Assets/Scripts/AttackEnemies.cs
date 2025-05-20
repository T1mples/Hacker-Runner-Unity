using System.Collections;
using UnityEngine;

public class AttackEnemies : MonoBehaviour
{
    [SerializeField] private ObjectCutting _objectCuttingScript;
    [SerializeField] private Transform _rightArm;
    [SerializeField] private float _attackDuration;

    private bool _isAttacking;


    private void OnEnable() 
    {
        if (SceneManagerSingleton.Instance.CurrentSceneIndex == 2)
        {
            ActiveEnemyManager.Instance.SetActiveEnemy(this);
        }
    }

    private void Update()
    {
        if (!_isAttacking && GameManager.IsGameStarted)
        {
            AttackBySpace();
        }
    }

    private void AttackBySpace()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.Instance.PlayAudioSwordHit();
            StartCoroutine(AttackCoroutine());
        }
    }

    private IEnumerator AttackCoroutine()
    {
        _isAttacking = true;
        float elapsedTime = 0;

        Quaternion initialRotation = _rightArm.localRotation;
        Quaternion attackRotation = Quaternion.Euler(90, 0, 0);

        while (elapsedTime < _attackDuration)
        {
            _rightArm.localRotation = Quaternion.Lerp(initialRotation, attackRotation, elapsedTime / _attackDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _rightArm.localRotation = attackRotation;
        _objectCuttingScript.CutObjects();

        elapsedTime = 0;
        while (elapsedTime < _attackDuration)
        {
            _rightArm.localRotation = Quaternion.Lerp(attackRotation, initialRotation, elapsedTime / _attackDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _rightArm.localRotation = initialRotation;
        _isAttacking = false;
    }

    public void AttackByButton()
    {
        if (!_isAttacking && GameManager.IsGameStarted)
        {
            AudioManager.Instance.PlayAudioSwordHit();
            StartCoroutine(AttackCoroutine());
        }
    }
}
