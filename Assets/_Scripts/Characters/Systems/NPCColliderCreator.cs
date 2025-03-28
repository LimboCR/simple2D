using System;
using System.Collections.Generic;
using UnityEngine;
using static SafeInstantiation;

public class NPCColliderCreator : MonoBehaviour
{
    [Header("===Colliders settings===")]
    [Header("Layermask To Ignore")]
    [SerializeField] public LayerMask _excludeLayers;

    [Header("Single colliders presets")]
    [SerializeField] private Vector2 _attackZoneOffset;
    [SerializeField] private Vector2 _visibilityZoneOffset;
    [SerializeField] private Vector2 _visibilityZoneSize;
    [SerializeField] private float _attackRange = 1f;

    [Space]
    [Header("===Trigger Colliders Instantiator===")]
    [SerializeField] private List<NPCSquareTriggerZoneTest> _squareTriggerZones;

    [Space]
    [Header("===Gizmos===")]
    [SerializeField] private bool _chaseZoneGizmosInEditor;
    [SerializeField] private bool _chaseZoneGizmosAtRuntime;
    [SerializeField] private bool _attackZoneGizmosInEditor;
    [SerializeField] private bool _attackZoneGizmosAtRuntime;
    [SerializeField] private bool _visibilityZoneGizmosInEditor;
    [SerializeField] private bool _visibilityZoneGizmosAtRuntime;

    [Header("Collider references to transfer")]
    [SerializeField] private CircleCollider2D _attackZoneCollider;
    [SerializeField] private BoxCollider2D _visZoneCollider;
    [SerializeField] private List<Collider2D> _chaseCollidersRef;

    #region Hidden Variables

    GameObject _collidersParrent;
    [HideInInspector] public NPCType NPCTypeSelector;

    #endregion

    #region Colliders Instantiation
    // Trigger Zones (chase) colliders instantiation
    public void InstantiateChaseTriggerColliders()
    {
        if (_collidersParrent == null) InstantiateParrent();

        
        // Create Chase Zone Collider
        if (_squareTriggerZones.Count > 0)
        {
            _excludeLayers |= gameObject.layer;
            int counter = 0;
            foreach (var zones in _squareTriggerZones)
            {
                if (zones.Type == NPCTriggerColliderType.ChaseZone)
                {
                    GameObject reference = InstantiateEmpty($"TriggerCollider {counter}", new Local(), _collidersParrent.transform);
                    BoxCollider2D referenceCollider = reference.AddComponent<BoxCollider2D>();

                    referenceCollider.isTrigger = true;
                    referenceCollider.excludeLayers = _excludeLayers;
                    referenceCollider.offset = zones.ColliderOffset;
                    referenceCollider.size = zones.ColliderSize;

                    _chaseCollidersRef.Add(referenceCollider);
                    reference = null;

                    counter++;
                }
            }
        }

        
    }

    // Attack Zone Circle Collider Instantiation
    public void InstantiateAttackZoneCollider()
    {
        if (_collidersParrent == null) InstantiateParrent();

        // Create Attack Zone Collider
        Vector3 newPosition = transform.position;

        if (_attackZoneOffset.x != 0 || _attackZoneOffset.y != 0)
            newPosition = new(transform.position.x + _attackZoneOffset.x, transform.position.y + _attackZoneOffset.y);

        GameObject attakcColReference = InstantiateEmpty("AttackZoneCollider", new World(newPosition), _collidersParrent.transform);

        _attackZoneCollider = attakcColReference.AddComponent<CircleCollider2D>();
        _attackZoneCollider.excludeLayers = _excludeLayers;
        _attackZoneCollider.isTrigger = true;
        _attackZoneCollider.radius = _attackRange;
    }

    //Visibility & Mind Collider Zone
    public void InstantiateVisibilityZone()
    {
        if (_visibilityZoneSize.x != 0 && _visibilityZoneSize.y != 0)
        {
            GameObject refToCal = InstantiateEmpty("Vision Zone Collider", new Local(), _collidersParrent.transform);
            _visZoneCollider = refToCal.AddComponent<BoxCollider2D>();

            _visZoneCollider.isTrigger = true;
            _visZoneCollider.offset = _visibilityZoneOffset;
            _visZoneCollider.size = _visibilityZoneSize;
        }
    }

    private void InstantiateParrent()
    {
        // Creates Empty Parrent GameObject to store colliders;
        _collidersParrent = InstantiateEmpty("NPCCollidersParrent", new Local(), transform);
    }

    public void DeleteAllCreatedColliders()
    {
        if (_collidersParrent == null) return;

        Transform[] children = _collidersParrent.GetComponentsInChildren<Transform>();
        if(children.Length > 0)
        {
            foreach (Transform obj in children)
            {
                DestroyImmediate(obj.gameObject);
            }
        }
        Destroy(_collidersParrent);
        _collidersParrent = null;
    }

    #endregion

    #region Populate right NPC

    public void AssignToTargetNPC()
    {
        switch (NPCTypeSelector)
        {
            case NPCType.Combat:
                AssignCollidersToCombatNPC();
                break;

            case NPCType.Peacful:
                break;

            case NPCType.Trader:
                break;

            default: break;
        }
    }

    private void AssignCollidersToCombatNPC()
    {
        if (_chaseCollidersRef == null || _chaseCollidersRef.Count <= 0)
        {
            Debug.LogWarning("Error, There are no chase colliders available", gameObject);
            return;
        }
        else if (_attackZoneCollider == null)
        {
            Debug.LogWarning("Error, attack zone collider wasn't created", gameObject);
            return;
        } 

        if (gameObject.TryGetComponent<NPCCombatNav>(out NPCCombatNav combatNav))
        {
            if (combatNav.RuntimeColliders != null || combatNav.RuntimeColliders.Count > 0)
                Debug.LogWarning("Erorr, this game object already has chase colliders", gameObject);
            else combatNav.RuntimeColliders = _chaseCollidersRef;

            if (combatNav.AttackZoneCollider != null)
                Debug.LogWarning("Erorr, this game object already has attack collider", gameObject);
            else combatNav.AttackZoneCollider = _attackZoneCollider;
            
            if (_visZoneCollider != null) combatNav.VisbilityZoneCollider = _visZoneCollider;
        }
    }

    #endregion

    #region Gizmos
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying && _chaseZoneGizmosInEditor)
            DrawChaseColliders();
        if (Application.isPlaying && _chaseZoneGizmosAtRuntime)
            DrawChaseColliders();

        if (!Application.isPlaying && _attackZoneGizmosInEditor)
            DrawAttackZoneCollider();
        if (Application.isPlaying && _attackZoneGizmosAtRuntime)
            DrawAttackZoneCollider();

        if (!Application.isPlaying && _visibilityZoneGizmosInEditor)
            DrawVisibilityZoneCollider();
        if (Application.isPlaying && _visibilityZoneGizmosAtRuntime)
            DrawVisibilityZoneCollider();
    }

    private void DrawVisibilityZoneCollider()
    {
        Gizmos.color = Color.blue;
        Vector3 value1 = new Vector3(transform.position.x + _visibilityZoneOffset.x, transform.position.y + _visibilityZoneOffset.y, 0f);
        Vector3 value2 = new Vector3(_visibilityZoneSize.x, _visibilityZoneSize.y, 0f);
        Gizmos.DrawWireCube(value1, value2);
    }

    private void DrawAttackZoneCollider()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new(transform.position.x + _attackZoneOffset.x, transform.position.y + _attackZoneOffset.y), _attackRange);
    }

    private void DrawChaseColliders()
    {
        if (_squareTriggerZones.Count > 0)
        {
            foreach (var TriggerPreset in _squareTriggerZones)
            {
                Gizmos.color = Color.green;
                Vector3 value1 = new Vector3(transform.position.x + TriggerPreset.ColliderOffset.x, transform.position.y + TriggerPreset.ColliderOffset.y, 0f);
                Vector3 value2 = new Vector3(TriggerPreset.ColliderSize.x, TriggerPreset.ColliderSize.y, 0f);
                Gizmos.DrawWireCube(value1, value2);
            }
        }
    }

    #endregion

    public void FinishCollidersSetup()
    {
        DestroyImmediate(this);
    }
}

[System.Serializable]
public class NPCSquareTriggerZoneTest
{
    [Header("Collider Settings")]
    public NPCTriggerColliderType Type;
    public Vector2 ColliderSize;
    public Vector2 ColliderOffset;

    public void OnValidate()
    {
        ColliderSize = new Vector2(Mathf.Abs(ColliderSize.x), Mathf.Abs(ColliderSize.y));
    }
}

public enum NPCTriggerColliderType
{
    ChaseZone,
    AttackZone,
    VisibilityZone
}