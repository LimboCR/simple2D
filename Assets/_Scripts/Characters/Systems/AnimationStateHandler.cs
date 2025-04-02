using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationStateHandler : MonoBehaviour
{
    private Animator _animator;
    private string _currentState;
    private string _previousState;
    private string idleAnimationState;

    public string CurrentState { get { return _currentState; } set { _currentState = value; } }
    public string PreviousState { get { return _previousState; } set { _previousState = value; } }

    public List<AnimationDictionaryEntry> AnimationsDictionary = new();
    public Dictionary<string, string> CharacterAnimations;

    void Awake()
    {
        CharacterAnimations = new Dictionary<string, string>();
        foreach (var entry in AnimationsDictionary)
        {
            if (!CharacterAnimations.ContainsKey(entry.Key))
                CharacterAnimations.Add(entry.Key, entry.AnimationName);
        }
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

[Serializable]
public class AnimationDictionaryEntry
{
    public string Key;
    public string AnimationName;
}