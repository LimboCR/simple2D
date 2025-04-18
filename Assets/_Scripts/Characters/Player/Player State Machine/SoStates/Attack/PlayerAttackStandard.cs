using UnityEngine;

[CreateAssetMenu(fileName = "ComboAttackStandard", menuName = "Player Logic/States SO/Standard Attack (combo x2)")]
public class PlayerAttackStandard : PlayerAttackSOBase
{
    [SerializeField] private string _attack1AnimationName, _attack2AnimationName;
    [SerializeField] private float _attackDamage = 10f;
    public override void DoAnimationTriggerEventLogic(NewPlayerController.PlayerAnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
        if(triggerType == NewPlayerController.PlayerAnimationTriggerType.LightAttack)
        {
            player.Attack(_attackDamage);
        }
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        if (!player.HaveAttacked)
        {
            player.AnimationState.ChangeAnimationState(_attack1AnimationName);
            player.soundManager.ForcePlayTrack("Attack1");
            player.HaveAttacked = true;
            player.StartCoroutine(player.ComboAttackTimer());
        }
        else
        {
            player.AnimationState.ChangeAnimationState(_attack2AnimationName);
            player.soundManager.ForcePlayTrack("Attack2");
            player.HaveAttacked = false;
        }
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

            if (Input.GetKeyDown(KeyCode.X) && !player.IsHeavyAttackCooldown)
                player.StateMachine.ChangeState(player.SkillAttackState);

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

    public override void Initialize(GameObject player, NewPlayerController controller)
    {
        base.Initialize(player, controller);
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }
}
