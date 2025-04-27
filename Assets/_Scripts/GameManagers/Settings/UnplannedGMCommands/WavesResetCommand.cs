using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WavesResetCommand : UnplannedGMCommandsBase
{
    public List<WaveTriggerModule> WaveModulesToReset = new();
    public WavesResetCommand(GameManager GM) : base(GM) { }

    private Coroutine _awaitWaveModules;
    private bool _allSet;

    public IEnumerator TryFindWaveModules(float TimeToWait)
    {
        float elapsedTime = 0f;

        while(WaveModulesToReset.Count < 0 || elapsedTime<TimeToWait)
        {
            WaveTriggerModule[] intermediary = MonoBehaviour.FindObjectsByType<WaveTriggerModule>(FindObjectsSortMode.None);
            WaveModulesToReset = new(intermediary);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (WaveModulesToReset.Count < 0 || WaveModulesToReset == null)
        {
            _allSet = false;
            Debug.LogError($"{this.GetType()} Was unable to find WaveTriggerModules to reset");
        }
        else _allSet = true;

        yield break;
    }

    public override void DoAction()
    {
        base.DoAction();
        if (_allSet)
        {
            foreach(var wm in WaveModulesToReset)
            {
                wm.ResetWaveSpawners();
            }
        }
    }

    public override void DoOnApplicationQuit()
    {
        base.DoOnApplicationQuit();
    }

    public override void DoOnDestroy()
    {
        base.DoOnDestroy();
    }

    public override void DoOnSceneLoad()
    {
        base.DoOnSceneLoad();
    }

    public override void DoReset()
    {
        base.DoReset();

        WaveModulesToReset.Clear();
        _awaitWaveModules = GM.StartCoroutine(TryFindWaveModules(3f));
    }

    public override void DoUpdateAction()
    {
        base.DoUpdateAction();
    }
}
