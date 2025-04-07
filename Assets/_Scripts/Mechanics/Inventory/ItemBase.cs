using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Inventory Item", menuName = "Inventory System/Create Item")]
public class ItemBase : ScriptableObject
{
    [Tooltip("Each items key must be unique")]public string ItemKey;
    public string ItemName;
    public Sprite ItemSprite;

    [Tooltip("Sets the effect it will provide to consumer")] public StatusEffectSO ItemEffect;
}
