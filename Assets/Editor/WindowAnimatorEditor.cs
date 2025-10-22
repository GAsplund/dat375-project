using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WindowAnimator), true)]
public class WindowAnimatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WindowAnimator windowAnimator = (WindowAnimator)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);

        EditorGUI.BeginDisabledGroup(!Application.isPlaying);
        if (GUILayout.Button("Trigger Shot (Debug)"))
        {
            windowAnimator.TriggerShotOnce();
        }
        EditorGUI.EndDisabledGroup();
    }
}
