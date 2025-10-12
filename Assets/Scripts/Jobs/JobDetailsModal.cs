using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class JobDetailsModal : MonoBehaviour
{
    public static JobDetailsModal Instance { get; private set; }

    [SerializeField] private GameObject panel; // panel root to enable/disable
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI detailsText;
    [SerializeField] private string sceneToLoad = "InteractionScene"; // Scene to load when job is accepted

    private CursorManager cursorManager;
    private Job currentJob;

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

        currentJob = job; // Store reference to current job
        panel.SetActive(true);
        titleText.text = $"Job for {job.forGang}";
        detailsText.text = $"Reward: {job.reward} gold\nItems: {job.NumberOfClothes()}";
    }

    public void Hide()
    {
        if (panel != null) panel.SetActive(false);
        cursorManager?.SetHovering(false); // Work around for the fact that the button disappears before OnPointerExit is called
    }

    /// <summary>
    /// Call this method when the Accept button is clicked.
    /// It stores the current job in JobManager and loads the cleaning scene.
    /// </summary>
    public void AcceptJob()
    {
        if (currentJob == null)
        {
            Debug.LogWarning("No job selected to accept!");
            return;
        }

        JobManager.SetJob(currentJob);
        SceneManager.LoadScene(sceneToLoad);
    }
}