using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundsManager : MonoBehaviour
{
    private AudioSource _audioSource;

    [Header("Sounds List")]
    public List<SoundContainer> sounds = new();
    private Dictionary<string, AudioClip> _sounds;

    [Space]
    [Header("AudioSource Checks")]
    public AudioClip LastPlayedSound;
    public AudioClip NextPlayingSound;

    public bool SequenceActive = false;
    public bool GoingThroughSequence = false;
    public bool _cancelSequence = false;

    public List<AudioClip> ActiveSequence = new();
    private Coroutine _sequenceWaiter;

    public bool IsPlaying => _audioSource.isPlaying;
    public float PlayingTime => _audioSource.time;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _sounds = new();
        if (sounds != null && sounds.Count > 0)
        {
            foreach (var sound in sounds)
            {
                if (sound.Key != null)
                {
                    if (sound.Sound != null) _sounds.Add(sound.Key, sound.Sound);
                    //else Debug.LogWarning($"For key: {sound.Key} audioclip was unassigned.");
                }

            }
        }
    }

    private void Update()
    {
        if (SequenceActive)
        {
            if (!GoingThroughSequence)
            {
                GoingThroughSequence = true;
                _sequenceWaiter = StartCoroutine(LoopThroughSequence());
            }
                
        }
    }

    private IEnumerator LoopThroughSequence()
    {
        _cancelSequence = false;

        foreach (var sound in ActiveSequence)
        {
            if (_cancelSequence)
            {
                SequenceActive = false;
                _sequenceWaiter = null;
                yield break;
            }   

            _audioSource.clip = sound;
            _audioSource.Play();
            yield return new WaitUntil(() => !IsPlaying);

            if (_cancelSequence) yield break;
        }

        SequenceActive = false;
        _sequenceWaiter = null;
        yield break;
    }

    public void PlayTrack(string key)
    {
        if (!IsPlaying)
        {
            if (_sounds.Count > 0)
            {
                if (_sounds.ContainsKey(key))
                {
                    _audioSource.clip = _sounds[key];
                    _audioSource.Play();
                }
            }
        }
    }

    public void PlayRandomTrack(params string[] keys)
    {
        if (!IsPlaying)
        {
            if (_sounds.Count > 0)
            {
                int randomKey = UnityEngine.Random.Range(0, keys.Length);
                string key = keys[randomKey];

                if (_sounds.ContainsKey(key))
                {
                    _audioSource.clip = _sounds[key];
                    _audioSource.Play();
                }
            }
        }
    }

    public void ForcePlayTrack(string key)
    {
        if (_sounds.Count > 0)
        {
            if (_sounds.ContainsKey(key))
            {
                _audioSource.clip = _sounds[key];
                _audioSource.Play();
            }
        }
    }

    public void ForcePlayRandomTrack(params string[] keys)
    {
        if (_sounds.Count > 0)
        {
            int randomKey = UnityEngine.Random.Range(0, keys.Length);
            string key = keys[randomKey];

            if (_sounds.ContainsKey(key))
            {
                _audioSource.clip = _sounds[key];
                _audioSource.Play();
            }
        }
    }

    public void PlaySequence(string[] keys)
    {
        foreach(string str in keys)
        {
            if(_sounds.TryGetValue(str, out var sound))
            {
                ActiveSequence.Add(sound);
            }
        }

        SequenceActive = true;
    }

    public void BreakSequence()
    {
        if(SequenceActive || _sequenceWaiter != null)
        {
            SequenceActive = false;
             _cancelSequence = true;
        }
    }
}
