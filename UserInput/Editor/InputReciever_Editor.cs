using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(BasicInputReciever))]
public class BasicInputReciever_Editor : Editor
{
    bool[] folds;
    public override void OnInspectorGUI()
    {
        BasicInputReciever script = target as BasicInputReciever;
        script.debugMode = EditorGUILayout.Toggle("Debug Mode: ", script.debugMode);
        script.totalInputs = EditorGUILayout.IntField("Number of Inputs: ", script.totalInputs);

        // Recreate if needed
        if (script.inputActions == null || script.totalInputs != script.inputActions.Length)
        {
            InputSystem.InputAction[] newInputActions = new InputSystem.InputAction[script.totalInputs];
            UnityEvent[] newOnInputActivated = new UnityEvent[script.totalInputs];
            UnityEvent[] newOnInputDeactivated = new UnityEvent[script.totalInputs];
            for (int i = 0; i < script.totalInputs && i < script.inputActions.Length; i++)
            {
                newInputActions[i] = script.inputActions[i];
                newOnInputActivated[i] = script.OnInputActivated[i];
                newOnInputDeactivated[i] = script.OnInputDeactivated[i];
            }
            script.inputActions = newInputActions;
            script.OnInputActivated = newOnInputActivated;
            script.OnInputDeactivated = newOnInputDeactivated;
            Undo.RecordObject(target, "Change Number of Inputs");
        }

        // No repeats
        for (int i = 0; i < script.totalInputs; i++)
        {
            if (script.inputActions[i] == InputSystem.InputAction.None)
                continue;

            for (int j = 0; j < i; j++)
            {
                if (script.inputActions[j] == script.inputActions[i])
                {
                    //script.inputActions[i] = InputSystem.InputAction.None;
                    Debug.Log("Error: both inputs " + j + " and " + i + " have input " + script.inputActions[j] + ".");
                    break;
                }
            }
        }

        // Display each input action
        serializedObject.Update();
        if (folds == null || folds.Length != script.totalInputs)
        {
            folds = new bool[script.totalInputs];
        }
        for (int i = 0; i < script.totalInputs; i++)
        {
            folds[i] = EditorGUILayout.Foldout(folds[i], script.inputActions[i] != InputSystem.InputAction.None ? script.inputActions[i].ToString() : "Input " + i + " (Empty)");
            if (folds[i])
            {
                script.inputActions[i] = (InputSystem.InputAction)EditorGUILayout.EnumPopup("Input Type: ", script.inputActions[i]);
                if (script.inputActions[i] != InputSystem.InputAction.None)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("OnInputActivated").GetArrayElementAtIndex(i), label: new GUIContent("On " + script.inputActions[i] + " Input Activated: "));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("OnInputDeactivated").GetArrayElementAtIndex(i), label: new GUIContent("On " + script.inputActions[i] + " Input Deactivated: "));
                }
                else
                {
                    EditorGUILayout.LabelField("Select an Input Action to edit events.");
                }
            }
        }
        serializedObject.ApplyModifiedProperties();
    }

}
