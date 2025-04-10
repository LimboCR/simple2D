using UnityEngine;

public class PlayerBlockState : PlayerState
{
    public PlayerBlockState(NewPlayerController player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void AnimationTriggerEvent(NewPlayerController.PlayerAnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        if(player.PlayerBlockBaseInstance != null)
            player.PlayerBlockBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        if (player.PlayerBlockBaseInstance != null)
            player.PlayerBlockBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        if (player.PlayerBlockBaseInstance != null)
            player.PlayerBlockBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (player.PlayerBlockBaseInstance != null)
            player.PlayerBlockBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (player.PlayerBlockBaseInstance != null)
            player.PlayerBlockBaseInstance.DoPhysicsUpdateLogic();
    }
}
