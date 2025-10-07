using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class WashClothes : MonoBehaviour
{
    public GameObject dirtLayer;
    private SpriteRenderer dirtRenderer;


    public string sceneToLoad = "MainScene";

    public TextMeshProUGUI approvedText;

    private bool isApproved = false;

    void Start()
    {
        if (dirtLayer != null)
            dirtRenderer = dirtLayer.GetComponent<SpriteRenderer>();

        if (approvedText != null)
            approvedText.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Brush") && dirtRenderer != null && !isApproved)
        {
            Color c = dirtRenderer.color;

            // Reduce alpha with 10%, opcaity
            c.a -= 0.1f;
            c.a = Mathf.Clamp01(c.a);
            dirtRenderer.color = c;

            //change scene when opacity reaches 0
            if (c.a <= 0f)
            {
                StartCoroutine(ShowApprovedAndChangeScene());
            }
        }
    }

    IEnumerator ShowApprovedAndChangeScene()
    {
        isApproved = true;

        if (approvedText != null)
        {
            approvedText.enabled = true;
            approvedText.text = "Job Done!";
        }

        // Wait 2 seconds
        yield return new WaitForSeconds(2f);

        // Remove dirt object
        if (dirtLayer != null)
            dirtLayer.SetActive(false);

        SceneManager.LoadScene(sceneToLoad);
    }
}
