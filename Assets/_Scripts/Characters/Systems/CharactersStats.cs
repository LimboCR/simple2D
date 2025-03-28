using UnityEngine;

[CreateAssetMenu(menuName = "CharacterSystems")]
public class CharactersStats : ScriptableObject
{
    [Header("Character Stats")]
    [Tooltip("Current health of the character")] public int health;
    [Tooltip("Max health of this character")] public int maxHealth;
    public float moveSpeed;
    public float runSpeed;
    public float jumpHeight;

    [Header("Character Standard Weapon Stats")]
    public int defaultWeaponDamage;
    public float weaponAttackRadius;

    [Header("Character Weapon Variables")]
    [Tooltip("Point from which we'll set attack radius and perform attacks")] public Transform attackPoint;
    [Tooltip("Layer, at which this character will be able to attack and make damage")] public LayerMask attackLayer;

    
}
