using UnityEngine;

public class FinishTriggerObjectInPlayer : MonoBehaviour
{
    [SerializeField] private PlayerController _playControllerScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinishTrigger"))
        {
            _playControllerScript.OnFinishTrigger();
        }
    }
}
