using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private static CursorManager Instance;

    [Header("Default Cursor Settings")]
    [SerializeField] private Texture2D defaultCursorTexture;
    [SerializeField] private Vector2 defaultHotSpot = Vector2.zero;

    [Header("Hover Cursor Settings")]
    [SerializeField] private Texture2D hoverCursorTexture;
    [SerializeField] private Vector2 hoverHotSpot = Vector2.zero;

    [Header("Click Cursor Settings")]
    [SerializeField] private Texture2D clickCursorTexture;
    [SerializeField] private Vector2 clickHotSpot = Vector2.zero;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickDownSound;
    [SerializeField] private AudioClip clickUpSound;

    private bool isHovering = false;

    private enum CursorState { Default, Hover, Clicked }
    private CursorState currentCursorState = CursorState.Default;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

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

    public static void SetHovering(bool hovering, bool playSound = true) => SetHovering(hovering, null, playSound);

    public static void SetHovering(bool hovering, AudioClip customSound, bool playSound = true)
    {
        if (Instance == null) return;
        
        Instance.isHovering = hovering;

        if (!playSound || !hovering) return;
        AudioManager.instance?.PlayOneShotEffect(customSound ?? Instance.hoverSound, Instance.transform);
    }

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