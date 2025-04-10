using UnityEngine;

public class PlayerFallState : PlayerState
{
    public PlayerFallState(NewPlayerController player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine){}

    public override void AnimationTriggerEvent(NewPlayerController.PlayerAnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        if (player.PlayerFallBaseInstance != null)
            player.PlayerFallBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        if(player.PlayerFallBaseInstance != null) 
            player.PlayerFallBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        if (player.PlayerFallBaseInstance != null)
            player.PlayerFallBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (player.PlayerFallBaseInstance != null)
            player.PlayerFallBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (player.PlayerFallBaseInstance != null)
            player.PlayerFallBaseInstance.DoPhysicsUpdateLogic();
    }
}
