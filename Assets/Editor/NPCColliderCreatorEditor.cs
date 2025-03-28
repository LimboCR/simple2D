using UnityEngine;
using UnityEditor;
using static GUIUtils;

[CustomEditor(typeof(NPCColliderCreator))]
public class NPCColliderCreatorEditor : Editor
{
    private SerializedProperty enumProperty;

    private void OnEnable()
    {
        enumProperty = serializedObject.FindProperty("NPCTypeSelector");
    }

    public override void OnInspectorGUI()
    {
        #region variables
        NPCColliderCreator colliderInstantiator = (NPCColliderCreator)target;

        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.normal.textColor = Color.white;
        labelStyle.fontSize = 12;
        labelStyle.fontStyle = FontStyle.Bold;

        GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
        boxStyle.normal.background = MakeTex(2, 2, Color.black);

        #endregion

        #region Header
        GUILayout.BeginHorizontal(boxStyle, GUILayout.Height(10), GUILayout.ExpandWidth(true));
        GUI.backgroundColor = Color.green;
        GUILayout.Label("NPC Colliders Setup - by Limbo", labelStyle);
        GUI.backgroundColor = Color.white;
        GUILayout.EndHorizontal();
        GUILayout.Space(20f);

        #endregion

        DrawDefaultInspector();

        #region Instantiate colliders
        GUILayout.Space(20f);

        GUILayout.BeginVertical("box");
        if (GUILayout.Button("Instantiate Chase Zone Colliders"))
        {
            colliderInstantiator.InstantiateChaseTriggerColliders();
        }
        GUILayout.Space(3f);
        if (GUILayout.Button("Instantiate Attack Zone Colliders"))
        {
            colliderInstantiator.InstantiateAttackZoneCollider();
        }
        GUILayout.Space(3f);
        if (GUILayout.Button("Instantiate Visibility Zone Colliders"))
        {
            colliderInstantiator.InstantiateVisibilityZone();
        }

        GUILayout.EndVertical();

        GUILayout.Space(5f);
        GUILayout.BeginHorizontal("box");

        #endregion

        #region Bulk Colliders Commands
        GUI.backgroundColor = Color.magenta;
        if (GUILayout.Button("Instantiate All"))
        {
            colliderInstantiator.InstantiateChaseTriggerColliders();
            colliderInstantiator.InstantiateAttackZoneCollider();
            colliderInstantiator.InstantiateVisibilityZone();
        }
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Destroy All"))
        {
            colliderInstantiator.DeleteAllCreatedColliders();
        }
        GUILayout.EndHorizontal();
        #endregion

        #region Colliders assignment
        GUILayout.Space(5f);
        GUILayout.BeginVertical("box");
        GUI.backgroundColor = Color.blue;
        if (GUILayout.Button("Assing Colliders to NPC"))
        {
            colliderInstantiator.AssignToTargetNPC();
        }

        GUILayout.Space(3f);
        GUI.backgroundColor = Color.white;
        serializedObject.Update();
        EditorGUILayout.PropertyField(enumProperty); // Auto dropdown
        serializedObject.ApplyModifiedProperties();

        GUILayout.EndVertical();
        #endregion

        #region Finishing the setup
        GUILayout.Space(15f);
        GUILayout.BeginVertical("box");

        GUI.backgroundColor = Color.green;
        if(GUILayout.Button("Finish colliders setup"))
        {
            colliderInstantiator.FinishCollidersSetup();
        }

        GUILayout.EndVertical();

        #endregion
    }
}
