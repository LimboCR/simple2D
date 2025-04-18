using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeHandler : MonoBehaviour
{
    [HideInInspector] public SkillTreeHandler Instance;

    [Header("====Assigned Skills====")]
    [Header("Active Skills")]
    public SkillBase ActiveSkill1;
    public SkillBase ActiveSkill2;
    [Header("Passive Skills")]
    public SkillBase PassiveSkill1;
    public SkillBase PassiveSkill2;

    [Space, Header("Unlocked Skills")]
    public List<SkillBase> PossibleSkills = new();

    [Space, Header("UI Assignments")]
    public List<Image> SkillSlots = new();
    public GameObject SkillOptionsMenu;

    private void Awake()
    {
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void AssignSkill(int slotNumber, SkillBase skillToAssign)
    {
        if (skillToAssign == null) Debug.LogError("Skill to assign is null");

        if (slotNumber == 1) ActiveSkill1 = skillToAssign;
        if (slotNumber == 2) ActiveSkill2 = skillToAssign;
        else Debug.LogError($"Trying to assign skill to slot number: {slotNumber}, which does not exist");
    }
}
