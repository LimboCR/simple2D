using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(NewPlayerController player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine){}

    public override void EnterState()
    {
        player.AnimationState.ChangeAnimationState("HeroKnight_Jump");
        player.PlayerRb.AddForce(new Vector2(0f, player.JumpHeight), ForceMode2D.Impulse);
        player.IsJumping = true;
    }

    public override void ExitState()
    {
        player.IsJumping = false;
    }

    public override void FrameUpdate()
    {
        if (Input.GetKeyDown(KeyCode.C) && !player.IsRolling)
            player.StateMachine.ChangeState(player.RollState);
    }

    public override void PhysicsUpdate()
    {
        if (player.PlayerRb.linearVelocity.y <= 0)
        {
            player.StateMachine.ChangeState(player.FallState);
        }
    }
}
