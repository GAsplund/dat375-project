using System.Linq;
using UnityEngine;
using TMPro;

public class JobDetailsModal : MonoBehaviour
{
    public static JobDetailsModal Instance { get; private set; }

    [SerializeField] private GameObject panel; // panel root to enable/disable
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI detailsText;

    private CursorManager cursorManager;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (panel != null) panel.SetActive(false);
    }

    private void Start()
    {
        cursorManager = FindObjectOfType<CursorManager>();
        if (cursorManager == null)
        {
            Debug.LogWarning("CursorManager not found in the scene. Cursor changes will be ignored.");
        }
    }

    public void Show(Job job)
    {
        if (job == null || panel == null) return;

        panel.SetActive(true);
        titleText.text = $"Job for {job.forGang}";
        detailsText.text = $"Reward: {job.reward} gold\nItems: {job.NumberOfClothes()}";
    }

    public void Hide()
    {
        if (panel != null) panel.SetActive(false);
        cursorManager?.SetHovering(false); // Work around for the fact that the button disappears before OnPointerExit is called
    }
}