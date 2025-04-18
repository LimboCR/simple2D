using System;
using UnityEngine;


namespace Limbo.DialogSystem
{
    [Serializable]
    public class DialogOptionVariant
    {
        [TextArea] public string OptionText;
        public DialogNode NextNode;
        public DialogResult Result;
    }
}