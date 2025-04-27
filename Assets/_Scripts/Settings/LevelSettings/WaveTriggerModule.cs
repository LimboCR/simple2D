using UnityEngine;

public class WaveTriggerModule : MonoBehaviour
{
    [Header("Wave Trigger Settings")]
    public bool SynchronizeWaves = false;
    public WaveSpawner LeftSpawner, RightSpawner;
    public bool StartSequence;
    private int Counter = 0;

    private void Awake()
    {
        GlobalEventsManager.OnResetOtherSpawns.AddListener(ResetWaveSpawners);
    }

    private void Update()
    {
        if (StartSequence)
        {
            if (SynchronizeWaves) WaveSynchronizer();
        }
    }

    public void TriggerWave()
    {
        if(AudioManager.Instance.AudioPacks[EAudioPackType.Soundtracks].TracksDictionary.TryGetValue("Action2", out AudioClip clip))
            GlobalEventsManager.PlayMusicSource(clip, PlayMode.force, true);

        LeftSpawner.WaveSpawnerActive = true;
        RightSpawner.WaveSpawnerActive = true;
        StartSequence = true;
    }

    private void WaveSynchronizer()
    {
        if (LeftSpawner.CurrentWaveIsNull && RightSpawner.CurrentWaveIsNull)
        {
            //Debug.Log($"[WaveTriggerModule] [WaveSynchronizer] Waves synchronized trigger activated.");
            Counter++;
            LeftSpawner.TriggerNextWave = true;
            RightSpawner.TriggerNextWave = true;
        }

        if (LeftSpawner.CurrentWave < 0)
        {
            //Debug.Log($"[WaveTriggerModule] [WaveSynchronizer] Left Spawner out of waves to spawn");
            LeftSpawner.WaveSpawnerActive = false;
            LeftSpawner.TriggerNextWave = false;
        }

        if (RightSpawner.CurrentWave < 0)
        {
            //Debug.Log($"[WaveTriggerModule] [WaveSynchronizer] Right Spawner out of waves to spawn");
            RightSpawner.WaveSpawnerActive = false;
            RightSpawner.TriggerNextWave = false;
        }

        if (LeftSpawner.CurrentWave < 0 && RightSpawner.CurrentWave < 0)
        {
            //Debug.Log($"[WaveTriggerModule] [WaveSynchronizer] Both WavesSpawners out of waves. Ending sequence.");
            StartSequence = false;
        }
    }

    public void ResetWaveSpawners()
    {
        StartSequence = false;

        LeftSpawner.WaveSpawnerActive = false;
        RightSpawner.WaveSpawnerActive = false;

        LeftSpawner.CurrentWaveIsNull = true;
        RightSpawner.CurrentWaveIsNull = true;

        LeftSpawner.StopAllCoroutines();
        RightSpawner.StopAllCoroutines();

        LeftSpawner.CurrentWaveNPCs.Clear();
        RightSpawner.CurrentWaveNPCs.Clear();

        LeftSpawner.CurrentWave = 0;
        RightSpawner.CurrentWave = 0;

        StartCoroutine(AudioManager.Level1_Training());
    }
}
