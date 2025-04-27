using Limbo.CustomEditorAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Space, Header("Wave Spawner Settings")]
    public List<WaveSO> Waves;
    public List<WaveSO> WavesInstances;
    public SpawnPointHandler sph;
    public bool WaveSpawnerActive = false;
    public bool TriggerNextWave = false;
    public bool ShowWaveMessage = false;

    [Space, Header("Wave Spawner Tracker")]
    public int AmountOfWaves;
    public int CurrentWave = 0;
    public List<GameObject> CurrentWaveNPCs;
    public bool CurrentWaveIsNull = true;
    private Coroutine ActiveWave;
    private bool Check = true;

    private void Awake()
    {
        sph = GetComponent<SpawnPointHandler>();
        if(Waves != null && Waves.Count > 0)
        {
            AmountOfWaves = Waves.Count;
            foreach(var wave in Waves)
            {
                WavesInstances.Add(Instantiate(wave));
            }
        }
    }

    private void Update()
    {
        if (WaveSpawnerActive)
        {
            if (TriggerNextWave)
            {
                TriggerNextWave = false;
                if (Check)
                {
                    Check = false;
                    CheckAction();
                }
            }
            else if (!TriggerNextWave && !CurrentWaveIsNull) CurrentWaveTracker();
        }
    }

    private void CheckAction()
    {
        //Debug.Log($"[{gameObject.name}] [Update] Next Wave is triggered. Starting...");

        int waveNum = GetWaveNumber();
        if (waveNum < 0) return;
        else CurrentWave++;

        
        ActiveWave = StartAndTrack(WavesInstances[waveNum].InitiateWave(sph, this), () => Safety(waveNum));
        
    }

    private void Safety(int waveNum)
    {
        CurrentWaveIsNull = false;
        ActiveWave = null;
        Check = true;

        if (ShowWaveMessage)
        {
            GlobalEventsManager.ShowNotification($"Wave #{waveNum+1}");
            GlobalEventsManager.ShowNotification($"Eneimes: {CurrentWaveNPCs.Count * 2}");
        }
    }

    private void CurrentWaveTracker()
    {
        if (CurrentWaveNPCs == null || CurrentWaveNPCs.Count < 0)
        {
            CurrentWaveIsNull = true;
            return;
        }
        else if (CurrentWaveNPCs != null && CurrentWaveNPCs.Count > 0) CurrentWaveIsNull = false;

        if (!CurrentWaveIsNull)
        {
            int deadNPCs = 0;
            foreach(var npc in CurrentWaveNPCs)
            {
                if (npc == null) deadNPCs++;
            }

            if(deadNPCs == CurrentWaveNPCs.Count)
            {
                //Debug.Log($"[{gameObject.name}] [CurrentWaveTracker] All NPCs of this wave have died");
                CurrentWaveNPCs.Clear();
                CurrentWaveIsNull = true;
            }
        }
    }

    public void TriggerWave()
    {
        WaveSpawnerActive = true;
    }

    private int GetWaveNumber()
    {
        if (CurrentWave <= (AmountOfWaves - 1))
        {
            //Debug.Log($"[{gameObject.name}] [GetWaveNumber] Spawning wave number {CurrentWave + 1}");
            return CurrentWave;
        }
        else
        {
            //Debug.Log($"[{gameObject.name}] [GetWaveNumber] No more waves left.");
            CurrentWave = -1;
            return -1;
        }
    }

    public Coroutine StartAndTrack(IEnumerator routine, Action onComplete)
    {
        return StartCoroutine(Wrapper());

        IEnumerator Wrapper()
        {
            yield return StartCoroutine(routine);
            onComplete?.Invoke();
        }
    }

    public void AddNpcToTrack(GameObject npc)
    {
        CurrentWaveNPCs.Add(npc);
    }
}
