using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MultiSaveSystem;

[RequireComponent(typeof(AnimationStateHandler), typeof(StatusEffectHandler), typeof(Rigidbody2D))]
[RequireComponent (typeof(PlayerZones), typeof(AudioSource), typeof(CharacterSoundsManager))]
public class NewPlayerController : MonoBehaviour, IDamageble
{
    [HideInInspector] public CharacterSoundsManager soundManager;

    [HideInInspector] public AnimationStateHandler AnimationState;
    [HideInInspector] public StatusEffectHandler StatusEffectManager;
    [HideInInspector] public Rigidbody2D PlayerRb { get; private set; }
    [HideInInspector] public PlayerZones Zones { get; private set;}

    public ButtonBindingsSO Buttons;

    #region SFX
    [Header("Audio Staff")]
    public List<SoundContainer> sounds = new();
    #endregion

    #region Skills
    [Space, Header("Skills")]
    public SkillBase ActiveSkill1;
    public SkillBase ActiveSkill2;
    public SkillBase PassiveSkill1;
    public SkillBase PassiveSkill2;
    #endregion

    #region Coinns values
    [Space, Header("Coins and Levels")]
    public int GoldenCoins;
    public int SilverCoins;
    public int RedCoins;
    public int SkillPoints;
    public int PlayerLevel;

    #endregion

    #region Movement Stats
    [Space, Header("Movement Settings")]
    [HideInInspector] public float Movement;
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
    [field: SerializeField] public float CurrentHealth { get; set; }
    public float RegenDelay { get; set; }
    public float RegenRate { get; set; }
    public bool Alive { get; set; }
    public bool TakingDamage { get; set; }
    public int GotDamagedCounter { get; set; }

    [Range(0, 10)] public int ProbabilityToGetHurtAnimation = 10;
    #endregion    

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
    //[Space]
    //[Header("===Wall Slide===")]
    [Header("Layer Mask")]
    [HideInInspector] public LayerMask SlidableLayers;
    //[Header("Left Side Checkpoints")]
    [HideInInspector] public Transform wallCheckLU;
    [HideInInspector] public Transform wallCheckLM;
    [HideInInspector] public Transform wallCheckLD;
    [HideInInspector] RaycastHit2D wallRayLU, wallRayLM, wallRayLD;
    //[Header("Right Side Checkpoints")]
    [HideInInspector] public Transform wallCheckRU;
    [HideInInspector] public Transform wallCheckRM;
    [HideInInspector] public Transform wallCheckRD;
    [HideInInspector] RaycastHit2D wallRayRU, wallRayRM, wallRayRD;
    [HideInInspector] bool wallCheckpointsAssigned;
    [HideInInspector] Vector2 sideForLeft = Vector2.left, sideForRight = Vector2.right;
    #endregion

    #region Attack vars
    [Space]
    [Header("Attack Staff")]
    public bool HaveAttacked;
    public bool IsHeavyAttackCooldown;
    public bool IsBlocking;

    public float HeavyAttackCooldownTimer = 5f;
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
    //public PlayerWallSlideState WallSlideState { get; set; }

    #endregion

    #region SO States Varibales

    [Space]
    [Header("Player States Assigner")]
    [SerializeField] private PlayerIdleSOBase PlayerIdleBase;
    [SerializeField] private PlayerWalkSOBase PlayerWalkBase;
    [SerializeField] private PlayerRunSOBase PlayerRunBase;
    [SerializeField] private PlayerJumpSOBase PlayerJumpBase;
    [SerializeField] private PlayerFallSOBase PlayerFallBase;
    [SerializeField] private PlayerRollSOBase PlayerRollBase;
    [SerializeField] private PlayerAttackSOBase PlayerAttackBase;
    [SerializeField] private PlayerSkillAttackSOBase PlayerSkillAttackBase;
    [SerializeField] private PlayerBlockSOBase PlayerBlockBase;
    [SerializeField] private PlayerHurtSOBase PlayerHurtBase;
    [SerializeField] private PlayerDeadSOBase PlayerDeadBase;

    public PlayerIdleSOBase PlayerIdleBaseInstance { get; set; }
    public PlayerWalkSOBase PlayerWalkBaseInstance { get; set; }
    public PlayerRunSOBase PlayerRunBaseInstance { get; set; }
    public PlayerJumpSOBase PlayerJumpBaseInstance { get; set; }
    public PlayerFallSOBase PlayerFallBaseInstance { get; set; }
    public PlayerRollSOBase PlayerRollBaseInstance { get; set; }
    public PlayerAttackSOBase PlayerAttackBaseInstance { get; set; }
    public PlayerSkillAttackSOBase PlayerSkillAttackBaseInstance { get; set; }
    public PlayerBlockSOBase PlayerBlockBaseInstance { get; set; }
    public PlayerHurtSOBase PlayerHurtBaseInstance { get; set; }
    public PlayerDeadSOBase PlayerDeadBaseInstance { get; set; }

    #endregion

    #region Awake, Start, Update...
    private void Awake()
    {
        AnimationState = GetComponent<AnimationStateHandler>();
        PlayerRb = GetComponent<Rigidbody2D>();
        StatusEffectManager = GetComponent<StatusEffectHandler>();
        Zones = GetComponent<PlayerZones>();
        soundManager = GetComponent<CharacterSoundsManager>();

        StateMachineInitilizer();

        CurrentHealth = MaxHealth;
        Alive = true;
    }

    private void StateMachineInitilizer()
    {
        #region SO States Instances
        if (PlayerIdleBase != null)
        {
            PlayerIdleBaseInstance = Instantiate(PlayerIdleBase);
            PlayerIdleBaseInstance.Initialize(gameObject, this);
        }
            
        if (PlayerWalkBase != null)
        {
            PlayerWalkBaseInstance = Instantiate(PlayerWalkBase);
            PlayerWalkBaseInstance.Initialize(gameObject, this);
        }
            
        if (PlayerRunBase != null)
        {
            PlayerRunBaseInstance = Instantiate(PlayerRunBase);
            PlayerRunBaseInstance.Initialize(gameObject, this);
        }
            
        if (PlayerJumpBase != null)
        {
            PlayerJumpBaseInstance = Instantiate(PlayerJumpBase);
            PlayerJumpBaseInstance.Initialize(gameObject, this);
        }
            
        if (PlayerFallBase != null)
        {
            PlayerFallBaseInstance = Instantiate(PlayerFallBase);
            PlayerFallBaseInstance.Initialize(gameObject, this);
        }
            
        if (PlayerRollBase != null)
        {
            PlayerRollBaseInstance = Instantiate(PlayerRollBase);
            PlayerRollBaseInstance.Initialize(gameObject, this);
        }
            
        if (PlayerAttackBase != null)
        {
            PlayerAttackBaseInstance = Instantiate(PlayerAttackBase);
            PlayerAttackBaseInstance.Initialize(gameObject, this);
        }
            
        if (PlayerSkillAttackBase != null)
        {
            PlayerSkillAttackBaseInstance = Instantiate(PlayerSkillAttackBase);
            PlayerSkillAttackBaseInstance.Initialize(gameObject, this);
        }
            
        if (PlayerBlockBase != null)
        {
            PlayerBlockBaseInstance = Instantiate(PlayerBlockBase);
            PlayerBlockBaseInstance.Initialize(gameObject, this);
        }
            
        if (PlayerHurtBase != null)
        {
            PlayerHurtBaseInstance = Instantiate(PlayerHurtBase);
            PlayerHurtBaseInstance.Initialize(gameObject, this);
        }
            
        if (PlayerDeadBase != null)
        {
            PlayerDeadBaseInstance = Instantiate(PlayerDeadBase);
            PlayerDeadBaseInstance.Initialize(gameObject, this);
        }
        
        #endregion

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
        //WallSlideState = new PlayerWallSlideState(this, StateMachine);
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);

        GlobalEventsManager.SendPlayerCurrentHealth(CurrentHealth);

        GlobalEventsManager.GameStateListener.AddListener(GameStateReactor);
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

            if (Input.GetKeyDown(Buttons.Interaction))
            {
                Zones.TryInteract();
            }
            if (Input.GetKeyDown(Buttons.Quicksave))
            {
                GlobalEventsManager.BroadcastActualGameState(GameStates.QuickSave);
            }

            if (Input.GetKeyDown(Buttons.InGameMenu))
            {
                GlobalEventsManager.ShowNotification("Currently unavilable");
                if (GameManager.Instance.GetGMSounds.TryGetValue("Save", out AudioClip clip)) GlobalEventsManager.PlayGMSfx(clip);
            }
        }

        if (Input.GetKeyDown(Buttons.Quickload))
        {
            GlobalEventsManager.BroadcastActualGameState(GameStates.Restart);
        }
    }

    private void FixedUpdate()
    {
        if (Alive)
        {
            StateMachine.CurrentPlayerState.PhysicsUpdate();
            if (!LockMovement) transform.position += MoveSpeed * Time.fixedDeltaTime * new Vector3(Movement, 0.0f, 0.0f);
        }
    }

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

            //if (WallSlideLeft || WallSlideRight)
            //    StateMachine.ChangeState(WallSlideState);
        }
    }

    private void GameStateReactor(GameStates estates)
    {
        switch (estates)
        {
            case GameStates.Save:
                break;

            case GameStates.QuickSave:
                break;

            case GameStates.Restart:
                break;

            case GameStates.Load:
                break;

            case GameStates.Pause:
                break;

            case GameStates.Unpause:
                break;

            case GameStates.Start:
                break;

            case GameStates.Exit:
                break;
        }
    }
    #endregion

    #region Animation Trigger Events Logic

    public void AnimationTriggerEvent(PlayerAnimationTriggerType triggerType)
    {
        StateMachine.CurrentPlayerState.AnimationTriggerEvent(triggerType);
    }

    public enum PlayerAnimationTriggerType
    {
        ParralaxShake,
        DamageTargetsInHitZone,
        LightAttack,
        HeavyAttack,
        SkillAction1,
        SkillAction2,
        SkillAction3,
        SkillAction4
    }

    #endregion

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

    #region Give
    public void GiveCoin(ECollectable type, int amount)
    {
        switch (type)
        {
            case ECollectable.Golden:
                GoldenCoins += amount;
                GlobalEventsManager.SendCoinsChanged(type, GoldenCoins);
                break;
            case ECollectable.Silver:
                SilverCoins += amount;
                GlobalEventsManager.SendCoinsChanged(type, SilverCoins);
                break;
            case ECollectable.Red:
                RedCoins += amount;
                GlobalEventsManager.SendCoinsChanged(type, RedCoins);
                break;
            case ECollectable.SkillPoint:
                SkillPoints += amount;
                CheckLevelUp();
                //GlobalEventsManager.SendCoinsChanged(type, RedCoins);
                break;

            default: break;
        }
    }

    private void CheckLevelUp()
    {
        if (SkillPoints >= 100)
        {
            PlayerLevel += (SkillPoints - (SkillPoints % 100)) / 100;
            SkillPoints %= 100;
            GlobalEventsManager.ShowNotification("Level UP!");
        }
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

    public void TakeHealing(float amount)
    {
        if (CurrentHealth < MaxHealth)
        {
            CurrentHealth += amount;
            GlobalEventsManager.SendPlayerHeal(amount);
        }
        else if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;
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
    public void Attack(float amount)
    {
        Zones.TryAttack(amount);
    }
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
        float waitTime = HeavyAttackCooldownTimer;
        SkillCooldownElapsedTime = 0f;

        IsHeavyAttackCooldown = true;
        GlobalEventsManager.SendPlayerHeavyAttackCooldown(IsHeavyAttackCooldown, HeavyAttackCooldownTimer);

        while (SkillCooldownElapsedTime < waitTime)
        {
            SkillCooldownElapsedTime += Time.deltaTime;
            yield return null;
        }

        IsHeavyAttackCooldown = false;
        GlobalEventsManager.SendPlayerHeavyAttackCooldown(IsHeavyAttackCooldown, 0f);
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