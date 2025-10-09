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
      ButtonUp.enabled = false;
       ButtonDown.enabled = true;
    }

    private void OnMouseExit()
    {
        ButtonUp.enabled = true;
        ButtonDown.enabled = false;
    }

    private void OnMouseDown()
    {
        SceneManager.LoadScene(GameScene.name);
    }
}
