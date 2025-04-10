using UnityEngine;

[CreateAssetMenu(fileName = "BlockStandard", menuName = "Player Logic/States SO/Standard/Block")]
public class PlayerBlockStandard : PlayerBlockSOBase
{
    [SerializeField] private string _blockAnimationName, _blockHitEffectAnimationName, _blockHitNoEffectAnimationName;
    public override void DoAnimationTriggerEventLogic(NewPlayerController.PlayerAnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        if (_blockAnimationName != null)
            player.AnimationState.ChangeAnimationState(_blockAnimationName);
        else Debug.LogWarning("Block Animation for BlockStandardSO wasn't set");
        player.LockMovement = true;
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();

        player.IsBlocking = false;
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        if (Input.GetKeyUp(KeyCode.Space))
        {
            player.LockMovement = false;

            if (Mathf.Abs(player.Movement) == 0f)
                player.StateMachine.ChangeState(player.IdleState);

            if (Mathf.Abs(player.Movement) > 0f)
                player.StateMachine.ChangeState(player.RunState);

            if (Input.GetKeyDown(KeyCode.Z))
                player.StateMachine.ChangeState(player.AttackState);

            if (Input.GetKeyDown(KeyCode.X) && !player.SkillAttackCooldown)
                player.StateMachine.ChangeState(player.SkillAttackState);

            if (Input.GetKeyDown(KeyCode.W) && player.IsGrounded)
                player.StateMachine.ChangeState(player.JumpState);

            if (Input.GetKeyDown(KeyCode.C) && !player.IsRolling)
                player.StateMachine.ChangeState(player.RollState);

            if (player.PlayerRb.linearVelocity.y < -1.5f)
            {
                player.StateMachine.ChangeState(player.FallState);
            }
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
