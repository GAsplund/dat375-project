using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SceneChangerButton : MonoBehaviour, IPointerClickHandler
{
    public SceneAsset GameScene;

    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene(GameScene.name);
    }
}
