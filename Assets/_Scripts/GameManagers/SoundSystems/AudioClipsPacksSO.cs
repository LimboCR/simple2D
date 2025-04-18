using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sounds Pack", menuName ="Custom Audio/Create Sound Pack")]
public class AudioClipsPacksSO : ScriptableObject
{
    public EAudioPackType PackType;
    public List<SoundContainer> Tracks;
    public Dictionary<string, AudioClip> TracksDictionary;

    public bool IsInitialized = false;

    public void Initialize()
    {
        if (!IsInitialized)
        {
            if (Tracks != null && Tracks.Count > 0)
            {
                TracksDictionary = new();
                foreach (var track in Tracks)
                {
                    TracksDictionary.Add(track.Key, track.Sound);
                }
                IsInitialized = true;
                return;
            }
            Debug.LogWarning($"Unable to initialize the pack, no tracks were added {name}");
        }
        
    }

    public AudioClip FindTrack(string name)
    {
        return null;
    }
}

public enum EAudioPackType
{
    Soundtracks,
    SFXSoundtracks
}