using UnityEngine;

[CreateAssetMenu(fileName = "JumpStandard", menuName = "Player Logic/States SO/Standard/Jump")]
public class PlayerJumpStandard : PlayerJumpSOBase
{
    [SerializeField] private string _jumpAnimationName;
    public override void DoAnimationTriggerEventLogic(NewPlayerController.PlayerAnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        player.AnimationState.ChangeAnimationState(_jumpAnimationName);
        player.PlayerRb.AddForce(new Vector2(0f, player.JumpHeight), ForceMode2D.Impulse);
        player.soundManager.ForcePlayTrack("Jump");
        player.IsJumping = true;
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();

        player.IsJumping = false;
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        if (!player.LockStateChange)
        {
            if (Input.GetKeyDown(KeyCode.C) && !player.IsRolling)
                player.StateMachine.ChangeState(player.RollState);
        }
    }

    public override void DoPhysicsUpdateLogic()
    {
        base.DoPhysicsUpdateLogic();

        if (player.PlayerRb.linearVelocity.y <= 0)
        {
            player.StateMachine.ChangeState(player.FallState);
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
