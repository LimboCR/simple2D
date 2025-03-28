using UnityEngine;

public class PlayerFallState : PlayerState
{
    public PlayerFallState(NewPlayerController player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine){}

    public override void EnterState()
    {
        player.AnimationState.ChangeAnimationState("HeroKnight_Fall");
        player.IsFalling = true;
    }

    public override void ExitState()
    {
        player.IsFalling = false;
    }

    public override void FrameUpdate()
    {
        if (Input.GetKeyDown(KeyCode.C) && !player.IsRolling)
            player.StateMachine.ChangeState(player.RollState);
    }

    public override void PhysicsUpdate()
    {
        if (player.IsGrounded)
        {
            playerStateMachine.ChangeState(Mathf.Abs(player.Movement) > 0 ? player.RunState : player.IdleState);
        }
    }
}
