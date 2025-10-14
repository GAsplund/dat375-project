using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public SpriteRenderer ButtonUp;
    public SpriteRenderer ButtonDown;

    public SceneAsset GameScene;


    private void OnMouseEnter()
    {
        ButtonUp.color = new Color(0.8f, 0.8f, 0.8f);
    }

    private void OnMouseExit()
    {
        ButtonUp.color = new Color(1.0f, 1.0f, 1.0f);
    }
    private void OnMouseDown()
    {
        ButtonUp.enabled = false;
        ButtonDown.enabled = true;
    }

    private void OnMouseUp()
    {
         SceneManager.LoadScene(GameScene.name);
    }
}
