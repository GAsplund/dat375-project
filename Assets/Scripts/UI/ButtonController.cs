using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private CursorManager cursorManager;
    [SerializeField] private UnityEvent onClick;

    void Start()
    {
        cursorManager = FindObjectOfType<CursorManager>();
        if (cursorManager == null)
        {
            Debug.LogWarning("CursorManager not found in the scene. Cursor changes will be ignored.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cursorManager?.SetHovering(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cursorManager?.SetHovering(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
    }
}
