using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TestComicsFrame : MonoBehaviour
{
    public AudioClip SoundToPlay;
    public bool TestFadeIn;
    private Image img;
    private Tween fadeTween;
    public bool FrameLogicFinished;

    public TestComicsFrame NextFrameToPlay;

    private void Awake()
    {
        img = GetComponent<Image>();
        var tempColor = img.color;
        tempColor.a = 0f;
        img.color = tempColor;
    }

    private void Update()
    {
        if (TestFadeIn)
        {
            TestFadeIn = false;
            if(SoundToPlay!=null) AudioManager.GM_SFX_Play(SoundToPlay, PlayMode.force);
            FadeIn(5f);
        }
    }

    public void FadeIn(float duration)
    {
        Fade(1f, duration, ()=> WaitForEnd());
    }

    public void Fade(float endValue, float duration, TweenCallback onEnd = null) 
    {
        if(fadeTween != null)
        {
            fadeTween.Kill(false);
        }

        fadeTween = img.DOFade(endValue, duration);
        fadeTween.onComplete += onEnd;
    }

    private void WaitForEnd()
    {
        bool playNextFrame = false;
        while (AudioManager.IsSourcePlaying(AudioSourceType.GameManager))
        {
            playNextFrame = false;
            FrameLogicFinished = false;
        }
        playNextFrame = true;

        if (playNextFrame == true)
        {
            if (NextFrameToPlay != null) InvokeNext();
            else FrameLogicFinished = true;
        }
        
            
    }

    private void InvokeNext()
    {
        FrameLogicFinished = true;
        NextFrameToPlay.TestFadeIn = true;
    }
}
