using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
public class SpawnSettingsWindow : EditorWindow
{
    //Refernece to the using script
    private static SpawnPointHandler _spawnPointReference;

    //Setting up the scrolls
    private Vector2 _scrollPosition;
    private Vector2 _descriptionScrollPosition;
    //

    //NPC Fetching & Selection
    private GameObject _selectedNPC;
    private List<GameObject> _npcPrefabs = new List<GameObject>();
    private Dictionary<GameObject, Texture2D> _npcSprites = new Dictionary<GameObject, Texture2D>();
    
    //Additional data in editor
    private bool _enableWaypoints;
    private int _setNPCTeam;
    private NPCType _npcType;

    #region New Health Settings
    private bool _healthScriptOverride;
    private bool _healthDataDropdown;

    private int _newMaxHealth;
    private int _newRegenRate;
    private float _newRegenDelay;
    #endregion

    #region New INPCMovableData
    private bool _iNPCMovableOverride;
    private bool _iMovableDropdown;

    private float _walkSpeed;
    private float _runSpeed;
    private float _jumpHeight;
    #endregion

    private bool _fetchedActualData;

    [MenuItem("Window/Spawn Settings")]
    public static void OpenWindow(SpawnPointHandler spawnPoint)
    {
        SpawnSettingsWindow window = GetWindow<SpawnSettingsWindow>("Spawn Settings");
        _spawnPointReference = spawnPoint;
        window.Show();
    }

    private void OnEnable()
    {
        LoadNPCPrefabs();
    }

    private void LoadNPCPrefabs()
    {
        _npcPrefabs.Clear();
        _npcSprites.Clear();
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/_Gameplay/Characters/NPC/" });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject npcPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            _npcPrefabs.Add(npcPrefab);

            Texture2D npcSprite = GetNPCSprite(npcPrefab);
            _npcSprites[npcPrefab] = npcSprite;
        }
    }

    private Texture2D GetNPCSprite(GameObject npcPrefab)
    {
        SpriteRenderer spriteRenderer = npcPrefab.GetComponent<SpriteRenderer>();
        Texture2D spriteTexture = AssetPreview.GetAssetPreview(spriteRenderer.sprite);
        return spriteTexture != null ? spriteTexture : null;
    }

    private void FetchSelectedNPCData()
    {
        _fetchedActualData = true;

        if(_selectedNPC.TryGetComponent<IMultiCharacterData>(out IMultiCharacterData characterData))
        {
            _setNPCTeam = characterData.CharacterTeam;
            _npcType = characterData.NPCType;

            if(_selectedNPC.TryGetComponent<IDamageble>(out IDamageble healthData))
            {
                _newMaxHealth = healthData.MaxHealth;
                _newRegenRate = healthData.RegenRate;
                _newRegenDelay = healthData.RegenDelay;
            }

            if(_selectedNPC.TryGetComponent<INPCMovable>(out INPCMovable movableData))
            {
                _walkSpeed = movableData.WalkSpeed;
                _runSpeed = movableData.RunSpeed;
                _jumpHeight = movableData.JumpHeight;
            }
        }
        _enableWaypoints = _spawnPointReference.InstantiateWaypoints;
    }

    private void OnGUI()
    {
        #region NPC Selection Scroll
        EditorGUILayout.LabelField("NPC Selection", EditorStyles.boldLabel);
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.Height(200));

        GUILayout.BeginHorizontal(GUILayout.MaxWidth(400), GUILayout.MaxHeight(400));

        foreach (GameObject npc in _npcPrefabs)
        {
            GUILayout.BeginVertical("box", GUILayout.Width(100));

            if (_npcSprites[npc] != null)
                GUILayout.Label(_npcSprites[npc], GUILayout.Width(100), GUILayout.Height(100));
            else 
                GUILayout.Label("No Preview", GUILayout.Width(100), GUILayout.Height(100));

            if (GUILayout.Button(new GUIContent(npc.name), GUILayout.ExpandWidth(true)))
            {
                _selectedNPC = npc;
                _fetchedActualData = false;
            }

            if(npc.TryGetComponent<IMultiCharacterData>(out IMultiCharacterData characterData))
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Type: ", EditorStyles.miniLabel);
                GUILayout.Space(0);
                GUILayout.Label(characterData.NPCType.ToString(), EditorStyles.miniLabel);
                GUILayout.EndHorizontal();
            }   
            else GUILayout.Label("Type: N/A", EditorStyles.miniLabel);

            GUILayout.EndVertical();
        }

        GUILayout.EndHorizontal();
        EditorGUILayout.EndScrollView();
        #endregion

        //GUILayout.FlexibleSpace();

        if (_selectedNPC != null)
        {
            if (!_fetchedActualData) FetchSelectedNPCData();
            _descriptionScrollPosition = EditorGUILayout.BeginScrollView(_descriptionScrollPosition);
            GUILayout.BeginVertical("box", GUILayout.MaxWidth(400), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            // Show NPC Texture
            GUILayout.BeginHorizontal(GUILayout.MaxHeight(100));
            if (_npcSprites[_selectedNPC] != null)
            {
                GUILayout.Label(_npcSprites[_selectedNPC], "box", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true), GUILayout.MaxWidth(100), GUILayout.MaxHeight(100));
            }
            else
            {
                GUILayout.Label("No Preview", "box", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true), GUILayout.MaxWidth(100), GUILayout.MaxHeight(100));
            }

            //
            GUILayout.BeginVertical();
            GUILayout.Space(4);
            GUILayout.TextArea("This is simple npc description for future staff ideas, presentet as it is in form as given, look forward.",
                GUILayout.MaxWidth(300), GUILayout.MaxHeight(100), GUILayout.ExpandWidth(true));
            GUILayout.EndVertical();
            ///

            GUILayout.EndHorizontal();

            GUILayout.Space(15);

            #region Details Regarding NPC
            GUILayout.BeginHorizontal("box");
            GUILayout.BeginVertical();

            _setNPCTeam = EditorGUILayout.IntSlider("Set NPC Team: ", _setNPCTeam, 0, 1);
            _enableWaypoints = EditorGUILayout.Toggle("Instantiate Waypoints: ", _enableWaypoints);

            GUILayout.Space(2);
            _healthScriptOverride = EditorGUILayout.Toggle("Tweak NPC Health ", _healthScriptOverride);

            // Shows dropdown for IDamagable override if selected
            if (_healthScriptOverride)
            {
                GUILayout.Space(5);
                _healthDataDropdown = EditorGUILayout.Foldout(_healthDataDropdown, "Health Settings");

                if (_healthDataDropdown)
                {
                    EditorGUI.indentLevel++; // Indent for better UI hierarchy
                    EditorGUILayout.IntField("MaxHealth: ", _newMaxHealth, GUILayout.MaxWidth(80), GUILayout.ExpandWidth(true));
                    EditorGUILayout.IntField("RegenRate: ", _newRegenRate, GUILayout.MaxWidth(80), GUILayout.ExpandWidth(true)); ;
                    EditorGUILayout.FloatField("RegenDelay: ", _newRegenDelay, GUILayout.MaxWidth(80), GUILayout.ExpandWidth(true));
                    EditorGUI.indentLevel--;
                }
            }

            GUILayout.Space(2);
            _iNPCMovableOverride = EditorGUILayout.Toggle("Tweak NPC Movement ", _iNPCMovableOverride);

            //Shows dropdown for INPCMovable override if selected
            if (_iNPCMovableOverride)
            {
                GUILayout.Space(5f);
                _iMovableDropdown = EditorGUILayout.Foldout(_iMovableDropdown, "Movement Settings");

                if (_iMovableDropdown)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.FloatField("WalkSpeed: ", _walkSpeed, GUILayout.MaxWidth(80), GUILayout.ExpandWidth(true));
                    EditorGUILayout.FloatField("RunSpeed: ", _runSpeed, GUILayout.MaxWidth(80), GUILayout.ExpandWidth(true));
                    EditorGUILayout.FloatField("JumpHeight: ", _jumpHeight, GUILayout.MaxWidth(80), GUILayout.ExpandWidth(true));
                    EditorGUI.indentLevel--;
                }
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            #endregion

            GUILayout.Space(20);
            EditorGUILayout.LabelField("Selected NPC: " + _selectedNPC.name, EditorStyles.boldLabel);
            if (GUILayout.Button("Set NPC to Spawn"))
            {
                _spawnPointReference.npc = _selectedNPC;
                _spawnPointReference.NPCTeam = _setNPCTeam;
                _spawnPointReference.InstantiateWaypoints = _enableWaypoints;

                if (_healthScriptOverride)
                {
                    _spawnPointReference.HealthScriptOverride = true;
                    _spawnPointReference.NPCMaxHealth = _newMaxHealth;
                    _spawnPointReference.NPCRegenRate = _newRegenRate;
                    _spawnPointReference.NPCRegenDelay = _newRegenDelay;
                }
                else _spawnPointReference.HealthScriptOverride = false;

                if (_iNPCMovableOverride)
                {
                    _spawnPointReference.INPCMovableOverride = true;
                    _spawnPointReference.WalkSpeed = _walkSpeed;
                    _spawnPointReference.RunSpeed = _runSpeed;
                    _spawnPointReference.JumpHeight = _jumpHeight;
                }
                else _spawnPointReference.INPCMovableOverride = false;
                Debug.Log("Selected NPC: " + _selectedNPC.name);
            }
        }
        else _fetchedActualData = false;
    }
}
