using UnityEngine;

[RequireComponent(typeof(AnimationStateHandler))]
public class FireEffectScript : MonoBehaviour
{
    public AnimationStateHandler AnimationSH;
    public bool IsFireDepleated = false;
    public bool IsFireLoop = false;

    private void Start()
    {
        if (AnimationSH.CharacterAnimations.TryGetValue("StartFire", out string AnimationName))
            AnimationSH.ChangeAnimationState(AnimationName);
    }

    void Update()
    {
        if (AnimationSH.IsAnimationFinished(AnimationSH.CurrentState) && IsFireLoop == false && IsFireDepleated == false)
        {
            if (AnimationSH.CharacterAnimations.TryGetValue("LoopFire", out string AnimationName))
                AnimationSH.ChangeAnimationState(AnimationName);
        }

        if(IsFireDepleated == true)
        {
            if (AnimationSH.CharacterAnimations.TryGetValue("EndFire", out string AnimationName))
                AnimationSH.ChangeAnimationState(AnimationName);

            if (AnimationSH.IsAnimationFinished(AnimationSH.CurrentState))
                Destroy(gameObject);
        }
    }
}
