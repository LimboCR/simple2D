using UnityEngine;

[CreateAssetMenu(fileName = "WalkStandard", menuName = "Player Logic/States SO/Standard/Walk")]
public class PlayerWalkStandard : PlayerWalkSOBase
{
    [SerializeField] private string _walkAnimationName;
    public override void DoAnimationTriggerEventLogic(NewPlayerController.PlayerAnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        player.AnimationState.ChangeAnimationState(_walkAnimationName);
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        player.soundManager.PlayRandomTrack("Run1", "Run2", "Run3", "Run4", "Run5", "Run6", "Run7", "Run8", "Run9", "Run10");

        if (Mathf.Abs(player.Movement) == 0f)
            player.StateMachine.ChangeState(player.IdleState);

        if (player.PlayerRb.linearVelocity.y < -1.5f)
        {
            player.StateMachine.ChangeState(player.FallState);
        }

        if (!player.LockStateChange)
        {
            if (Input.GetKeyDown(KeyCode.Z))
                player.StateMachine.ChangeState(player.AttackState);

            if (Input.GetKeyDown(KeyCode.X) && !player.IsHeavyAttackCooldown)
                player.StateMachine.ChangeState(player.SkillAttackState);

            if (Input.GetKeyDown(KeyCode.W) && player.IsGrounded)
                player.StateMachine.ChangeState(player.JumpState);

            if (Input.GetKeyDown(KeyCode.C) && !player.IsRolling)
                player.StateMachine.ChangeState(player.RollState);
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
