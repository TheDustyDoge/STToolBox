using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

class InputGenerationWindow : EditorWindow
{
    static bool showSets = true;
    static List<InputSet> inputSets;

    static InputSet activeSet;
    static bool[] actionsShown;

    [MenuItem("Window/ST Input Generation")]
    public static void ShowWindow()
    {
        GetWindow(typeof(InputGenerationWindow));
    }

    private void OnGUI()
    {
        //GUI.skin.button.alignment = TextAnchor.MiddleLeft;
        //

        if (inputSets == null) 
            inputSets = new List<InputSet>();
        //

        GUILayout.Label("Global Input Settings", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        DrawInputSetButtons();
        EditorGUILayout.Space();

        if (activeSet != null)
        {
            DrawActiveSetSettings();
            EditorGUILayout.Space();
            DrawInputSet(activeSet);
        }
        else
        {
            GUILayout.Label("Select an Input Set.");
        }
    }

    void DrawInputSetButtons()
    {
        showSets = EditorGUILayout.Foldout(showSets, "Show All Input Sets");
        if (!showSets)
            return;

        float buttonWidth = 120;
        int columns = (int)(position.width / buttonWidth + 0.5f);
        GUILayout.Label("Width: " + position.width + ": cols " + columns);

        if (GUILayout.Button("+Add Set", GUILayout.MaxWidth(70)))
        {
            inputSets.Add(new InputSet("Set_" + inputSets.Count));
        }

        GUILayout.BeginHorizontal();
        for (int j = 0; j < inputSets.Count && j < columns; j++)
        {
            GUILayout.BeginVertical(GUILayout.MaxWidth(buttonWidth));
            for (int i = 0; j + i < inputSets.Count; i += columns)
            {
                if (GUILayout.Button(inputSets[j + i].name))
                {
                    activeSet = inputSets[j + i];
                }
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
    }

    void DrawActiveSetSettings()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Active Set: ", GUILayout.Width(70));
        activeSet.name = GUILayout.TextField(activeSet.name);
        if (GUILayout.Button("Delete", GUILayout.MaxWidth(60)))
        {
            if (EditorUtility.DisplayDialog("Delete Input Set", "Are you sure you want to delete this Input Set?", "Yes, Delete it", "No, Keep it"))
            {
                inputSets.Remove(activeSet);
                activeSet = null;
            }
        }
        GUILayout.EndHorizontal();
    }

    void DrawInputSet(InputSet set)
    {
        if (activeSet == null)
            return;
        else if (actionsShown == null || actionsShown.Length != activeSet.inputActions.Count)
            actionsShown = new bool[activeSet.inputActions.Count];

        GUILayout.Label("Input Actions", GUILayout.Width(80));
        for (int i = 0; i < set.inputActions.Count; i++)
        {
            actionsShown[i] = EditorGUILayout.Foldout(actionsShown[i], set.inputActions[i].name);
            if (actionsShown[i])
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Name: ", GUILayout.Width(45));
                GUILayout.FlexibleSpace();
                set.inputActions[i].name = GUILayout.TextField(set.inputActions[i].name, GUILayout.MinWidth(140), GUILayout.Width(300));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label("Type: ", GUILayout.Width(45));
                GUILayout.FlexibleSpace();
                set.inputActions[i].type = (InputAction.Type)EditorGUILayout.EnumPopup(set.inputActions[i].type, GUILayout.MaxWidth(300));
                GUILayout.EndHorizontal();
            }
        }
        if (GUILayout.Button("+", GUILayout.Width(20)))
        {
            set.inputActions.Add(new InputAction("NewAction"));
        }
    }

    // =========== \\
    // = Getters = \\
    // =========== \\

    InputSet GetSetNamed(string name)
    {
        for (int i = 0; i < inputSets.Count; i++)
        {
            if (inputSets[i].name == name)
                return inputSets[i];
        }
        return null;
    }




}
