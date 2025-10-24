using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SceneChangerButton : MonoBehaviour, IPointerClickHandler
{
   
    public string SceneToChange;

    public void OnPointerClick(PointerEventData eventData)
    {
      
       SceneManager.LoadScene(SceneToChange);
    }
}
