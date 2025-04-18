using UnityEngine;

[CreateAssetMenu(fileName = "SkillAttackStandard", menuName = "Player Logic/States SO/Standard/Skill Attack")]
public class PlayerSkillAttackStandard : PlayerSkillAttackSOBase
{
    [SerializeField] private string _skillAttackAnimationName;
    [SerializeField] private SkillBase _skillAction;
    private SkillBase _skillInstance;
    [SerializeField] private float _heavyAttackDamage = 15f;
    public override void DoAnimationTriggerEventLogic(NewPlayerController.PlayerAnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
        if (triggerType == NewPlayerController.PlayerAnimationTriggerType.HeavyAttack) player.Attack(_heavyAttackDamage);
        if (triggerType == NewPlayerController.PlayerAnimationTriggerType.SkillAction1) ActivateSkill();
        
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        if (_skillAttackAnimationName != null)
        {
            player.AnimationState.ChangeAnimationState(_skillAttackAnimationName);
            player.soundManager.ForcePlayTrack("AttackHeavy");
        }
            
        else Debug.LogWarning($"Animation for PlayerSkillAttackStandard.so wasn't set.");

        player.StartCoroutine(player.SkillAttackCooldownTimer());
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        if (player.AnimationState.IsAnimationFinished(player.AnimationState.CurrentState))
        {
            if (Mathf.Abs(player.Movement) == 0f)
                player.StateMachine.ChangeState(player.IdleState);

            if (Mathf.Abs(player.Movement) > 0f)
                player.StateMachine.ChangeState(player.RunState);

            if (Input.GetKeyDown(KeyCode.Z))
                player.StateMachine.ChangeState(player.AttackState);

            if (Input.GetKeyDown(KeyCode.W) && player.IsGrounded)
                player.StateMachine.ChangeState(player.JumpState);

            if (Input.GetKeyDown(KeyCode.C) && !player.IsRolling)
                player.StateMachine.ChangeState(player.RollState);

            if (player.PlayerRb.linearVelocity.y < -1.5f)
            {
                player.StateMachine.ChangeState(player.FallState);
            }
        }
    }

    public override void DoPhysicsUpdateLogic()
    {
        base.DoPhysicsUpdateLogic();
    }

    public override void Initialize(GameObject playerObj, NewPlayerController player)
    {
        base.Initialize(playerObj, player);
        _skillInstance = Instantiate(_skillAction);
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }

    private void ActivateSkill()
    {
        _skillInstance.Execute(playerObj);
    }
}
