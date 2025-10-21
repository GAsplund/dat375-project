using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneInteractable : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;
    public TextMeshProUGUI interactText;
    public int cost = 0;
    public string sceneToLoad;

    private bool playerInRange = false;
    private int currentMoney = 0;

    void OnEnable() => MoneyManager.OnValueChange += UpdateMoney;
    void OnDisable() => MoneyManager.OnValueChange -= UpdateMoney;

    void UpdateMoney(int money) => currentMoney = money;

    void Start()
    {
        if (interactText != null)
            interactText.enabled = false;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            if (currentMoney >= cost)
            {
                if (cost > 0)
                    MoneyManager.Subtract(cost);

                if (!string.IsNullOrEmpty(sceneToLoad))
                    SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                Debug.Log("Not enough money!");
            }
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
