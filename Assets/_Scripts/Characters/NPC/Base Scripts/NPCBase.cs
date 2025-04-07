using UnityEngine;

public abstract class NPCBase : MonoBehaviour, IMultiCharacterData
{
    [Header("Character Data")]
    [field: SerializeField] public string CharacterName { get; set; }
    [field: SerializeField] public NPCType NPCType { get; set; }
    [field: SerializeField] public int CharacterTeam { get; set; }

    public virtual void SetInitialMultiCharData(SafeInstantiation.CharacterData? characterData) { }
}
