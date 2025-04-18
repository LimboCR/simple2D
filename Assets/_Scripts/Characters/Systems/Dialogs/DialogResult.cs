using UnityEngine;

namespace Limbo.DialogSystem
{
    public abstract class DialogResult : ScriptableObject
    {
        public virtual void ApplyResult(GameObject target) { }
    }
}