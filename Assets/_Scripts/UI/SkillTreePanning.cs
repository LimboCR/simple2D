using UnityEngine;
using UnityEngine.EventSystems;

public class SkillTreePanning : MonoBehaviour, IPointerDownHandler, IDragHandler, IScrollHandler
{
    private RectTransform _rectTransform;
    private Vector2 _lastMousePosition;

    [Header("Zoom Settings")]
    public float zoomSpeed = 0.1f;
    public float minZoom = 0.5f;
    public float maxZoom = 2f;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out _lastMousePosition
        );
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentMousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out currentMousePosition
        );

        Vector2 adjustedDelta = eventData.delta / _rectTransform.localScale.x;
        _rectTransform.anchoredPosition += adjustedDelta;
        _lastMousePosition = currentMousePosition;
    }

    public void OnScroll(PointerEventData eventData)
    {
        float scrollDelta = eventData.scrollDelta.y;
        float newScale = Mathf.Clamp(_rectTransform.localScale.x + scrollDelta * zoomSpeed, minZoom, maxZoom);
        _rectTransform.localScale = new Vector3(newScale, newScale, 1f);
    }
}