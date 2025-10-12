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

    public ParticleSystem bubbleSystemPrefab; 
    public float bubbleLifetime = 0.5f;  

    void Start()
    {
        // Get SpriteRender from blood
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
        if (other.CompareTag("Brush") && !isApproved)
        {
            foreach (var dirtRenderer in dirtRenderers)
            {
                if (dirtRenderer == null) continue;

                Collider2D dirtCollider = dirtRenderer.GetComponent<Collider2D>();
                if (dirtCollider != null && other.IsTouching(dirtCollider))
                {
                    // Opacity
                    Color c = dirtRenderer.color;
                    c.a -= 0.1f;
                    c.a = Mathf.Clamp01(c.a);
                    dirtRenderer.color = c;

                    //Create bubbles
                    Vector3 spawnPos = dirtRenderer.transform.position;
                    spawnPos += (Vector3)(Random.insideUnitCircle * 0.2f); // around the blood 
                    SpawnBubbleEffect(spawnPos);
                }
            }

            if (AllDirtClean())
            {
                StartCoroutine(ShowApprovedAndChangeScene());
            }
        }
    }

    void SpawnBubbleEffect(Vector3 position)
    {
        if (bubbleSystemPrefab == null) return;

        // Create bubbles
        ParticleSystem bubbleEffect = Instantiate(bubbleSystemPrefab, position, Quaternion.identity);
        bubbleEffect.Play();

        // Destroy bubbles
        Destroy(bubbleEffect.gameObject, bubbleLifetime);
    }

    bool AllDirtClean()
    {
        foreach (var dirtRenderer in dirtRenderers)
        {
            if (dirtRenderer != null && dirtRenderer.color.a > 0f)
                return false; // Blood not gone
        }
        return true; // Blood cleaned
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

        foreach (var dirt in dirtLayers)
        {
            if (dirt != null)
                dirt.SetActive(false);
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}

