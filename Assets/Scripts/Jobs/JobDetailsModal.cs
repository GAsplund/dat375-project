using System.Linq;
using UnityEngine;
using TMPro;

public class JobDetailsModal : MonoBehaviour
{
    public static JobDetailsModal Instance { get; private set; }

    [SerializeField] private GameObject panel; // panel root to enable/disable
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI detailsText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (panel != null) panel.SetActive(false);
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
    }
}