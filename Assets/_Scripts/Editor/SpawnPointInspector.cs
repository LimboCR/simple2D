using UnityEditor;
using UnityEngine;
using static GUIUtils;

[CustomEditor(typeof(SpawnPointHandler))]
public class SpawnPointInspector : Editor
{
    private bool _showIDamagebleDropdown = false;
    private bool _showINPCMovableDropdown = false;
    public override void OnInspectorGUI()
    {
        #region GUI Variables
        GUIStyle headerStyle = new GUIStyle(GUI.skin.label);
        headerStyle.normal.textColor = Color.white;
        headerStyle.fontSize = 12;
        headerStyle.fontStyle = FontStyle.Bold;

        GUIStyle headerBoxStyle = new GUIStyle(GUI.skin.box);
        headerBoxStyle.normal.background = MakeTex(2, 2, Color.black);

        #endregion

        DrawDefaultInspector();

        SpawnPointHandler spawnPoint = (SpawnPointHandler)target;
        //if (GUILayout.Button("Open Spawn Settings"))
        //{
        //    SpawnSettingsWindow.OpenWindow(spawnPoint);
        //}

        if (spawnPoint.HealthScriptOverride)
        {
            GUILayout.Space(5f);
            GUILayout.BeginVertical(headerBoxStyle, GUILayout.Height(10), GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("Custom health settings were set for this NPC", headerStyle);
            EditorGUILayout.LabelField("If neccessary, you can re-modify them down below", EditorStyles.miniBoldLabel);
            GUILayout.EndVertical();

            GUILayout.BeginVertical("box");
            _showIDamagebleDropdown = EditorGUILayout.Foldout(_showIDamagebleDropdown, "Health Settings");
            if (_showIDamagebleDropdown)
            {
                EditorGUI.indentLevel++; // Indent for better UI hierarchy
                EditorGUILayout.FloatField("MaxHealth: ", spawnPoint.NPCMaxHealth, GUILayout.MaxWidth(80), GUILayout.ExpandWidth(true));
                EditorGUILayout.FloatField("RegenRate: ", spawnPoint.NPCRegenRate, GUILayout.MaxWidth(80), GUILayout.ExpandWidth(true)); ;
                EditorGUILayout.FloatField("RegenDelay: ", spawnPoint.NPCRegenDelay, GUILayout.MaxWidth(80), GUILayout.ExpandWidth(true));
                EditorGUI.indentLevel--;
            }
            GUILayout.EndVertical();
        }

        if (spawnPoint.INPCMovableOverride)
        {
            GUILayout.Space(5f);
            GUILayout.BeginVertical(headerBoxStyle, GUILayout.Height(10), GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("Custom movement settings were set for this NPC", headerStyle);
            EditorGUILayout.LabelField("If neccessary, you can re-modify them down below", EditorStyles.miniBoldLabel);
            GUILayout.EndVertical();

            GUILayout.BeginVertical("box");
            _showINPCMovableDropdown = EditorGUILayout.Foldout(_showINPCMovableDropdown, "Movement Settings");
            if (_showINPCMovableDropdown)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.FloatField("WalkSpeed: ", spawnPoint.WalkSpeed, GUILayout.MaxWidth(80), GUILayout.ExpandWidth(true));
                EditorGUILayout.FloatField("RunSpeed: ", spawnPoint.RunSpeed, GUILayout.MaxWidth(80), GUILayout.ExpandWidth(true));
                EditorGUILayout.FloatField("JumpHeight: ", spawnPoint.JumpHeight, GUILayout.MaxWidth(80), GUILayout.ExpandWidth(true));
                EditorGUI.indentLevel--;
            }
            GUILayout.EndVertical();
        }
    }

    
}