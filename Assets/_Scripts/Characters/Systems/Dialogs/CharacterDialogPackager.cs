using System.Collections.Generic;
using UnityEngine;

namespace Limbo.DialogSystem
{
    [CreateAssetMenu(menuName = "Dialog System/Create Character Dialog Package")]
    public class CharacterDialogPackager : ScriptableObject
    {
        public PackageLocalization CurrentLocalization;
        public List<DialogPackageLocalized> Packs = new();

        public DialogNode GetStartingNode()
        {
            if(Packs == null || Packs.Count <= 0)
            {
                Debug.LogError("No dialog packages were added");
                return null;
            }

            foreach(var pack in Packs)
            {
                if(CurrentLocalization == pack.Localization)
                {
                    return pack.StartingNode;
                }
            }
            Debug.LogError("No package with such localization was found");
            return null;
        }
    }
}
