using UnityEngine;

[CreateAssetMenu(fileName ="ButtonBindings", menuName ="Player Logic/Button Bindings")]
public class ButtonBindingsSO : ScriptableObject
{
    [Header("Movement")]
    public KeyCode JumpKey = KeyCode.W;
    public KeyCode RollKey = KeyCode.C;
    public KeyCode ShieldKey = KeyCode.Space;

    [Space]
    [Header("Combat button bindings")]
    public KeyCode Skill1 = KeyCode.Q;
    public KeyCode Skill2 = KeyCode.E;
    
    [Space]
    public KeyCode SimpleAttack = KeyCode.Z;
    public KeyCode HeavyAttack = KeyCode.X;

    [Space]
    [Header("Other buttons")]
    public KeyCode InGameMenu = KeyCode.Tab;
    public KeyCode Inventory = KeyCode.I;
    public KeyCode Map = KeyCode.M;
    public KeyCode SkillsMenu = KeyCode.K;

    [Space]
    [Header("Saving")]
    public KeyCode Quicksave = KeyCode.F5;
    public KeyCode Quickload = KeyCode.F6;

}
