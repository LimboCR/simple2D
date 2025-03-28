using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCombat : MonoBehaviour
{
    [Header("AUTOMATIC! EDIT ONLY AT RUNTIME!")]
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange;
    [SerializeField] private int _damage;
    [SerializeField] private float _chaseRange;
    [SerializeField] private LayerMask _attackableLayers;
    [SerializeField] private float _weaponAttackRadius;
    [field: SerializeField] public Transform CurrentTarget { get; private set; }
    //[SerializeField] private Collider2D[] potentialTargets;
    //[SerializeField] private Collider2D[] hitTargets;

    [Space]
    [Header("[AUTO] Lossing target delay variables")]
    [SerializeField] private static float s_looseTargetTime = 3f;
    [SerializeField] private float _elapsedTime = 0f;
    [SerializeField] private bool _isLosingTarget = false;
    private Coroutine _looseTargetCoroutine;

    public float AttackRange { get { return _attackRange; } set { _attackRange = value; } }
    public int Damage { get { return _damage; } set { _damage = value; } }
    [Tooltip("Distance at which enemy will chase the player")] public float ChaseRange { get { return _chaseRange; } set { _chaseRange = value; } }
    public Transform AttackPoint { get { return _attackPoint; } set { _attackPoint = value;} }
    public LayerMask AttackableLayers { get { return _attackableLayers; } set { _attackableLayers = value; } }
    public float WeaponAttackRadius { get { return _weaponAttackRadius; } set { _weaponAttackRadius = value; } }

    public CircleCollider2D ChaseZoneCollider;
    public CircleCollider2D AttackZoneCollider;

    [Space]
    [Header("Gizmos")]
    [SerializeField] private bool _gizmosInEditor;
    [SerializeField] private bool _gizmosAtRuntime;
    [SerializeField] private bool _drawOldGizmos = false;

    private void Awake()
    {
        // Create Chase Zone Collider
        ChaseZoneCollider = gameObject.AddComponent<CircleCollider2D>();
        ChaseZoneCollider.isTrigger = true;
        ChaseZoneCollider.radius = _chaseRange;

        // Create Attack Zone Collider
        AttackZoneCollider = gameObject.AddComponent<CircleCollider2D>();
        AttackZoneCollider.isTrigger = true;
        AttackZoneCollider.radius = _attackRange;
    }



    public void CombatAttack()
    {
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(AttackPoint.position, WeaponAttackRadius, _attackableLayers);

        foreach (Collider2D target in hitTargets)
        {
            if (target.TryGetComponent(out Health targetHealth))
                targetHealth.TakeDamage(Damage);
        }
    }

    public Transform FindClosestTarget()
    {
        Collider2D[] potentialTargets = Physics2D.OverlapCircleAll(transform.position, ChaseRange, _attackableLayers);

        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D target in potentialTargets)
        {
            if (target.CompareTag("Dead")) continue;

            float distance = Vector2.Distance(transform.position, target.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target.transform;
            }
        }
        
        return closestTarget;
    }

    public bool TargetValidation(float adjust = 0f)
    {
        if (CurrentTarget == null) return false;

        if (CurrentTarget.CompareTag("Dead"))
        {
            CurrentTarget = null;
            return false;
        }

        if (Vector2.Distance(transform.position, CurrentTarget.position) > ChaseRange + adjust)
        {
            if (!_isLosingTarget && _looseTargetCoroutine == null)
            {
                _looseTargetCoroutine = StartCoroutine(LooseTargetDelay());
            }

            return !_isLosingTarget;
        }
        _isLosingTarget = false;
        return true;
    }

    //Chooses target for chasing and attacking
    public void ChooseTarget()
    {
        if (CurrentTarget != null && TargetValidation()) return;

        if (CurrentTarget != null && CurrentTarget.tag == "Dead")
            CurrentTarget = null;

        else if (CurrentTarget == null || !TargetValidation())
            CurrentTarget = FindClosestTarget();

        else return;
    }

    private IEnumerator LooseTargetDelay()
    {
        _isLosingTarget = true;
        _elapsedTime = 0f;
        //yield return new WaitForSeconds(s_looseTargetTime); // Adjust delay as needed

        while (_elapsedTime < s_looseTargetTime)
        {
            Debug.Log($"Elapsed time: {_elapsedTime}");

            if (FindClosestTarget() != null) // If an enemy is detected, stop idling
            {
                yield break;
            }

            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        _isLosingTarget = false;
        _looseTargetCoroutine = null;
        CurrentTarget = null;
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying && !_gizmosInEditor) return;

        if (Application.isPlaying && !_gizmosAtRuntime) return;

        if (_drawOldGizmos)
        {
            if (ChaseRange > 0)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, ChaseRange);
            }

            if (AttackRange > 0)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(transform.position, AttackRange);
            }
        }
    }


    #region Discontinued
    public bool d_IsTargetValid(Transform target)
    {
        if (target == null) return false;

        if (target.CompareTag("Dead"))
        {
            CurrentTarget = null;
            return false;
        }

        if (Vector2.Distance(transform.position, target.position) > ChaseRange)
        {
            if (!_isLosingTarget && _looseTargetCoroutine == null)
            {
                _looseTargetCoroutine = StartCoroutine(LooseTargetDelay());
            }

            return _isLosingTarget;
        }

        _isLosingTarget = false;
        return true;
    }

    //Checks if target is reachable
    //if not - removes target
    public bool d_IsTargetReachable(float adjust = 0f)
    {
        if (CurrentTarget == null) return false;

        else if (Vector2.Distance(transform.position, CurrentTarget.position) > ChaseRange + adjust)
        {
            CurrentTarget = null; // Stop chasing
            return false;
        }
        return Vector2.Distance(transform.position, CurrentTarget.position) <= ChaseRange;
    }
    #endregion
}
