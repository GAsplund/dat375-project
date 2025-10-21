using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private UnityEvent onClick;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = new Color(0.8f, 0.8f, 0.8f);
    
    private SpriteRenderer spriteRenderer;
    private Image image;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        image = GetComponent<Image>();
        
        // Store the original color
        if (spriteRenderer != null)
            normalColor = spriteRenderer.color;
        else if (image != null)
            normalColor = image.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CursorManager.SetHovering(true);
        SetColor(hoverColor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CursorManager.SetHovering(false);
        SetColor(normalColor);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
    }

    private void SetColor(Color color)
    {
        if (spriteRenderer != null)
            spriteRenderer.color = color;
        else if (image != null)
            image.color = color;
    }
}
