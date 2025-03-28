using UnityEngine;

[CreateAssetMenu(menuName = "CharacterSystems/NPC Definition")]
public class ScriptableNPCDefinition : ScriptableObject
{
    [Header("Create Charachter Default values")]
    public string Name;
    [Tooltip("Friendly - 0, Enemy - 1"), Range(0, 1)] public int team;
    public LayerMask friendlyMask;
    public LayerMask enemyMask;
    [Space]
    [Header("Character Stats")]
    public float moveSpeed;
    public float chaseSpeed;
    public float jumpHeight;
    
    [Header("Health settings")]
    public int maxHealth;
    public float regenDelay;
    public int regenRate;

    [Header("Character Weapon Stats")]
    public int weaponDamage;
    public float weaponAttackRadius;
    public float chaseRange;
}
