using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableObject : MonoBehaviour
{
    public string sceneToLoad = "SampleScene";
    public KeyCode interactKey = KeyCode.E;

    private bool isNear = false;

    void Update()
    {
        // If player is in trigger
        if (isNear && Input.GetKeyDown(interactKey))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    //Get notified if you are in trigger in the console to test
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger");
            isNear = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited trigger");
            isNear = false;
        }
    }

}
