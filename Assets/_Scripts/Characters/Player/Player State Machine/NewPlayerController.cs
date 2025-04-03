using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewPlayerController : MonoBehaviour, IDamageble
{
    public AnimationStateHandler AnimationState;
    public StatusEffectHandler StatusEffectManager;
    public Rigidbody2D PlayerRb { get; private set; }

    #region Movement Stats
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _rollSpeed;
    [SerializeField] private bool _facingRight = true;
    public bool IsJumping;
    public bool IsRolling;
    public bool WallSlideLeft;
    public bool WallSlideRight;
    public bool IsFalling;
    public float MoveSpeed => _moveSpeed;
    public float JumpHeight =>_jumpHeight;
    public float RollSpeed => _rollSpeed;
    public bool FacingRight => _facingRight;
    #endregion

    #region IDamagable Variables
    [field: SerializeField] public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public float RegenDelay { get; set; }
    public float RegenRate { get; set; }
    public bool Alive { get; set; }
    public bool TakingDamage { get; set; }
    public int GotDamagedCounter { get; set; }

    [Range(0, 10)] public int ProbabilityToGetHurtAnimation = 10;
    #endregion

    [Space]
    [Header("Do not touch")]
    [HideInInspector] public float Movement;

    #region GroundVariables
    [Space]
    [Header("Ground check")]
    [SerializeField] public bool IsGrounded;
    [SerializeField] private LayerMask _groundMask;
    [Header("Gorund Checkpoints")]
    public Transform groundCheckPoint1;
    public Transform groundCheckPoint2;
    public Transform groundCheckPoint3;
    RaycastHit2D groundRay1, groundRay2, groundRay3;
    #endregion

    #region WallSlide Vars
    [Space]
    [Header("===Wall Slide===")]
    [Header("Layer Mask")]
    public LayerMask SlidableLayers;
    [Header("Left Side Checkpoints")]
    public Transform wallCheckLU;
    public Transform wallCheckLM;
    public Transform wallCheckLD;
    RaycastHit2D wallRayLU, wallRayLM, wallRayLD;
    [Header("Right Side Checkpoints")]
    public Transform wallCheckRU;
    public Transform wallCheckRM;
    public Transform wallCheckRD;
    RaycastHit2D wallRayRU, wallRayRM, wallRayRD;
    bool wallCheckpointsAssigned;
    Vector2 sideForLeft = Vector2.left, sideForRight = Vector2.right;
    #endregion

    #region Attack vars
    [Space]
    [Header("Attack Staff")]
    public bool HaveAttacked;
    public bool SkillAttackCooldown;
    public bool IsBlocking;

    public float SkillCooldownTimer = 5f;
    public float SkillCooldownElapsedTime = 0f;
    //[SerializeField] private float elapsedTime = 0f;
    #endregion

    #region StateMachine Variables

    [Space]
    [Header("State Lockers")]
    public bool LockMovement;
    public bool LockStateChange;
    public PlayerStateMachine StateMachine { get; set; }
    public PlayerIdleState IdleState { get; set; }
    public PlayerRunState RunState { get; set; }
    public PlayerJumpState JumpState { get; set; }
    public PlayerFallState FallState { get; set; }
    public PlayerRollState RollState { get; set; }
    public PlayerAttackState AttackState { get; set; }
    public PlayerSuperAttackState SkillAttackState { get; set; }
    public PlayerBlockState BlockState { get; set; }
    public PlayerHurtState HurtState { get; set; }
    public PlayerWallSlideState WallSlideState { get; set; }

    #endregion

    #region Awake, Start, Update...
    private void Awake()
    {
        AnimationState = GetComponent<AnimationStateHandler>();
        PlayerRb = GetComponent<Rigidbody2D>();
        StatusEffectManager = GetComponent<StatusEffectHandler>();

        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine);
        RunState = new PlayerRunState(this, StateMachine);
        JumpState = new PlayerJumpState(this, StateMachine);
        FallState = new PlayerFallState(this, StateMachine);
        RollState = new PlayerRollState(this, StateMachine);
        AttackState = new PlayerAttackState(this, StateMachine);
        SkillAttackState = new PlayerSuperAttackState(this, StateMachine);
        BlockState = new PlayerBlockState(this, StateMachine);
        HurtState = new PlayerHurtState(this, StateMachine);
        WallSlideState = new PlayerWallSlideState(this, StateMachine);

        CurrentHealth = MaxHealth;
        Alive = true;
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);

        GlobalEventsManager.SendPlayerHealthChanged(CurrentHealth);
    }

    private void Update()
    {
        if (Alive)
        {
            StateMachine.CurrentPlayerState.FrameUpdate();

            IsPlayerGrounded();

            if (IsJumping || IsFalling || !IsGrounded) CheckWallSlide();

            HandleLogic();
            FlipSidesHandler();
        }

        if (Keyboard.current.slashKey.wasPressedThisFrame)
        {
            TakeDamage(Random.Range(1f, 10f));
        }
    }

    private void FixedUpdate()
    {
        if (Alive)
        {
            StateMachine.CurrentPlayerState.PhysicsUpdate();
            if (!LockMovement) transform.position += new Vector3(Movement, 0.0f, 0.0f) * Time.fixedDeltaTime * MoveSpeed;
        }
    }
    #endregion

    private void HandleLogic()
    {
        if (Alive)
        {
            Movement = Input.GetAxis("Horizontal");

            if (_facingRight)
            {
                sideForLeft = Vector2.left;
                sideForRight = Vector2.right;
            }
            else if (!_facingRight)
            {
                sideForLeft = Vector2.right;
                sideForRight = Vector2.left;
            }

            if (Input.GetKey(KeyCode.Space) && !IsBlocking && !IsRolling && !IsJumping && IsGrounded)
            {
                IsBlocking = true;
                StateMachine.ChangeState(BlockState);
            }

            if (WallSlideLeft || WallSlideRight)
                StateMachine.ChangeState(WallSlideState);
        }
    }

    #region Movement & Checks Logic
    private void FlipSidesHandler()
    {
        if (Movement < 0f && _facingRight)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            _facingRight = false;
        }
        else if (Movement > 0f && !_facingRight)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            _facingRight = true;
        }
    }

    public void ForceFlip()
    {
        if (_facingRight)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            _facingRight = false;
        }
        else if (!_facingRight)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            _facingRight = true;
        }
    }

    private void IsPlayerGrounded()
    {
        groundRay1 = Physics2D.Raycast(groundCheckPoint1.position, Vector2.down, 0.5f, _groundMask);
        groundRay2 = Physics2D.Raycast(groundCheckPoint2.position, Vector2.down, 0.5f, _groundMask);
        groundRay3 = Physics2D.Raycast(groundCheckPoint3.position, Vector2.down, 0.5f, _groundMask);

        if (groundRay1 || groundRay2 || groundRay3) IsGrounded = true;
        else IsGrounded = false;
    }

    private void CheckWallSlide()
    {
        //wallRayLU = Physics2D.Raycast(wallCheckLU.position, Vector2.left, 0.5f, _groundMask);
        wallRayLM = Physics2D.Raycast(wallCheckLM.position, sideForLeft, 0.5f, SlidableLayers);
        wallRayLD = Physics2D.Raycast(wallCheckLD.position, sideForLeft, 0.5f, SlidableLayers);

        //wallRayRU = Physics2D.Raycast(wallCheckRU.position, Vector2.right, 0.5f, _groundMask);
        wallRayRM = Physics2D.Raycast(wallCheckRM.position, sideForRight, 0.5f, SlidableLayers);
        wallRayRD = Physics2D.Raycast(wallCheckRD.position, sideForRight, 0.5f, SlidableLayers);

        if (wallRayLM || wallRayLD) WallSlideLeft = true;
        else WallSlideLeft = false;

        if (wallRayRD || wallRayRM) WallSlideRight = true;
        else WallSlideRight = false;
    }

    #endregion

    #region Health Logic
    public void TakeDamage(float amount)
    {
        CurrentHealth -= amount;
        GlobalEventsManager.SendPlayerTookDamage(amount);

        if (Random.Range(0, 10) > ProbabilityToGetHurtAnimation)
        {
            StateMachine.ChangeState(HurtState);
        }
        if (CurrentHealth <= 0)
        {
            Alive = false;
            Die();
        }
    }

    public void Die()
    {
        AnimationState.ChangeAnimationState("HeroKnight_Death");
    }

    public void SetInitialIDamageble(SafeInstantiation.HealthStats? healthStats)
    {

    }
    #endregion

    #region Combat Logic
    public IEnumerator ComboAttackTimer()
    {
        float waitTime = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < waitTime)
        {
            if(!HaveAttacked) yield break;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        HaveAttacked = false;
    }

    public IEnumerator SkillAttackCooldownTimer()
    {
        float waitTime = SkillCooldownTimer;
        SkillCooldownElapsedTime = 0f;

        SkillAttackCooldown = true;
        GlobalEventsManager.SendPlayerSkillCooldownStatus(SkillAttackCooldown);

        while (SkillCooldownElapsedTime < waitTime)
        {
            SkillCooldownElapsedTime += Time.deltaTime;
            yield return null;
        }

        SkillAttackCooldown = false;
        GlobalEventsManager.SendPlayerSkillCooldownStatus(SkillAttackCooldown);
    }
    #endregion

    #region Save, Load, Reset

    public void Save(ref PlayerSaveData data)
    {
        data.Position = transform.position;
    }

    public void Load(PlayerSaveData data)
    {
        transform.position = data.Position;
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        #region Ground Ray Check

        if (groundCheckPoint1 != null && groundCheckPoint2 != null && groundCheckPoint3 != null)
        {
            if (groundRay1) Gizmos.color = Color.green;
            else Gizmos.color = Color.red;
            Gizmos.DrawRay(groundCheckPoint1.position, Vector2.down * 0.5f);

            if (groundRay2) Gizmos.color = Color.green;
            else Gizmos.color = Color.red;
            Gizmos.DrawRay(groundCheckPoint2.position, Vector2.down * 0.5f);

            if (groundRay3) Gizmos.color = Color.green;
            else Gizmos.color = Color.red;
            Gizmos.DrawRay(groundCheckPoint3.position, Vector2.down * 0.5f);
        }
        #endregion

        #region Wall Slide Ray Check

        if(wallCheckLM != null && wallCheckLD != null &&
            wallCheckRM != null && wallCheckRD != null)
        {
            if (wallRayLM) Gizmos.color = Color.green;
            else Gizmos.color = Color.red;
            Gizmos.DrawRay(wallCheckLM.position, sideForLeft * 0.5f);

            if (wallRayLD) Gizmos.color = Color.green;
            else Gizmos.color = Color.red;
            Gizmos.DrawRay(wallCheckLD.position, sideForLeft * 0.5f);

            if (wallRayRM) Gizmos.color = Color.green;
            else Gizmos.color = Color.red;
            Gizmos.DrawRay(wallCheckRM.position, sideForRight * 0.5f);

            if (wallRayRD) Gizmos.color = Color.green;
            else Gizmos.color = Color.red;
            Gizmos.DrawRay(wallCheckRD.position, sideForRight * 0.5f);
        }
        #endregion
    }
}

[System.Serializable]
public struct PlayerSaveData
{
    public Vector3 Position;
}