using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(NewPlayerController player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine) { }

    public override void AnimationTriggerEvent(NewPlayerController.PlayerAnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        //if(player.WallSlideLeft && player.FacingRight) player.ForceFlip();
        //if (player.WallSlideRight && !player.FacingRight) player.ForceFlip();

        //player.AnimationState.ChangeAnimationState("HeroKnight_WallSlide");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        //if(player.WallSlideLeft || player.WallSlideRight)
        //{
        //    if (Input.GetKeyDown(KeyCode.W))
        //        player.StateMachine.ChangeState(player.JumpState);
        //}
        //else if (!player.WallSlideLeft && !player.WallSlideRight)
        //{
        //    if (player.PlayerRb.linearVelocity.y < 0f)
        //    {
        //        player.StateMachine.ChangeState(player.FallState);
        //    }

        //    if (Mathf.Abs(player.Movement) == 0f)
        //        player.StateMachine.ChangeState(player.IdleState);

        //    if (Mathf.Abs(player.Movement) > 0f)
        //        player.StateMachine.ChangeState(player.RunState);

        //    if (Input.GetKeyDown(KeyCode.Z))
        //        player.StateMachine.ChangeState(player.AttackState);

        //    if (Input.GetKeyDown(KeyCode.X) && !player.SkillAttackCooldown)
        //        playerStateMachine.ChangeState(player.SkillAttackState);

        //    if (Input.GetKeyDown(KeyCode.W) && player.IsGrounded)
        //        player.StateMachine.ChangeState(player.JumpState);

        //    if (Input.GetKeyDown(KeyCode.C) && !player.IsRolling)
        //        player.StateMachine.ChangeState(player.RollState);

        //    if (player.PlayerRb.linearVelocity.y < -1.5f)
        //    {
        //        player.StateMachine.ChangeState(player.FallState);
        //    }
        //}
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
