using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class WashClothes : MonoBehaviour
{
    public GameObject[] dirtLayers; 
    private SpriteRenderer[] dirtRenderers;

    public string sceneToLoad = "InteractionScene";

    public TextMeshProUGUI approvedText;

    private bool isApproved = false;

    void Start()
    {
        // Get sprite renderer 
        dirtRenderers = new SpriteRenderer[dirtLayers.Length];
        for (int i = 0; i < dirtLayers.Length; i++)
        {
            if (dirtLayers[i] != null)
                dirtRenderers[i] = dirtLayers[i].GetComponent<SpriteRenderer>();
        }

        if (approvedText != null)
            approvedText.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Endast om borsten nuddar
        if (other.CompareTag("Brush") && !isApproved)
        {
            // Get which object it touches
            foreach (var dirtRenderer in dirtRenderers)
            {
                if (dirtRenderer == null) continue;

                // Soap touches blood object
                Collider2D dirtCollider = dirtRenderer.GetComponent<Collider2D>();
                if (dirtCollider != null && other.IsTouching(dirtCollider))
                {
                    Color c = dirtRenderer.color;
                    c.a -= 0.1f; // reduce opcaity
                    c.a = Mathf.Clamp01(c.a);
                    dirtRenderer.color = c;
                }
            }

            // All object cleaned -> change scene
            if (AllDirtClean())
            {
                StartCoroutine(ShowApprovedAndChangeScene());
            }
        }
    }

    bool AllDirtClean()
    {
        foreach (var dirtRenderer in dirtRenderers)
        {
            if (dirtRenderer != null && dirtRenderer.color.a > 0f)
                return false; // at least one dirt/blood object left
        }
        return true; //cleaned
    }

    IEnumerator ShowApprovedAndChangeScene()
    {
        isApproved = true;

        if (approvedText != null)
        {
            approvedText.enabled = true;
            approvedText.text = "Job Done!";
        }

        yield return new WaitForSeconds(2f);

        // Delete all blood
        foreach (var dirt in dirtLayers)
        {
            if (dirt != null)
                dirt.SetActive(false);
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}

