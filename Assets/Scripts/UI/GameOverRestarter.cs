using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameOverRestarter : MonoBehaviour
{
   
    public string SceneToChange;

    [Tooltip("Scene to load when Escape is pressed.")]
    [SerializeField] private string sceneToLoad = "MenuScene";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            foreach (GameObject obj in GetDontDestroyOnLoadObjects())
            {
                Destroy(obj);
            }
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    public static GameObject[] GetDontDestroyOnLoadObjects()
    {
        GameObject temp = null;
        try
        {
            temp = new GameObject();
            Object.DontDestroyOnLoad( temp );
            UnityEngine.SceneManagement.Scene dontDestroyOnLoad = temp.scene;
            Object.DestroyImmediate( temp );
            temp = null;

            return dontDestroyOnLoad.GetRootGameObjects();
        }
        finally
        {
            if( temp != null )
                Object.DestroyImmediate( temp );
        }
    }
}
