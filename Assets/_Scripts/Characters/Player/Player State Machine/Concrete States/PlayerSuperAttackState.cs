using UnityEngine;

public class PlayerSuperAttackState : PlayerState
{
    public PlayerSuperAttackState(NewPlayerController player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void AnimationTriggerEvent(NewPlayerController.PlayerAnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        if(player.PlayerSkillAttackBaseInstance != null)
            player.PlayerSkillAttackBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        if (player.PlayerSkillAttackBaseInstance != null)
            player.PlayerSkillAttackBaseInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        if (player.PlayerSkillAttackBaseInstance != null)
            player.PlayerSkillAttackBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (player.PlayerSkillAttackBaseInstance != null)
            player.PlayerSkillAttackBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (player.PlayerSkillAttackBaseInstance != null)
            player.PlayerSkillAttackBaseInstance.DoPhysicsUpdateLogic();
    }
}
