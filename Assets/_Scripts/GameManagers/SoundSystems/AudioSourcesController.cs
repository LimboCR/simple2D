using System.Collections.Generic;
using UnityEngine;

public class AudioSourcesController : MonoBehaviour
{
    [Header("Audio Sources")]
    public List<AudioSourceTyper> SourcesList;
    public Dictionary<AudioSourceType, AudioSourceTyper> Sources;

    private void Awake()
    {
        Sources = new();
        if (SourcesList != null && SourcesList.Count > 0)
        {
            foreach(var source in SourcesList)
            {
                if(source.Source != null)
                    Sources.Add(source.SourceType, source);
            }
        }
    }

    public void PlayTrack(AudioClip clip, AudioSourceType type, bool loop)
    {
        if(clip != null)
        {
            if (Sources.TryGetValue(type, out var source))
            {
                if (!source.IsPlaying())
                {
                    source.Source.clip = clip;
                    source.Source.loop = loop;
                    source.Source.Play();
                }
            }
        }
    }

    public void PlayRandomTrack(AudioSourceType type, bool loop, params AudioClip[] clips)
    {
        if (Sources.TryGetValue(type, out var source))
        {
            if (!source.IsPlaying())
            {
                int randomKey = Random.Range(0, clips.Length);

                source.Source.clip = clips[randomKey];
                source.Source.loop = loop;
                source.Source.Play();
            }
        }
    }

    public void ForcePlayTrack(AudioClip clip, AudioSourceType type, bool loop)
    {
        if (clip != null)
        {
            if (Sources.TryGetValue(type, out var source))
            {
                source.Source.clip = clip;
                source.Source.loop = loop;
                source.Source.Play();
            }
        }
    }

    public void ForcePlayRandomTrack(AudioSourceType type, bool loop, params AudioClip[] clips)
    {
        if (Sources.TryGetValue(type, out var source))
        {
            int randomKey = Random.Range(0, clips.Length);

            source.Source.clip = clips[randomKey];
            source.Source.loop = loop;
            source.Source.Play();
        }
    }

    public void ForceStopPlaying(AudioSourceType type)
    {
        if (Sources.TryGetValue(type, out var source)) source.Source.Stop();
    }

    public bool IsPlaying(AudioSourceType type)
    {
        if (Sources.TryGetValue(type, out var source))
        {
            return source.IsPlaying();
        }

        Debug.LogError($"No AudioSource of type {type} exists in AudioSourceController");
        return false;
    }
}
