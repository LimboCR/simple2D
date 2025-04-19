using Limbo.DialogSystem;
using System;
using UnityEngine;
using UnityEngine.Events;

public class GlobalEventsManager : MonoBehaviour
{
    #region Sounds Events
    public static UnityEvent<AudioClip, PlayMode> OnGameManagerSFXPlay = new();
    public static UnityEvent<PlayMode, AudioClip[]> OnGameManagerSFXPlayRandom = new();

    public static UnityEvent<AudioClip, PlayMode> OnMusicSourcePlay = new();
    public static UnityEvent<PlayMode, AudioClip[]> OnMusicSourcePlayRandom = new();

    public static UnityEvent<AudioClip, PlayMode> OnPlayerInteractionPlay = new();
    public static UnityEvent<PlayMode, AudioClip[]> OnPlayerInteractionPlayRandom = new();

    public static UnityEvent<AudioSourceType> OnForceStopPlaying = new();
    #region Volume Settings
    public static UnityEvent<float> OnVolumeChange = new();
    #endregion
    #endregion

    #region Player Events

    #region Health
    public static UnityEvent<float> OnPlayerCurrentHealth = new();
    public static UnityEvent<float> OnPlayerMaxHealth = new();
    public static UnityEvent<float> OnPlayerTakeDamage = new();
    public static UnityEvent<float> OnPlayerHeal = new();
    #endregion

    #region Coins
    public static UnityEvent<ECollectable, int> OnPlayerCoinsChanged = new();

    public static UnityEvent<int> OnPlayerGoldenCoinsChange = new();
    public static UnityEvent<int> OnPlayerSilverCoinsChange = new();
    public static UnityEvent<int> OnPlayerRedCoinsChange = new();
    #endregion

    #region Combat
    public static UnityEvent<bool, float> OnPlayerHeavyAttackCooldown = new();

    /// <summary>
    /// <br>Sends skills cooldown event.</br> <br></br>
    /// <br><u>UnityEvent[bool, int, float]</u>, where:</br><br></br>
    /// <br><b><u>bool</u></b> - true if at cool down</br>
    /// <br><b><u>int(0 - 3)</u></b> number of which skill is at cooldown</br>
    /// <br><b><u>float</u></b> - time of cooldown for the skill</br>
    /// </summary>
    public static UnityEvent<bool, int, float> OnPlayerSkillAttackCooldown = new();
    #endregion

    #endregion

    #region Dialog Events
    public static UnityEvent<DialogNode> OnTryStartDialog = new();
    #endregion

    #region Game State Events
    public static UnityEvent<GameStates> OnStateChange = new();
    public static UnityEvent<GameStates> GameStateListener = new();
    #endregion

    #region NPCs Events
    public static UnityEvent<GameObject, NPCType> OnNPCSpawn = new();
    public static UnityEvent<GameObject, NPCType> OnNPCRemove = new();
    #endregion

    #region Spawn Points Events
    public static UnityEvent<GameObject> OnSpawnAdd = new();
    public static UnityEvent<GameObject> OnSpawnRemove = new();
    #endregion

    #region UI Events
    public static UnityEvent<string> OnMessageSent = new();
    public static UnityEvent<bool> OnShowDeathScreen = new();
    public static UnityEvent OnHealthBarReset = new();
    #endregion

    #region NPCs Events Logic
    public static void SendEnemySpawn(GameObject npc, NPCType type)
    {
        OnNPCSpawn.Invoke(npc, type);
    }

    public static void SendEnemyRemove(GameObject npc, NPCType type)
    {
        OnNPCRemove.Invoke(npc, type);
    }

    #endregion

    #region Spawn Points Events Logics
    public static void SendSpawnPoint(GameObject spawnPoint)
    {
        OnSpawnAdd.Invoke(spawnPoint);
    }

    public static void RemoveSpawnPoint(GameObject spawnPoint)
    {
        OnSpawnRemove.Invoke(spawnPoint);
    }
    #endregion

    #region Player Events Logic
    public static void SendPlayerCurrentHealth(float currentHealth)
    {
        OnPlayerCurrentHealth.Invoke(currentHealth);
    }

    public static void SendPlayerMaxHealth(float maxHealth)
    {
        OnPlayerMaxHealth.Invoke(maxHealth);
    }

    public static void SendPlayerTookDamage(float amount)
    {
        OnPlayerTakeDamage.Invoke(amount);
    }
    public static void SendPlayerHeal(float amount)
    {
        OnPlayerHeal.Invoke(amount);
    }

    public static void SendPlayerHeavyAttackCooldown(bool isInCooldown, float cooldownTime)
    {
        OnPlayerHeavyAttackCooldown.Invoke(isInCooldown, cooldownTime);
    }

    public static void SendCoinsChanged(ECollectable type, int amount)
    {
        OnPlayerCoinsChanged.Invoke(type, amount);
    }

    public static void SendGoldenCoinsChanged(int amount)
    {
        OnPlayerGoldenCoinsChange.Invoke(amount);
    }
    public static void SendSilverCoinsChanged(int amount)
    {
        OnPlayerSilverCoinsChange.Invoke(amount);
    }
    public static void SendRedCoinsChanged(int amount)
    {
        OnPlayerRedCoinsChange.Invoke(amount);
    }
    #endregion

    #region Game States Events Logic
    public static void SendNewGameState(GameStates state)
    {
        OnStateChange.Invoke(state);
    }

    public static void BroadcastActualGameState(GameStates state)
    {
        GameStateListener.Invoke(state);
    }

    #endregion

    #region UI Events Logic
    public static void ShowNotification(string messageText)
    {
        OnMessageSent.Invoke(messageText);
    }

    public static void ShowDeathScreen(bool @active)
    {
        OnShowDeathScreen.Invoke(active);
    }

    public static void ResetHealthBar()
    {
        OnHealthBarReset.Invoke();
    }
    #endregion

    #region Sounds Events Logic

    #region FixedSources
    #region Play
    public static void PlayGMSfx(AudioClip clip, PlayMode playMode = PlayMode.safe)
    {
        OnGameManagerSFXPlay.Invoke(clip, playMode);
    }
    public static void PlayMusicSource(AudioClip clip, PlayMode playMode = PlayMode.safe)
    {
        OnMusicSourcePlay.Invoke(clip, playMode);
    }
    public static void PlayPlayerInteraction(AudioClip clip, PlayMode playMode = PlayMode.safe)
    {
        OnPlayerInteractionPlay.Invoke(clip, playMode);
    }

    #endregion

    #region PlayRandom
    public static void PlayRandomGMSfx(PlayMode playMode = PlayMode.safe, params AudioClip[] clip)
    {
        OnGameManagerSFXPlayRandom.Invoke(playMode, clip);
    }
    public static void PlayRandomMusicSource(PlayMode playMode = PlayMode.safe, params AudioClip[] clip)
    {
        OnMusicSourcePlayRandom.Invoke(playMode, clip);
    }
    public static void PlayRandomPlayerInteraction(PlayMode playMode = PlayMode.safe, params AudioClip[] clip)
    {
        OnPlayerInteractionPlayRandom.Invoke(playMode, clip);
    }

    #endregion

    #region Force Stop
    public static void ForceStopPlaying(AudioSourceType type)
    {
        OnForceStopPlaying.Invoke(type);
    }
    #endregion

    #endregion

    #endregion

    #region Dialog Logics
    public static void StartDialog(DialogNode startingNode)
    {
        OnTryStartDialog.Invoke(startingNode);
    }
    #endregion
}
