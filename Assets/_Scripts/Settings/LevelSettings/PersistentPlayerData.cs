using UnityEngine;

[CreateAssetMenu(fileName = "PersistentPlayerData", menuName = "Game/Persistent Player Data")]
public class PersistentPlayerData : ScriptableObject
{
    [Header("Position")]
    public Vector3 LastKnownPosition;

    [Header("Stats")]
    public float Health;
    public float MaxHealth;

    [Space, Header("Coins and Levels")]
    public int GoldenCoins;
    public int SilverCoins;
    public int RedCoins;
    public int SkillPoints;
    public int PlayerLevel;

    [Space, Header("Skills")]
    public SkillBase ActiveSkill1;
    public SkillBase ActiveSkill2;
    public SkillBase PassiveSkill1;
    public SkillBase PassiveSkill2;

    [Space, Header("Attack Staff")]
    public bool HaveAttacked;

    [Space, Header("State Machine States")]
    public PlayerIdleSOBase PlayerIdleBase;
    public PlayerWalkSOBase PlayerWalkBase;
    public PlayerRunSOBase PlayerRunBase;
    public PlayerJumpSOBase PlayerJumpBase;
    public PlayerFallSOBase PlayerFallBase;
    public PlayerRollSOBase PlayerRollBase;
    public PlayerAttackSOBase PlayerAttackBase;
    public PlayerSkillAttackSOBase PlayerSkillAttackBase;
    public PlayerBlockSOBase PlayerBlockBase;
    public PlayerHurtSOBase PlayerHurtBase;
    public PlayerDeadSOBase PlayerDeadBase;

    [Space, Header("Time Manager")]
    public int Hours;
    public int Minutes;

    public void SaveFromPlayer(NewPlayerController player)
    {
        #region Common data
        LastKnownPosition = player.transform.position;
        #endregion

        #region Stats
        Health = player.CurrentHealth;
        MaxHealth = player.MaxHealth;
        #endregion

        #region Currency & Level
        GoldenCoins = player.GoldenCoins;
        SilverCoins = player.SilverCoins;
        RedCoins = player.RedCoins;
        SkillPoints = player.SkillPoints;
        PlayerLevel = player.PlayerLevel;
        #endregion

        #region Skills and attacks
        ActiveSkill1 = player.ActiveSkill1;
        ActiveSkill2 = player.ActiveSkill2;
        PassiveSkill1 = player.PassiveSkill1;
        PassiveSkill2 = player.PassiveSkill2;

        HaveAttacked = player.HaveAttacked;
        #endregion

        #region States
        PlayerIdleBase = player.PlayerIdleBase;
        PlayerAttackBase = player.PlayerAttackBase;
        PlayerWalkBase = player.PlayerWalkBase;
        PlayerRunBase = player.PlayerRunBase;
        PlayerJumpBase = player.PlayerJumpBase;
        PlayerFallBase = player.PlayerFallBase;
        PlayerRollBase = player.PlayerRollBase;
        PlayerSkillAttackBase = player.PlayerSkillAttackBase;
        PlayerBlockBase = player.PlayerBlockBase;
        PlayerHurtBase = player.PlayerHurtBase;
        PlayerDeadBase = player.PlayerDeadBase;
        #endregion
    }

    public void LoadToPlayer(NewPlayerController player)
    {
        #region Stats
        player.CurrentHealth = Health;
        player.MaxHealth = MaxHealth;
        #endregion

        #region Currency & Level
        player.GoldenCoins = GoldenCoins;
        player.SilverCoins = SilverCoins;
        player.RedCoins = RedCoins;
        player.SkillPoints = SkillPoints;
        player.PlayerLevel = PlayerLevel;
        #endregion

        #region Skills and attacks
        player.ActiveSkill1 = ActiveSkill1;
        player.ActiveSkill2 = ActiveSkill2;
        player.PassiveSkill1 = PassiveSkill1;
        player.PassiveSkill2 = PassiveSkill2;

        player.HaveAttacked = HaveAttacked;
        #endregion

        #region States
        player.PlayerIdleBase = PlayerIdleBase;
        player.PlayerAttackBase = PlayerAttackBase;
        player.PlayerWalkBase = PlayerWalkBase;
        player.PlayerRunBase = PlayerRunBase;
        player.PlayerJumpBase = PlayerJumpBase;
        player.PlayerFallBase = PlayerFallBase;
        player.PlayerRollBase = PlayerRollBase;
        player.PlayerSkillAttackBase = PlayerSkillAttackBase;
        player.PlayerBlockBase = PlayerBlockBase;
        player.PlayerHurtBase = PlayerHurtBase;
        player.PlayerDeadBase = PlayerDeadBase;
        #endregion
    }

    public void SaveFromTimeManager(TimeManager time)
    {
        Hours = time.hours;
        Minutes = time.minutes;
    }

    public void LoadToTimeManager(TimeManager time)
    {
        time.SetHours = Hours;
        time.SetMinutes = Minutes;
        time.ChangeTime = true;
    }
}