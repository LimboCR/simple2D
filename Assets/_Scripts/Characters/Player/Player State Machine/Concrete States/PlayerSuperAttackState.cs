using UnityEngine;

public class PlayerSuperAttackState : PlayerState
{
    public PlayerSuperAttackState(NewPlayerController player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        player.AnimationState.ChangeAnimationState("HeroKnight_Attack3");
        player.StartCoroutine(player.SkillAttackCooldownTimer());
    }

    public override void FrameUpdate()
    {
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
}
