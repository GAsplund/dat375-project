using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class InteractableObject : MonoBehaviour
{
    public string sceneToLoad = "WashScene";
    public float interactionDistance = 1.5f;
    public KeyCode interactKey = KeyCode.E;

    public TextMeshProUGUI interactText;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        interactText.enabled = false;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(player.position, transform.position);

        if (distance <= interactionDistance)
        {
            interactText.enabled = true;

            if (Input.GetKeyDown(interactKey))
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
        else
        {
            interactText.enabled = false;
        }
    }
}
