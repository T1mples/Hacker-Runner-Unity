using UnityEngine;
using UnityEngine.EventSystems;

public class StaticJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private RectTransform _background;
    [SerializeField] private RectTransform _handle;

    [SerializeField] private float _handleLimit;
    [SerializeField] private float _deadZone;

    private Vector2 _input = Vector2.zero;

    public Vector2 InputDirection => (_input.magnitude > _deadZone) ? _input : Vector2.zero;


    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - (Vector2)_background.position;
        _input = (direction.magnitude > _background.sizeDelta.x / 2f)
            ? direction.normalized
            : direction / (_background.sizeDelta.x / 2f);

        _handle.anchoredPosition = _input * (_background.sizeDelta.x / 2f) * _handleLimit;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _input = Vector2.zero;
        _handle.anchoredPosition = Vector2.zero;
    }
}
