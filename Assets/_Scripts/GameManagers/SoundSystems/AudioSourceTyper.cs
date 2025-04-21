using UnityEngine;

[System.Serializable]
public class AudioSourceTyper
{
    public AudioSourceType SourceType;
    public AudioSource Source;
    //public bool IsPlaying
    //public float PlayingTime;


    public bool IsPlaying()
    {
        if (Source != null) return Source.isPlaying;
        else return false;
    }

    public float PlayingTime()
    {
        if (Source != null) return Source.time;
        else return 0;
    }
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