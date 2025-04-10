using UnityEngine;

public class PlayerRunState : PlayerState
{
    public PlayerRunState(NewPlayerController player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine){}

    public override void AnimationTriggerEvent(NewPlayerController.PlayerAnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);

        if (player.PlayerRunBaseInstance != null) { }//Do run state
        else if (player.PlayerWalkBaseInstance != null)
            player.PlayerWalkBaseInstance.DoAnimationTriggerEventLogic(triggerType);

    }

    public override void EnterState()
    {
        base.EnterState();

        if(player.PlayerRunBaseInstance != null) { }//Do run state

        else if (player.PlayerWalkBaseInstance != null)
            player.PlayerWalkBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();

        if (player.PlayerRunBaseInstance != null) { }//Do run state
        else if (player.PlayerWalkBaseInstance != null)
            player.PlayerWalkBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (player.PlayerRunBaseInstance != null) { }//Do run state
        else if (player.PlayerWalkBaseInstance != null)
            player.PlayerWalkBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (player.PlayerRunBaseInstance != null) { }//Do run state
        else if (player.PlayerWalkBaseInstance != null)
            player.PlayerWalkBaseInstance.DoPhysicsUpdateLogic();
    }
}
