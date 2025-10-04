using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [Header("Default Cursor Settings")]
    [SerializeField] private Texture2D defaultCursorTexture;
    [SerializeField] private Vector2 defaultHotSpot = Vector2.zero;

    [Header("Hover Cursor Settings")]
    [SerializeField] private Texture2D hoverCursorTexture;
    [SerializeField] private Vector2 hoverHotSpot = Vector2.zero;

    [Header("Click Cursor Settings")]
    [SerializeField] private Texture2D clickCursorTexture;
    [SerializeField] private Vector2 clickHotSpot = Vector2.zero;

    private bool isHovering = false;

    private enum CursorState { Default, Hover, Clicked }
    private CursorState currentCursorState = CursorState.Default;

    void Start()
    {
        ApplyState(CursorState.Default);
    }

    void Update()
    {
        // Determine desired state once
        CursorState desired;
        if (Input.GetMouseButton(0))
        {
            desired = CursorState.Clicked;
        }
        else if (isHovering)
        {
            desired = CursorState.Hover;
        }
        else
        {
            desired = CursorState.Default;
        }

        if (desired == currentCursorState) return;

        ApplyState(desired);
    }

    public void SetHovering(bool hovering) => isHovering = hovering;

    private void ApplyState(CursorState state)
    {
        currentCursorState = state;

        switch (state)
        {
            case CursorState.Clicked:
                SetCursor(clickCursorTexture, clickHotSpot);
                break;
            case CursorState.Hover:
                SetCursor(hoverCursorTexture, hoverHotSpot);
                break;
            case CursorState.Default:
            default:
                SetCursor(defaultCursorTexture, defaultHotSpot);
                break;
        }
    }

    private void SetCursor(Texture2D texture, Vector2 hotSpot)
    {
        if (texture == null)
        {
            Debug.LogWarning("Requested cursor texture is null. Reverting to default/system cursor.");
            // Use system cursor when no texture provided
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            return;
        }

        Cursor.SetCursor(texture, hotSpot, CursorMode.Auto);
    }
}