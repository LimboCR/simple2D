using UnityEngine;

namespace Limbo.DialogSystem
{
    [CreateAssetMenu(fileName = "DialogResult - Trigger Wave", menuName = "Dialog System/Result Nodes/Trigger Wave")]
    public class Result_TriggerWave : DialogResult
    {
        public WaveTriggerModule WaveModule;
        public override void ApplyResult(GameObject target)
        {
            //base.ApplyResult(target);
            GlobalEventsManager.ShowNotification("SURVIVE!!!");
            WaveModule = FindAnyObjectByType<WaveTriggerModule>();
            WaveModule.TriggerWave();
        }
    }
}

