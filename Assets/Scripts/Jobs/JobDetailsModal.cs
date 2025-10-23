using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class JobDetailsModal : MonoBehaviour
{
    public static JobDetailsModal Instance { get; private set; }

    [SerializeField] private GameObject panel; // panel root to enable/disable
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI detailsText;
    [SerializeField] private TextMeshProUGUI storyText;
    [SerializeField] private string sceneToLoad = "InteractionScene"; // Scene to load when job is accepted
    [SerializeField] private AudioClip openSound;

    private Job currentJob;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (panel != null) panel.SetActive(false);
    }

    private void OnEnable()
    {
        FindObjectOfType<EscapeHandler>()?.RegisterResponder(HandleEscape);
    }

    private void OnDisable()
    {
        FindObjectOfType<EscapeHandler>()?.UnregisterResponder(HandleEscape);
    }

    public void Show(Job job)
    {
        if (job == null || panel == null) return;

        AudioManager.instance?.PlayOneShotSfx(openSound);

        currentJob = job; // Store reference to current job
        panel.SetActive(true);
        titleText.text = $"Job for {job.forGang}";
        detailsText.text = $"Reward: {job.reward} gold\nItems: {job.NumberOfClothes()}";
        storyText.text = job.noteDescription;
    }

    public void Hide()
    {
        if (panel != null) panel.SetActive(false);
        currentJob = null;
        CursorManager.SetHovering(false); // Work around for the fact that the button disappears before OnPointerExit is called
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

        CursorManager.SetHovering(false);
        JobManager.SetJob(currentJob);
        SceneManager.LoadScene(sceneToLoad);
    }

    /// <summary>
    /// Call this method when the Decline button is clicked.
    /// It declines the current job and hides the modal.
    /// </summary>
    public void Decline()
    {
        if (currentJob == null)
        {
            Debug.LogWarning("No job selected to decline!");
            Hide();
            return;
        }

        var generator = FindObjectOfType<JobGenerator>();
        if (generator == null)
        {
            Debug.LogWarning("JobGenerator not found in the scene. Cannot decline job.");
            Hide();
            currentJob = null;
            return;
        }

        generator.RemoveJob(currentJob);

        currentJob = null;
        Hide();
    }

    private bool HandleEscape()
    {
        if (panel != null && panel.activeSelf)
        {
            Hide();
            return true;
        }

        return false;
    }
}