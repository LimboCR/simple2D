using UnityEngine;

public class PlayerHurtState : PlayerState
{
    public PlayerHurtState(NewPlayerController player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine) { }

    public override void AnimationTriggerEvent(NewPlayerController.PlayerAnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        if(player.PlayerHurtBaseInstance != null)
            player.PlayerHurtBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        if (player.PlayerHurtBaseInstance != null)
            player.PlayerHurtBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        if (player.PlayerHurtBaseInstance != null)
            player.PlayerHurtBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (player.PlayerHurtBaseInstance != null)
            player.PlayerHurtBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (player.PlayerHurtBaseInstance != null)
            player.PlayerHurtBaseInstance.DoPhysicsUpdateLogic();
    }
}
