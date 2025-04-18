using UnityEngine;

namespace Limbo.DialogSystem
{
    [CreateAssetMenu(fileName = "NewDialogPackage", menuName = "Dialog System/Create Dialog Package")]
    public class DialogPackageLocalized : ScriptableObject
    {
        public PackageLocalization Localization;
        public DialogNode StartingNode;
    }
}

public enum PackageLocalization
{
    EN,
    UA,
    RUS
}