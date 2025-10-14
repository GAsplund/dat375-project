using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
{
    [SerializeField] private UnityEvent onClick;
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip mouseDownSound;
    [SerializeField] private AudioClip mouseUpSound;

    public void OnPointerEnter(PointerEventData eventData)
    {
        CursorManager.SetHovering(true, hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CursorManager.SetHovering(false);
    }

    public void OnPointerDown(PointerEventData _)
    {
        AudioManager.instance?.PlayOneShotEffect(mouseDownSound, transform);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.instance?.PlayOneShotEffect(mouseUpSound, transform);
        onClick?.Invoke();
    }
}
