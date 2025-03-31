using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(GameManager))]
public class GameManagerInspectorEditor : Editor
{
    public VisualTreeAsset VisualTree;
    private GameManager _gameManager;

    private Button _fetchAllLayers;
    private PropertyField npcListField;
    private PropertyField spawnListField;

    private void OnEnable()
    {
        _gameManager = (GameManager)target;
    }
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new();
        VisualTree.CloneTree(root);

        //Find all required buttons
        //_fetchAllLayers = root.Q<Button>("FetchAllLayersButton");
        //_fetchAllLayers.RegisterCallback<ClickEvent>(OnFetchLayersButtonClick);

        // Custom List View
        SerializedProperty npcListProp = serializedObject.FindProperty("EnemyAtScene");
        npcListField = root.Q<PropertyField>("NPCsAtScene");
        npcListField.BindProperty(npcListProp);

        SerializedProperty spawnsListProp = serializedObject.FindProperty("SpawnPoints");
        spawnListField = root.Q<PropertyField>("SpawnPointsRef");
        spawnListField.BindProperty(spawnsListProp);

        return root;
    }

    #region Button Events
    private void OnFetchLayersButtonClick(ClickEvent evt)
    {
        //_gameManager.FetchAllLayers();
    }

    #endregion
}
