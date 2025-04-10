using UnityEngine;

public class PlayerRollState : PlayerState
{
    public PlayerRollState(NewPlayerController player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine){}

    public override void AnimationTriggerEvent(NewPlayerController.PlayerAnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);

        if(player.PlayerRollBaseInstance != null)
            player.PlayerRollBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();

        if (player.PlayerRollBaseInstance != null)
            player.PlayerRollBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();

        if (player.PlayerRollBaseInstance != null)
            player.PlayerRollBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (player.PlayerRollBaseInstance != null)
            player.PlayerRollBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (player.PlayerRollBaseInstance != null)
            player.PlayerRollBaseInstance.DoPhysicsUpdateLogic();
    }
}
