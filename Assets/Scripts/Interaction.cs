using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class InteractableObject : MonoBehaviour
{
    public string sceneToLoad = "WashScene";
    public KeyCode interactKey = KeyCode.E;
    public TextMeshProUGUI interactText;

    private bool playerInRange = false;

    void Start()
    {
        if (interactText != null)
            interactText.enabled = false;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactText != null)
                interactText.enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactText != null)
                interactText.enabled = false;
        }
    }
}

