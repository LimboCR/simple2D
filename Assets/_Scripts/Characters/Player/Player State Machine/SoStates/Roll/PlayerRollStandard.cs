using UnityEngine;

[CreateAssetMenu(fileName = "RollStandard", menuName = "Player Logic/States SO/Standard/Roll")]
public class PlayerRollStandard : PlayerRollSOBase
{
    [SerializeField] private string _rollAnimationName;
    public override void DoAnimationTriggerEventLogic(NewPlayerController.PlayerAnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        player.AnimationState.ChangeAnimationState(_rollAnimationName);
        player.IsRolling = true;

        if (player.FacingRight)
            player.PlayerRb.AddForce(new Vector2(player.RollSpeed, 0f), ForceMode2D.Impulse);
        else
            player.PlayerRb.AddForce(new Vector2(-player.RollSpeed, 0f), ForceMode2D.Impulse);
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        player.IsRolling = false;
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        if (player.AnimationState.IsAnimationFinished(_rollAnimationName)) player.IsRolling = false;

        if (player.IsGrounded && !player.IsRolling)
        {
            player.StateMachine.ChangeState(Mathf.Abs(player.Movement) > 0 ? player.RunState : player.IdleState);
        }
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
