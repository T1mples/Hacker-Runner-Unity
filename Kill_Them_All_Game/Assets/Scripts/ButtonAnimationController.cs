using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimationController : MonoBehaviour
{
    private Animator _animator;


    private void Start()
    {
        if (SceneManagerSingleton.Instance.CurrentSceneIndex == 1)
        {
            _animator = GetComponent<Animator>();

            _animator.SetBool("WasPressed", false);
        }
    }

    public void OnClick()
    {
        _animator.SetBool("WasPressed", true);
    }

    public void OnPointerExit()
    {
        _animator.SetBool("WasPressed", false);

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnPointerClick()
    {
        AudioManager.Instance.PlayAudioClick();
    }
}
