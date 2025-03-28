using UnityEngine;

[CreateAssetMenu(menuName = "CharacterSystems/NPC Spawn Presets")]
public class SpawnPresets : ScriptableObject
{
    public GameObject NPCPrefab;
    public ScriptableNPCDefinition npcSO;
}
