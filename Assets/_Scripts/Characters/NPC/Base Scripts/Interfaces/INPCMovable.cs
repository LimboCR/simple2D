using UnityEngine;

public interface INPCMovable
{
    float WalkSpeed { get; set; }
    float RunSpeed { get; set; }
    float JumpHeight { get; set; }
    int LookingSideIndex { get; set; }
    LayerMask GroundMask { get; set; }

    void SetInitialINPCMovable(SafeInstantiation.MovableStats? movableStats);
    void NPCMove(float moveSpeed);
    void FlipSides(int sideIndex);
    void MoveTowardsTarget(Vector2 target);
    void GroundCheck();
}
