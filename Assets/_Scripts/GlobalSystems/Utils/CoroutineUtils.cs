using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public static class CoroutineUtils
{
    #region Whatcher Coroutine
    public static Coroutine StartAndTrack(IEnumerator routine, Action onComplete)
    {
        return MonoWrapper.Instance.StartCoroutine(Wrapper());

        IEnumerator Wrapper()
        {
            yield return MonoWrapper.Instance.StartCoroutine(routine);
            onComplete?.Invoke();
        }
    }
    #endregion

    #region Standard Coroutines
    /// <summary>
    /// Waits a duration, then runs an action. Standard Time Based Actions
    /// </summary>
    /// <param name="delay">Amount of time to wait</param>
    /// <param name="onComplete">What to do when timer runs out</param>
    /// <returns></returns>
    public static IEnumerator WaitThenDo(float delay, Action onComplete)
    {
        yield return new WaitForSeconds(delay);
        onComplete?.Invoke();
    }

    /// <summary>
    /// Timer that tracks elapsed time, calls optional onTick and onComplete
    /// </summary>
    /// <param name="duration">Timer duration</param>
    /// <param name="onTick">Action that happens every tick (optional)</param>
    /// <param name="onComplete">Action to do when timer runs out (optional)</param>
    /// <param name="breakCondition">Condition to break and leave coroutine (optional)</param>
    /// <returns></returns>
    public static IEnumerator Timer(
        float duration,
        Action<float> onTick = null,
        Action onComplete = null,
        Func<bool> breakCondition = null)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (breakCondition != null && breakCondition())
                yield break;

            elapsed += Time.deltaTime;
            onTick?.Invoke(elapsed);
            yield return null;
        }

        onComplete?.Invoke();
    }

    /// <summary>
    /// Repeat action every X seconds for duration
    /// </summary>
    /// <param name="duration">Timer duration</param>
    /// <param name="interval">Every X seconds do onRepeat</param>
    /// <param name="onRepeat">Action to do every X seconds</param>
    /// <param name="onComplete">Action to do when timer runs out (optional)</param>
    /// <param name="breakCondition">Condition to break and leave coroutine (optional)</param>
    /// <returns></returns>
    public static IEnumerator RepeatAction(
        float duration,
        float interval,
        Action onRepeat,
        Action onComplete = null,
        Func<bool> breakCondition = null)
    {
        float elapsed = 0f;
        float nextTick = 0f;

        while (elapsed < duration)
        {
            if (breakCondition != null && breakCondition())
                yield break;

            if (elapsed >= nextTick)
            {
                onRepeat?.Invoke();
                nextTick += interval;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        onComplete?.Invoke();
    }

    /// <summary>
    /// Loops until breakCondition returns true
    /// </summary>
    /// <param name="breakCondition">Condition to break and leave coroutine</param>
    /// <param name="onComplete">Action to do when timer runs out (optional)</param>
    /// <returns></returns>
    public static IEnumerator WaitUntil(Func<bool> breakCondition, Action onComplete = null)
    {
        yield return new WaitUntil(breakCondition);
        onComplete?.Invoke();
    }

    /// <summary>
    /// Loop forever until stopped manually. As well requires to be stopped manualy if leaving Runtime.
    /// </summary>
    /// <param name="loopAction">Action to do while looping</param>
    /// <param name="interval">Delay between repeatedly executing loopAction</param>
    /// <returns></returns>
    public static IEnumerator ForeverLoop(Action loopAction, float interval)
    {
        while (true)
        {
            loopAction?.Invoke();
            yield return new WaitForSeconds(interval);
        }
    }

    #endregion

    #region Async Coroutines

    /// <summary>
    /// Async Delay timer that loops. Optionaly CancellationToken can be added.
    /// </summary>
    /// <param name="seconds">Time to wait</param>
    /// <param name="token">Cancellation token</param>
    /// <returns></returns>
    public static async Task AsyncDelay(float seconds, CancellationToken token = default)
    {
        int milliseconds = Mathf.RoundToInt(seconds * 1000f);
        try
        {
            await Task.Delay(milliseconds, token);
        }
        catch (TaskCanceledException) { }
    }

    /// <summary>
    /// Async Timer with with optional: Action on Tick/Complete, Tick Rate, Break condition, Cancelation token.
    /// </summary>
    /// <param name="duration">Timer duration</param>
    /// <param name="onTick">Action to execute on tick(optional)</param>
    /// <param name="onComplete">Action to execute when completed (optional)</param>
    /// <param name="tickRate">Tick rate (optional, default = 0.1f)</param>
    /// <param name="breakCondition">Condition at which Timer breaks (optional)</param>
    /// <param name="token">CancellationToken</param>
    /// <returns></returns>
    public static async Task AsyncTimer(
        float duration,
        Action<float> onTick = null,
        Action onComplete = null,
        float tickRate = 0.1f,
        Func<bool> breakCondition = null,
        CancellationToken token = default)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (breakCondition != null && breakCondition())
                return;

            onTick?.Invoke(elapsed);
            await Task.Delay(Mathf.RoundToInt(tickRate * 1000f), token);

            elapsed += tickRate;
        }

        onComplete?.Invoke();
    }

    #endregion
}

public class MonoWrapper : MonoBehaviour
{
    public static MonoWrapper Instance = new();
}