using UnityEngine;

public class PlayerRunState : PlayerState
{
    public PlayerRunState(NewPlayerController player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine){}

    public override void EnterState()
    {
        player.AnimationState.PlayAnimation("HeroKnight_Run");
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (Mathf.Abs(player.Movement) == 0f)
            player.StateMachine.ChangeState(player.IdleState);

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
