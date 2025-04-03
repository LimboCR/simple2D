using UnityEngine;
using UnityEngine.UI;

public abstract class ItemBase
{
    [Tooltip("Each items key must be unique")]public string ItemKey;
    public string ItemName;
    public Sprite ItemSprite;

    [Tooltip("Sets the effect it will provide to consumer")] public StatusEffectSO ItemEffect;
}
