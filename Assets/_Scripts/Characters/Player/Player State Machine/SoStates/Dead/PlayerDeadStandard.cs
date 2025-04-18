using UnityEngine;

[CreateAssetMenu(fileName = "DeadStandard", menuName = "Player Logic/States SO/Standard/Dead")]
public class PlayerDeadStandard : PlayerDeadSOBase
{
    [SerializeField] private string _deadAnimationName;
    public override void DoAnimationTriggerEventLogic(NewPlayerController.PlayerAnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        if(_deadAnimationName != null)
            player.AnimationState.ChangeAnimationState(_deadAnimationName);
        player.soundManager.ForcePlayTrack("Dead");
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
    }

    public override void DoPhysicsUpdateLogic()
    {
        base.DoPhysicsUpdateLogic();
    }

    public override void Initialize(GameObject playerObj, NewPlayerController player)
    {
        base.Initialize(playerObj, player);
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }
}
