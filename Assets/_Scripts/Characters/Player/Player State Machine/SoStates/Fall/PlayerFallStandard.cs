using UnityEngine;

[CreateAssetMenu(fileName = "FallStandard", menuName = "Player Logic/States SO/Standard/Fall")]
public class PlayerFallStandard : PlayerFallSOBase
{
    [SerializeField] private string _fallAnimationName;
    public override void DoAnimationTriggerEventLogic(NewPlayerController.PlayerAnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        player.AnimationState.ChangeAnimationState(_fallAnimationName);
        player.IsFalling = true;
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();

        player.IsFalling = false;
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        if (Input.GetKeyDown(KeyCode.C) && !player.IsRolling)
            player.StateMachine.ChangeState(player.RollState);
    }

    public override void DoPhysicsUpdateLogic()
    {
        base.DoPhysicsUpdateLogic();

        if (player.IsGrounded)
        {
            player.StateMachine.ChangeState(Mathf.Abs(player.Movement) > 0 ? player.RunState : player.IdleState);
        }
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
