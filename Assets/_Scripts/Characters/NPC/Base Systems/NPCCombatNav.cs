using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCCombatNav : MonoBehaviour
{
    [Header("===Colliders===")]
    [Header("Colliders Reference")]
    [SerializeField] private List<Collider2D> _runtimeCollidersRef;
    [SerializeField] private CircleCollider2D _attackZoneCollider;
    [SerializeField] private BoxCollider2D _visbilityZoneCollider;
    [SerializeField] private float _attackRange = 1f;

    [Header("Layermask To Ignore")]
    [SerializeField] private LayerMask _excludeLayers;
    public LayerMask ExcludeLayers { get { return _excludeLayers; } set { _excludeLayers = value; } }
    
    [Space]
    [Header("===Targets Entering Collider===")]
    [SerializeField] private List<Collider2D> _potentialTargets;
    [SerializeField] private List<Collider2D> _hitTargets;
    public List<Collider2D> TargetsInHitZone => _hitTargets;

    [Space]
    [Header("===Combat===")]
    [Header("Combat Settings")]
    [SerializeField] private float _targetValidationMaxTime;
    [SerializeField] private float _targetValidationMinTime;
    [SerializeField] private NPCTargetingPriorities targetingPriorities;
    [SerializeField] private bool _ignoreDistanceToTarget;
    [SerializeField] private bool _charInAttackRange;
    public bool CharacterInAttackRange => _charInAttackRange;

    [Header("Target Details")]
    [SerializeField] private GameObject _currentTarget;
    [SerializeField] private bool _ableToAttack;
    public bool AbleToAttack => _ableToAttack;
    public GameObject CurrentTarget => _currentTarget;
    private GameObject _newTarget;

    [Space]
    [Header("===Gizmos===")]
    [SerializeField] private bool _chaseZoneGizmosInEditor;
    [SerializeField] private bool _chaseZoneGizmosAtRuntime;
    [SerializeField] private bool _attackZoneGizmosInEditor;
    [SerializeField] private bool _attackZoneGizmosAtRuntime;
    [SerializeField] private bool _visibilityZoneGizmosInEditor;
    [SerializeField] private bool _visibilityZoneGizmosAtRuntime;

    #region Hidden Variables
    private Coroutine _keepTargetValidCoroutine = null;
    bool ApplicationExit;
    #endregion

    private void Update()
    {
        RemoveInvalidTargets();
        if (_currentTarget == null) _currentTarget = ChooseClosestTarget();

        if (_hitTargets.Count > 0) _charInAttackRange = true;
        else _charInAttackRange = false;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    #region Target Choise & Validation

    private void RemoveInvalidTargets()
    {
        if (_currentTarget != null && _currentTarget.CompareTag("Dead")) _currentTarget = null;

        if(_hitTargets.Count > 0)
        {
            foreach (Collider2D target in _hitTargets)
            {
                if (target.CompareTag("Dead"))
                {
                    _hitTargets.Remove(target);
                    break;
                }
                    
            }
        }

        if(_potentialTargets.Count > 0)
        {
            foreach (Collider2D target in _potentialTargets)
            {
                if (target.CompareTag("Dead"))
                {
                    _potentialTargets.Remove(target);
                    break;
                }
                    
            }
        }
    }

    private GameObject ChooseClosestTarget()
    {
        if(_hitTargets.Count > 0)
            return _hitTargets.First().gameObject;

        else if (_potentialTargets.Count > 0)
        {
            _newTarget = null;
            float closestDistance = 100f;

            foreach (Collider2D targets in _potentialTargets)
            {
                float check = Vector2.Distance(transform.position, targets.transform.position);
                if (check < closestDistance)
                {
                    closestDistance = check;
                    _newTarget = targets.gameObject;
                }
            }
            return _newTarget;
        }
        else return null;
    }

    private IEnumerator KeepTargetValid()
    {
        float validationTime = UnityEngine.Random.Range(_targetValidationMinTime, _targetValidationMaxTime);
        float elapsedTime = 0f;
        Collider2D check = _currentTarget.GetComponent<Collider2D>();

        while (elapsedTime < validationTime)
        {
            if(_potentialTargets.Contains(check) || _hitTargets.Contains(check))
            {
                _keepTargetValidCoroutine = null;
                yield break;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _currentTarget = null;
        _keepTargetValidCoroutine = null;
    }

    #endregion

    #region Collective mind and enforcment

    public void ForceToTarget(GameObject target, bool ignoreDistance = false)
    {
        _currentTarget = target;
    }

    public void ForceToUntarget()
    {
        _currentTarget = null;
    }

    public void IgnoreDistanceToTarget(bool value)
    {
        _ignoreDistanceToTarget = value;
    }

    private void TranslateTargetToNearFriendly()
    {

    }

    #endregion

    #region OnTrigger Enter/Exit
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Dead"))
        {
            if (_currentTarget != null)
            {
                if (other == _currentTarget)
                    _potentialTargets.Add(other);

                if (other.IsTouching(_attackZoneCollider) && !_hitTargets.Contains(_currentTarget.GetComponent<Collider2D>()))
                    _currentTarget = other.gameObject;
            }

            if (!_potentialTargets.Contains(other) && !_hitTargets.Contains(other))
            {
                foreach (BoxCollider2D zones in _runtimeCollidersRef)
                {
                    if (other.IsTouching(zones))
                    {
                        _potentialTargets.Add(other);
                        break;
                    }
                }
            }
            else if (other.IsTouching(_attackZoneCollider) && !_hitTargets.Contains(other))
            {
                _hitTargets.Add(other);
                _ableToAttack = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Dead"))
        {
            bool stillInChaseZone = false;
            bool stillInAttackZone = other.IsTouching(_attackZoneCollider);

            foreach (BoxCollider2D zone in _runtimeCollidersRef)
            {
                if (other.IsTouching(zone))
                {
                    stillInChaseZone = true;
                    break;
                }
            }

            // Remove from attack targets if fully exited
            if (!stillInAttackZone)
            {
                _hitTargets.Remove(other);
                if (_hitTargets.Count > 0) _currentTarget = ChooseClosestTarget();
                else if (_hitTargets.Count == 0) _ableToAttack = false;
            }

            // Try removing target from list of potential targets
            if (!stillInChaseZone)
            {
                _potentialTargets.Remove(other);
                TargetExitChaseCheck(other);
            }
        }   
    }

    private void TargetExitChaseCheck(Collider2D other)
    {
        if (_ignoreDistanceToTarget) return;

        if (other.CompareTag("Dead"))
        {
            if (other.gameObject == _currentTarget) _currentTarget = null;
        }

        switch (targetingPriorities) 
        { 
            case NPCTargetingPriorities.TryKeepChasingCurrent:
                if (_hitTargets.Count > 0)
                {
                    _currentTarget = null;
                    _currentTarget = ChooseClosestTarget();
                }
                else if (other.gameObject == _currentTarget && _keepTargetValidCoroutine == null)
                    _keepTargetValidCoroutine = StartCoroutine(KeepTargetValid());
                break;

            case NPCTargetingPriorities.ChaseClosestTarget:
                if (_hitTargets.Count > 0)
                {
                    _currentTarget = null;
                    _currentTarget = ChooseClosestTarget();
                }

                else if (other.gameObject == _currentTarget)
                {
                    if (_potentialTargets.Count > 0)
                    {
                        if (_currentTarget != null) _currentTarget = ChooseClosestTarget();
                    }
                    else if (this.gameObject.activeSelf)
                        _keepTargetValidCoroutine = StartCoroutine(KeepTargetValid());
                }
                break;

            default:
                break;
        }
    }
    #endregion

    #region Colliders assignment & Modification
    public List<Collider2D> RuntimeColliders
    {
        get { return _runtimeCollidersRef; }
        set { _runtimeCollidersRef = value; }
    }

    public CircleCollider2D AttackZoneCollider
    {
        get { return _attackZoneCollider; }
        set { _attackZoneCollider = value; }
    }
    public BoxCollider2D VisbilityZoneCollider
    {
        get { return _visbilityZoneCollider; }
        set { _visbilityZoneCollider = value; }
    }

    public void ModifyCombatLayers(LayerMask layersToExclude, LayerMask layersToInclude)
    {
        Debug.Log($"Adding Layers {layersToInclude} to {gameObject.name}");

        _attackZoneCollider.includeLayers = layersToInclude;
        _attackZoneCollider.excludeLayers = layersToExclude;

        foreach (Collider2D col in _runtimeCollidersRef)
        {
            col.includeLayers = layersToInclude;
            col.excludeLayers = layersToExclude;
        }
    }

    #endregion
}

public enum NPCTargetingPriorities
{
    ChaseClosestTarget,
    TryKeepChasingCurrent
}
