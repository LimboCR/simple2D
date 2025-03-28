using UnityEngine;

public class PlayerAttackState : PlayerState
{
    public PlayerAttackState(NewPlayerController player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine) {}

    public override void EnterState()
    {
        base.EnterState();

        if (!player.HaveAttacked)
        {
            player.AnimationState.ChangeAnimationState("HeroKnight_Attack1");
            player.HaveAttacked = true;
            player.StartCoroutine(player.ComboAttackTimer());
        }
        else
        {
            player.AnimationState.ChangeAnimationState("HeroKnight_Attack2");
            player.HaveAttacked = false;
        }
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        if (player.AnimationState.IsAnimationFinished(player.AnimationState.CurrentState))
        {
            if (Mathf.Abs(player.Movement) == 0f)
                player.StateMachine.ChangeState(player.IdleState);

            if (Mathf.Abs(player.Movement) > 0f)
                player.StateMachine.ChangeState(player.RunState);

            if (Input.GetKeyDown(KeyCode.X) && !player.SkillAttackCooldown)
                playerStateMachine.ChangeState(player.SkillAttackState);

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
}
