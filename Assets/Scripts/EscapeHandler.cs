using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeHandler : MonoBehaviour
{
    [Tooltip("Scene to load when Escape is pressed.")]
    [SerializeField] private string sceneToLoad = "InteractionScene";
    private readonly List<Func<bool>> responders = new();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!TryHandleWithResponders())
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }

    /// <summary>
    /// Registers a responder that can intercept Escape presses.
    /// </summary>
    public void RegisterResponder(Func<bool> callback)
    {
        if (callback == null) return;

        if (responders.Contains(callback)) return;

        responders.Add(callback);
    }

    /// <summary>
    /// Removes a previously registered responder.
    /// </summary>
    public void UnregisterResponder(Func<bool> callback)
    {
        if (callback == null) return;

        responders.Remove(callback);
    }

    private bool TryHandleWithResponders()
    {
        if (responders.Count == 0) return false;

        var snapshot = responders.ToArray();

        foreach (var responder in snapshot)
        {
            try
            {
                if (responder != null && responder())
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        return false;
    }
}
