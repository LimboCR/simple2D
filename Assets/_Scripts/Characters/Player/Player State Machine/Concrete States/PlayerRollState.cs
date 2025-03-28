using UnityEngine;

public class PlayerRollState : PlayerState
{
    public PlayerRollState(NewPlayerController player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine){}

    public override void EnterState()
    {
        player.AnimationState.ChangeAnimationState("HeroKnight_Roll");
        player.IsRolling = true;

        if (player.FacingRight)
            player.PlayerRb.AddForce(new Vector2(player.RollSpeed, 0f), ForceMode2D.Impulse);
        else
            player.PlayerRb.AddForce(new Vector2(-player.RollSpeed, 0f), ForceMode2D.Impulse);
    }

    public override void ExitState()
    {
        player.IsRolling = false;
    }

    public override void FrameUpdate()
    {
        if(player.AnimationState.IsAnimationFinished("HeroKnight_Roll")) player.IsRolling = false;

        if (player.IsGrounded && !player.IsRolling)
        {
            player.StateMachine.ChangeState(Mathf.Abs(player.Movement) > 0 ? player.RunState : player.IdleState);
        }
    }
}
