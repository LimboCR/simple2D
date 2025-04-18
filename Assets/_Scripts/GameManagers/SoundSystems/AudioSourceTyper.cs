using UnityEngine;

[System.Serializable]
public class AudioSourceTyper
{
    public AudioSourceType SourceType;
    public AudioSource Source;
    public bool IsPlaying => Source.isPlaying;
    public float PlayingTime => Source.time;
}

public enum AudioSourceType
{
    Nature,
    Enviroment,
    OnDestruction,
    PlayerConsumable,
    GameManager,
    Music
}