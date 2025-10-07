using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
public class HoverSpriteChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField, Tooltip("Sprite used when the pointer is not hovering. If left empty, the current SpriteRenderer sprite will be used.")]
    private Sprite defaultSprite;

    [SerializeField, Tooltip("Sprite used when the pointer is hovering.")]
    private Sprite hoverSprite;

    private SpriteRenderer spriteRenderer;
    private CursorManager cursorManager;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("HoverSpriteChanger requires a SpriteRenderer. Disabling component.");
            enabled = false;
            return;
        }

        if (defaultSprite == null)
        {
            defaultSprite = spriteRenderer.sprite;
        }

        spriteRenderer.sprite = defaultSprite;

        cursorManager = FindObjectOfType<CursorManager>();
        if (cursorManager == null)
        {
            Debug.LogWarning("CursorManager not found in the scene. Cursor changes will be ignored.");
        }
    }

    private void OnValidate()
    {
        // If no default sprite set, try to fill it from the SpriteRenderer.
        if (defaultSprite == null)
        {
            var sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                defaultSprite = sr.sprite;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSprite != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = hoverSprite;
        }

        cursorManager?.SetHovering(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = defaultSprite;
        }

        cursorManager?.SetHovering(false);
    }
}
