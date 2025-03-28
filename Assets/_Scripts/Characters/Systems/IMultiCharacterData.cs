public interface IMultiCharacterData
{
    string CharacterName { get; set; }
    NPCType NPCType { get; set; }
    int CharacterTeam { get; set; }

    void SetInitialMultiCharData(SafeInstantiation.CharacterData? characterData);
}