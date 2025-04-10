using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(NewPlayerController player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine){}

    public override void AnimationTriggerEvent(NewPlayerController.PlayerAnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        if(player.PlayerJumpBaseInstance != null)
            player.PlayerJumpBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        if (player.PlayerJumpBaseInstance != null)
            player.PlayerJumpBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        if (player.PlayerJumpBaseInstance != null)
            player.PlayerJumpBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (player.PlayerJumpBaseInstance != null)
            player.PlayerJumpBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base .PhysicsUpdate();
        if (player.PlayerJumpBaseInstance != null)
            player.PlayerJumpBaseInstance.DoPhysicsUpdateLogic();
    }
}
