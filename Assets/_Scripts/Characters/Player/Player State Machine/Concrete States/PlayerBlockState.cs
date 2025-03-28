using UnityEngine;

public class PlayerBlockState : PlayerState
{
    public PlayerBlockState(NewPlayerController player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        player.AnimationState.ChangeAnimationState("HeroKnight_BlockIdle");
        player.LockMovement = true;
    }

    public override void ExitState()
    {
        player.IsBlocking = false;
    }

    public override void FrameUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            player.LockMovement = false;

            if (Mathf.Abs(player.Movement) == 0f)
                player.StateMachine.ChangeState(player.IdleState);

            if (Mathf.Abs(player.Movement) > 0f)
                player.StateMachine.ChangeState(player.RunState);

            if (Input.GetKeyDown(KeyCode.Z))
                player.StateMachine.ChangeState(player.AttackState);

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
