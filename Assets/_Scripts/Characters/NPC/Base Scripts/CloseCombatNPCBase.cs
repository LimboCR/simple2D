using System.Collections;
using UnityEngine;
using static SafeInstantiation;
using static GlobalEventsManager;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D), typeof(AnimationStateHandler), typeof(NPCCombatNav))]
public abstract class CloseCombatNPCBase : NPCBase, IDamageble, INPCMovable
{
    #region Required Scripts
    [Header("Required")]
    private Rigidbody2D _npcRB;
    public NPCCombatNav combatNav;
    public AnimationStateHandler AnimationState;

    #endregion

    #region MultiCharacterData
    //[Header("Character Data")]
    //[field: SerializeField] public string CharacterName { get; set; }
    //[field: SerializeField] public NPCType NPCType { get; set; }
    //[field: SerializeField] public int CharacterTeam { get; set; }
    #endregion

    #region NPC IDamagable variables
    // Health
    [field: SerializeField] public float MaxHealth { get; set; } = 100;
    [field: SerializeField] public float CurrentHealth { get; set; }
    [field: SerializeField] public float RegenDelay { get; set; } = 3f;
    [field: SerializeField] public float RegenRate { get; set; } = 1;
    public bool Alive { get; set; }
    public bool TakingDamage { get; set; }
    public int GotDamagedCounter { get; set; }
    #endregion

    #region Droppable Loot
    [Space]
    [Header("Droppable Loot")]
    [SerializeField] protected List<GameObject> _droppableLoot = new();
    [SerializeField] protected float _dropForce = 5f;
    [SerializeField] protected bool _isLootDropped = false;

    #endregion

    #region NPC INPCMovable variables
    // Walking stats
    [field: SerializeField] public float WalkSpeed { get; set; } = 2f;
    [field: SerializeField] public float RunSpeed { get; set; } = 4f;
    [field: SerializeField] public float JumpHeight { get; set; } = 12f;
    [field: SerializeField, Tooltip("Layers at which NPC will be considered as grounded")] public LayerMask GroundMask { get; set; }
    [field: SerializeField, Tooltip("Used to verify NPC looking direction and flip it if neccessary")] public bool LookingRight { get; set; }
    [field: SerializeField, Tooltip("Verifies NPC initial looking side")] public bool IsInitiallyLookingRight { get; set; }
    #endregion

    #region Verification variables
    [Space]
    [Header("Verification Variables")]
    [Tooltip("Does not effect anything, just for visual response")] public NPCStateCheck ActiveState;
    [Tooltip("Automatic")] public bool IsJumping = false;
    [Tooltip("Automatic")] public bool IsAgrresive = false;
    [Tooltip("Automatic")] public bool IsHitObstacle = false;

    [Header("Movement CheckPoints")]
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Transform _jumpCheckPoint;
    [SerializeField] private Transform _obstaclePoint;

    Coroutine aggresiveTimerCoroutine;
    RaycastHit2D hitWalkCheck, hitJumpCheck, obstacleCheck;
    [Header("Gizmos")]
    [SerializeField] private bool DrawGroundCheckGizmos;

    #endregion

    #region Waypoints & Navigation variables

    [Space]
    [Header("Waypoints")]
    public GameObject WaypointL;
    public GameObject WaypointR;
    public GameObject NPCBasePoint;
    public GameObject PreviousWaypoint;
    public GameObject ReturningWaypoint;

    [Header("Waypoints verifications")]
    public bool WaypointsAssigned;
    public bool IdlingAtWaypoint;
    public float DistanceToWL;
    public float DistanceToWR;

    [Header("Waypoints verifications")]
    public bool IgnoreWaypoints;

    #endregion

    #region State Machine Variables
    public bool StateLocker;
    public NPCStateMachine StateMachine { get; set; }
    public NPCIdleState IdleState { get; set; }
    public NPCWalkState WalkState { get; set; }
    public NPCRunState RunState { get; set; }
    public NPCChaseState ChaseState { get; set; }
    public NPCReturnState ReturnState { get; set; }
    public NPCAttackState AttackState { get; set; }
    public NPCHurtState HurtState { get; set; }
    public NPCDeadState DeadState { get; set; }

    #endregion

    #region SO States Varibales

    [Space]
    [Header("NPC States Assigner")]
    [SerializeField] protected NPCIdleSOBase NPCIdleBase;
    [SerializeField] protected NPCWalkSOBase NPCWalkBase;
    [SerializeField] protected NPCRunSOBase NPCRunBase;
    [SerializeField] protected NPCChaseSOBase NPCChaseBase;
    [SerializeField] protected NPCReturnSOBase NPCReturnBase;
    [SerializeField] protected NPCAttackSOBase NPCAttackBase;
    [SerializeField] protected NPCHurtSOBase NPCHurtBase;
    [SerializeField] protected NPCDeadSOBase NPCDeadBase;

    public NPCIdleSOBase NPCIdleBaseInstance { get; set; }
    public NPCWalkSOBase NPCWalkBaseInstance { get; set; }
    public NPCRunSOBase NPCRunBaseInstance { get; set; }
    public NPCChaseSOBase NPCChaseBaseInstance { get; set; }
    public NPCReturnSOBase NPCReturnBaseInstance { get; set; }
    public NPCAttackSOBase NPCAttackBaseInstance { get; set; }
    public NPCHurtSOBase NPCHurtBaseInstance { get; set; }
    public NPCDeadSOBase NPCDeadBaseInstance { get; set; }

    #endregion

    #region Data override on instantiation
    public virtual void SetInitialIDamageble(HealthStats? healthStats)
    {
        if (healthStats.HasValue)
        {
            if (healthStats.Value.MaxHealth.HasValue) MaxHealth = healthStats.Value.MaxHealth.Value;
            if (healthStats.Value.RegenRate.HasValue) RegenRate = healthStats.Value.RegenRate.Value;
            if (healthStats.Value.RegenDelay.HasValue) RegenDelay = healthStats.Value.RegenDelay.Value;
        }
    }

    public virtual void SetInitialINPCMovable(MovableStats? movableStats)
    {
        if (movableStats.HasValue)
        {
            if (movableStats.Value.WalkSpeed.HasValue) WalkSpeed = movableStats.Value.WalkSpeed.Value;
            if (movableStats.Value.RunSpeed.HasValue) RunSpeed = movableStats.Value.RunSpeed.Value;
            if (movableStats.Value.JumpHeight.HasValue) JumpHeight = movableStats.Value.JumpHeight.Value;
            //if (movableStats.Value.LookingSideIndex.HasValue) LookingSideIndex = movableStats.Value.LookingSideIndex.Value;
        }
    }

    public override void SetInitialMultiCharData(CharacterData? characterData)
    {
        if (characterData.HasValue)
        {
            if (characterData.Value.NPCTeam.HasValue) CharacterTeam = characterData.Value.NPCTeam.Value;
        }
    }
    #endregion

    #region Awake, Start, Update, FixedUpdate and Required Functions
    private void Awake()
    {
        SendEnemySpawn(gameObject);
    }

    private void OnDestroy()
    {
        SendEnemyRemove(gameObject);
    }

    protected virtual void Start()
    {
        AnimationState = GetComponent<AnimationStateHandler>();
        _npcRB = GetComponent<Rigidbody2D>();
        combatNav = GetComponent<NPCCombatNav>();

        NPCTeamSetup();
        StateMachineInitializer();

        CurrentHealth = MaxHealth;
        Alive = true;

        if (WaypointL != null || WaypointR != null)
            WaypointsAssigned = true;
    }

    protected virtual void NPCTeamSetup()
    {
        if (CharacterTeam == 0)
        {
            this.gameObject.tag = "Friendly";
            this.gameObject.layer = LayerMask.NameToLayer("Friendly");
            combatNav.ModifyCombatLayers(LayerMask.GetMask("Friendly", "Player", "Ground", "WaypointsAndNav", "Default", "Platform", "Items", "Destructable"), LayerMask.NameToLayer("Enemy"));
        }
        else if (CharacterTeam == 1)
        {
            this.gameObject.tag = "Enemy";
            this.gameObject.layer = LayerMask.NameToLayer("Enemy");
            combatNav.ModifyCombatLayers(LayerMask.GetMask("Enemy", "Ground", "WaypointsAndNav", "Default", "Platform", "Items", "Destructable"), LayerMask.GetMask("Friendly", "Player"));
        }
    }

    protected virtual void StateMachineInitializer()
    {
        NPCIdleBaseInstance = Instantiate(NPCIdleBase);
        NPCWalkBaseInstance = Instantiate(NPCWalkBase);
        NPCRunBaseInstance = Instantiate(NPCRunBase);
        NPCChaseBaseInstance = Instantiate(NPCChaseBase);
        NPCReturnBaseInstance = Instantiate(NPCReturnBase);
        NPCAttackBaseInstance = Instantiate(NPCAttackBase);
        NPCHurtBaseInstance = Instantiate(NPCHurtBase);
        NPCDeadBaseInstance = Instantiate(NPCDeadBase);

        StateMachine = new NPCStateMachine();

        IdleState = new NPCIdleState(this, StateMachine);
        WalkState = new NPCWalkState(this, StateMachine);
        RunState = new NPCRunState(this, StateMachine);
        ChaseState = new NPCChaseState(this, StateMachine);
        AttackState = new NPCAttackState(this, StateMachine);
        DeadState = new NPCDeadState(this, StateMachine);
        ReturnState = new NPCReturnState(this, StateMachine);
        HurtState = new NPCHurtState(this, StateMachine);

        NPCIdleBaseInstance.Initialize(gameObject, this);
        NPCWalkBaseInstance.Initialize(gameObject, this);
        NPCRunBaseInstance.Initialize(gameObject, this);
        NPCChaseBaseInstance.Initialize(gameObject, this);
        NPCReturnBaseInstance.Initialize(gameObject, this);
        NPCAttackBaseInstance.Initialize(gameObject, this);
        NPCHurtBaseInstance.Initialize(gameObject, this);
        NPCDeadBaseInstance.Initialize(gameObject, this);

        StateMachine.Initialize(IdleState);
    }

    protected virtual void Update()
    {
        StateMachine.CurrentNPCState.FrameUpdate();
        NPCUpdateStateLogics();
    }

    protected virtual void FixedUpdate()
    {
        StateMachine.CurrentNPCState.PhysicsUpdate();
    }

    #endregion

    #region States Logic
    protected virtual void NPCUpdateStateLogics()
    {
        if (!Alive)
        {
            if (CurrentHealth <= 0 && ActiveState != NPCStateCheck.Dead)
                Die();
        }

        else if (!StateLocker && Alive)
        {
            AwaitHurtState();

            if (gameObject.activeSelf && IsAgrresive && aggresiveTimerCoroutine == null)
                aggresiveTimerCoroutine = StartCoroutine(AggresiveTimer());
        }
    }

    protected virtual void AwaitHurtState()
    {
        if (TakingDamage && GotDamagedCounter != Random.Range(0, 4))
            StateMachine.ChangeState(HurtState);
    }

    #endregion

    #region Health / Die Functions
    public virtual void TakeDamage(float amount) // ++++
    {
        if (!Alive) return;
        TakingDamage = true;
        GotDamagedCounter += 1;
        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
        {
            Alive = false;
            if (ActiveState != NPCStateCheck.Dead) Die();
        }        
    }

    public void TakeHealing(float amount)
    {
        CurrentHealth += amount;
    }

    public virtual void Die() // ++++
    {
        StateMachine.ChangeState(DeadState);
        Debug.Log($"{gameObject.name} died horrible death. Pray!");

        gameObject.name = $"Dead {gameObject.name}";
        gameObject.tag = "Dead";

        if(_isLootDropped == false) DropLootOnDeath();

        Destroy(gameObject, 15f); // Cleanup
    }

    protected virtual void DropLootOnDeath()
    {
        if(_droppableLoot.Count > 0)
        {
            foreach(GameObject loot in _droppableLoot)
            {
                Vector3 spawnPos = transform.position + new Vector3(0, 0.4f, 0);
                GameObject drop = Instantiate(loot, spawnPos, Quaternion.identity);

                Vector2 randomDir = Random.insideUnitCircle.normalized + Vector2.up * 0.5f;
                randomDir.Normalize();

                if (drop.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                    rb.AddForce(randomDir * _dropForce, ForceMode2D.Impulse);
            }
            _isLootDropped = true;
        }
    }

    #endregion

    #region NPC Movement Functions & Verifications

    public virtual void NPCMove(float moveSpeed) // ++++
    {
        if (!IsInitiallyLookingRight)
        {
            if (transform.eulerAngles.y >= 0)
                transform.Translate(Vector2.left * Time.deltaTime * moveSpeed);
            else
                transform.Translate(Vector2.right * Time.deltaTime * moveSpeed);
        }
    }

    public virtual void FlipSides(bool lookingRight) // ++++
    {
        if (IsInitiallyLookingRight)
        {
            if (LookingRight)
            {
                LookingRight = false;
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
            else
            {
                LookingRight = true;
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
        }

        if (!IsInitiallyLookingRight)
        {
            if (!LookingRight)
            {
                LookingRight = true;
                transform.eulerAngles = new Vector3(0f, 180f, 0f);
            }
            else
            {
                LookingRight = false;
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
        }
    }

    public virtual void MoveTowardsTarget(Vector2 target) // ++++
    {
        transform.position = Vector2.MoveTowards(transform.position, target, RunSpeed * Time.deltaTime);
    }

    public virtual void GroundCheck() // ++++
    {
        hitWalkCheck = Physics2D.Raycast(_groundCheckPoint.position, Vector2.down, 1, GroundMask);
        hitJumpCheck = Physics2D.Raycast(_jumpCheckPoint.position, Vector2.down, 1, GroundMask);

        if (!hitWalkCheck) {
            if (IsJumping == false) {
                if (hitJumpCheck) NPCJump();
                else FlipSides(LookingRight);
            }     
        } if (hitWalkCheck && IsJumping) IsJumping = false;
    }

    public virtual bool ObstacleCheck()
    {
        if (_obstaclePoint != null)
        {
            return obstacleCheck = Physics2D.Raycast(_obstaclePoint.position, LookingRight ? Vector2.right : Vector2.left, 0.25f, GroundMask);
        }
        else return false;
    }
    protected virtual void NPCJump() // ++++
    {
        IsJumping = true;
        _npcRB.AddForce(new Vector2(0f, JumpHeight), ForceMode2D.Impulse);
    }

    public float CheckDistance(Transform target)
    {
        return Vector2.Distance(transform.position, target.position);
    }

    public virtual GameObject NeedToReturn() // ++++
    {
        if (WaypointsAssigned)
        {
            DistanceToWL = CheckDistance(WaypointL.transform);
            DistanceToWR = CheckDistance(WaypointR.transform);

            if (WaypointL.transform.position.x > transform.position.x && WaypointR.transform.position.x > transform.position.x)
            {
                if (LookingRight) FlipSides(LookingRight);
                PreviousWaypoint = WaypointR;
                return WaypointL;
            }

            else if (WaypointL.transform.position.x < transform.position.x && WaypointR.transform.position.x < transform.position.x)
            {
                if (!LookingRight) FlipSides(LookingRight);
                PreviousWaypoint = WaypointL;
                return WaypointR;
            }
            return null;
        }
        else return null;
    }

    public virtual void OnTriggerEnter2D(Collider2D collision) // ++++
    {
        if (WaypointsAssigned && !IgnoreWaypoints)
        {
            if (ReturningWaypoint != null)
            {
                if (collision.gameObject == ReturningWaypoint)
                {
                    PreviousWaypoint = collision.gameObject;
                    StateMachine.ChangeState(IdleState);
                }
            }
            else if (collision.gameObject == WaypointL || collision.gameObject == WaypointR)
            {
                if (PreviousWaypoint != collision.gameObject || PreviousWaypoint == null)
                {
                    PreviousWaypoint = collision.gameObject;
                    IdlingAtWaypoint = true;
                    StateMachine.ChangeState(IdleState);
                }
            }
        }
    }

    #endregion

    #region Combat Related Staff
    protected virtual IEnumerator AggresiveTimer() // ++++
    {
        float cautiosTime = Random.Range(15f, 30f);
        float elapsedTime = 0f;

        while (elapsedTime < cautiosTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        IsAgrresive = false;
        aggresiveTimerCoroutine = null;
    }

    #endregion

    #region Animation Triggers

    protected virtual void AnimationTriggerrEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentNPCState.AnimationTriggerEvent(triggerType);
    }

    public enum AnimationTriggerType
    {
        ParralaxShake,
        DamageTargetsInHitZone
    }

    #endregion

    public enum NPCStateCheck
    {
        Idle = 0,
        Walk = 1,
        Run = 2,
        Chase = 3,
        Attack = 4,
        Hurt = 5,
        Dead = 6,
        ReturningToBase = 7,
        Patrolling = 8,
        Pause = 9,
        CrossState = 10
    }

    #region Gizmos Logic
    protected virtual void OnDrawGizmosSelected()
    {
        if (DrawGroundCheckGizmos && ActiveState != NPCStateCheck.Dead)
        {
            if (_groundCheckPoint != null)
            {
                if (hitWalkCheck) Gizmos.color = Color.green;
                else Gizmos.color = Color.red;
                Gizmos.DrawRay(_groundCheckPoint.position, Vector2.down * 1);
            }
            if (_jumpCheckPoint != null)
            {
                if (hitJumpCheck) Gizmos.color = Color.green;
                else Gizmos.color = Color.red;
                Gizmos.DrawRay(_jumpCheckPoint.position, Vector2.down * 1);
            }
            if(_obstaclePoint != null)
            {
                if (obstacleCheck) Gizmos.color = Color.green;
                else Gizmos.color = Color.red;
                Gizmos.DrawRay(_obstaclePoint.position, LookingRight ? Vector2.right * 0.25f : Vector2.left * 0.25f);
            }
        }
    }

    
    #endregion
}
