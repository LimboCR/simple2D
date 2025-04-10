using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(NewPlayerController player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine){}

    public override void AnimationTriggerEvent(NewPlayerController.PlayerAnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        if(player.PlayerIdleBaseInstance != null)
            player.PlayerIdleBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        if(player.PlayerIdleBaseInstance != null)
            player.PlayerIdleBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        if (player.PlayerIdleBaseInstance != null)
            player.PlayerIdleBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (player.PlayerIdleBaseInstance != null)
            player.PlayerIdleBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (player.PlayerIdleBaseInstance != null)
            player.PlayerIdleBaseInstance.DoPhysicsUpdateLogic();
    }
}
