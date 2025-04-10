using UnityEngine;

public class PlayerAttackState : PlayerState
{
    public PlayerAttackState(NewPlayerController player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine) {}

    public override void AnimationTriggerEvent(NewPlayerController.PlayerAnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        if(player.PlayerAttackBaseInstance != null)
            player.PlayerAttackBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        if (player.PlayerAttackBaseInstance != null)
            player.PlayerAttackBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        if (player.PlayerAttackBaseInstance != null)
            player.PlayerAttackBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (player.PlayerAttackBaseInstance != null)
            player.PlayerAttackBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (player.PlayerAttackBaseInstance != null)
            player.PlayerAttackBaseInstance.DoPhysicsUpdateLogic();
    }
}
