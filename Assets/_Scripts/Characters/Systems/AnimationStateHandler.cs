using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationStateHandler : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _currentState;
    [SerializeField] private string _previousState;
    [SerializeField] private string idleAnimationState;

    public string CurrentState => _currentState;
    public string PreviousState => _previousState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public bool IsAnimationFinished(string animationName, float animTimeFloat = 1.0f)
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= animTimeFloat;
    }

    public void ChangeAnimationState(string animation, float crossFade = 0f)
    {
        if (_currentState != animation)
        {
            _previousState = _currentState;
            _currentState = animation;
            _animator.CrossFade(animation, crossFade);
        }
    }

    public void PlayAnimation(string animation)
    {
        if (_currentState != animation)
        {
            _previousState = _currentState;
            _currentState = animation;
            _animator.Play(animation);
        }
    }

    public void ChangeToIdle()
    {
        _previousState = _currentState;

        _currentState = idleAnimationState;
        _animator.CrossFade(idleAnimationState, 0);
    }

    public void SetTrigger(string trigger)
    {
        _animator.SetTrigger(trigger);
    }

    public void SetFloat(string name, float value)
    {
        _animator.SetFloat(name, value);
    }

    public void SetBool(string name, bool state)
    {
        _animator.SetBool(name, state);
    }
}
