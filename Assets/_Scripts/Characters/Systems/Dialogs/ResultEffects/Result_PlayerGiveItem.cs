using UnityEngine;

namespace Limbo.DialogSystem
{
    [CreateAssetMenu(fileName = "DialogResult - Give Item", menuName = "Dialog System/Result Nodes/Give Item")]
    public class Result_PlayerGiveItem : DialogResult
    {
        public ECollectable CoinType;
        public int amount;

        public override void ApplyResult(GameObject target)
        {
            base.ApplyResult(target);
            NewPlayerController player = target.GetComponent<NewPlayerController>();

            player.GiveCoin(CoinType, amount);
        }
    }
}

