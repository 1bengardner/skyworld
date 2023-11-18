using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UmberJackDialogue))]
[CanEditMultipleObjects]
public class UmberJackDialogueEditor : Editor {

    public override void OnInspectorGUI()
    {
        UmberJackDialogue script = target as UmberJackDialogue;

        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(script), typeof(MonoScript), true);
        GUI.enabled = true;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("zone"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("dialogues"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("dialogueSound"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("celebrateSound"), true);

        using (var group = new EditorGUILayout.FadeGroupScope(script.zone == UmberJackDialogue.Area.Grass ? 1 : 0))
        {
            if (group.visible)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("axe"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("forestWarp"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("tree"), true);
            }
        }

        using (var group = new EditorGUILayout.FadeGroupScope(script.zone == UmberJackDialogue.Area.Snow ? 1 : 0))
        {
            if (group.visible)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("flashlight"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("rollingPinPrefab"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("dungeonWarp"), true);
            }
        }

        using (var group = new EditorGUILayout.FadeGroupScope(script.zone == UmberJackDialogue.Area.Desert ? 1 : 0))
        {
            if (group.visible)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pencilPrefab"), true);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
