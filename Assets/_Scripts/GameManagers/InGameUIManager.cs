using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using static GlobalEventsManager;
using UnityEngine.SceneManagement;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager Instance;

    #region Currency
    [Space]
    [Header("Coins")]
    private int _goldenC, _silverC, _redC;
    [SerializeField] private TMP_Text _goldenCoins;
    [SerializeField] private TMP_Text _silverCoins;
    [SerializeField] private TMP_Text _redCoins;
    #endregion

    #region Heavy Attack
    [Space]
    [Header("Heavy Attack")]
    [SerializeField] private Image _heavyAttackCooldown;
    [SerializeField] private Image _heavyAttackIcon;
    bool IsHeavyAttackAtCooldown = false;
    bool ResetHeavyAttackCooldown = false;
    float HeavyAttackCooldownTimer = 0f;

    [SerializeField] private Color _skillAtCooldownColor;
    private Color _initialSkillRefColor;
    #endregion

    #region Player Hud
    private NewPlayerController Player;

    [Space]
    [Header("Test Player HUD")]
    public GameObject InventoryPanel;
    public List<GameObject> InventorySlotImages;
    List<GameObject> inventorySlots;

    [Space]
    [Header("Dev Tools")]
    public TMP_InputField CommandLine;

    [Header("StatusEffect Display")]
    [SerializeField] private GameObject _statusEffectsPanel;
    [SerializeField] private List<GameObject> _statusEffectsImages;
    [SerializeField] private GameObject _statusEffectImagePrefab;

    //Clock staff
    [SerializeField] private TMP_Text _hoursText, _minutesText;

    [Space]
    [Header("Notification prefab")]
    public GameObject NotificationPrefab;
    public GameObject NotificationsPanel;

    [Space, Header("Death screen")]
    public GameObject DeathScreen;
    public Button DeathScreenButton;
    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        GettersAndFinders();
        EventSubscriber();
        FetchQuickItemsSlots();

        _initialSkillRefColor = _heavyAttackIcon.color;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnApplicationQuit()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(DeathScreenButton.onClick.GetPersistentEventCount() > 0)
            DeathScreenButton.onClick.RemoveAllListeners();

        DeathScreenButton.onClick.AddListener(GameManager.TryLoad);
    }

    void Update()
    {
        if (IsHeavyAttackAtCooldown == true)
        {
            if (ResetHeavyAttackCooldown == false)
            {
                ResetHeavyAttackCooldown = true;
                _heavyAttackCooldown.fillAmount = 0;
                _heavyAttackIcon.color = _skillAtCooldownColor;
            }
            ShowSkillCooldown();
        }
        else if (IsHeavyAttackAtCooldown == false)
        {
            _heavyAttackCooldown.fillAmount = 0;
            _heavyAttackIcon.color = _initialSkillRefColor;
            ResetHeavyAttackCooldown = false;
        }

        if (Player != null)
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                if (inventorySlots[0].TryGetComponent<ItemSlotStorage>(out ItemSlotStorage slotStorage))
                {
                    if (slotStorage.ItemInstance != null)
                        slotStorage.UseItem(Player.gameObject);
                }
            }

            if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                if (inventorySlots[1].TryGetComponent<ItemSlotStorage>(out ItemSlotStorage slotStorage))
                {
                    if (slotStorage.ItemInstance != null)
                        slotStorage.UseItem(Player.gameObject);
                }
            }

            if (Keyboard.current.digit3Key.wasPressedThisFrame)
            {
                if (inventorySlots[2].TryGetComponent<ItemSlotStorage>(out ItemSlotStorage slotStorage))
                {
                    if (slotStorage.ItemInstance != null)
                        slotStorage.UseItem(Player.gameObject);
                }
            }

            if (Keyboard.current.digit4Key.wasPressedThisFrame)
            {
                if (inventorySlots[3].TryGetComponent<ItemSlotStorage>(out ItemSlotStorage slotStorage))
                {
                    if (slotStorage.ItemInstance != null)
                        slotStorage.UseItem(Player.gameObject);
                }
            }

            if (Keyboard.current.digit5Key.wasPressedThisFrame)
            {
                if (inventorySlots[4].TryGetComponent<ItemSlotStorage>(out ItemSlotStorage slotStorage))
                {
                    if (slotStorage.ItemInstance != null)
                        slotStorage.UseItem(Player.gameObject);
                }
            }

            if (Keyboard.current.digit6Key.wasPressedThisFrame)
            {
                if (inventorySlots[5].TryGetComponent<ItemSlotStorage>(out ItemSlotStorage slotStorage))
                {
                    if (slotStorage.ItemInstance != null)
                        slotStorage.UseItem(Player.gameObject);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        TimeDisplay();
    }

    private void GettersAndFinders()
    {
        if (FindAnyObjectByType<NewPlayerController>())
           Player = FindAnyObjectByType<NewPlayerController>();
    }

    #region Events Based Actions

    private void EventSubscriber()
    {
        OnPlayerCoinsChanged.AddListener(CoinsStatsDisplay);
        OnPlayerHeavyAttackCooldown.AddListener(IsPlayerHeavyAttackAtCooldown);
        OnMessageSent.AddListener(ShowNotification);
        OnShowDeathScreen.AddListener(ShowDeathScreen);
    }

    private void CoinsStatsDisplay(ECollectable type, int amount)
    {
        switch (type)
        {
            case ECollectable.Golden:
                _goldenC = amount;
                if(_goldenCoins) _goldenCoins.text = _goldenC.ToString();
                break;
            case ECollectable.Silver:
                _silverC = amount;
                if (_silverCoins) _silverCoins.text = _silverC.ToString();
                break;
            case ECollectable.Red:
                _redC = amount;
                if (_redCoins) _redCoins.text = _redC.ToString();
                break;

            default: break;
        }
    }

    #endregion

    #region TimeDisplay

    public void TimeDisplay()
    {
        if(_hoursText != null & _minutesText != null)
        {
            if(TimeManager.CurrentHours.ToString().Length < 2) 
                _hoursText.text = "0" + TimeManager.CurrentHours.ToString();
            else _hoursText.text = TimeManager.CurrentHours.ToString();

            if(TimeManager.CurrentMinutes.ToString().Length < 2) 
                _minutesText.text = "0" + TimeManager.CurrentMinutes.ToString();
            else _minutesText.text = TimeManager.CurrentMinutes.ToString();

        }
    }

    #endregion

    #region PlayerHUD Logic
    private void ShowSkillCooldown()
    {
        float ratio = Player.SkillCooldownElapsedTime / HeavyAttackCooldownTimer;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_heavyAttackCooldown.DOFillAmount(ratio, 0.01f)).SetEase(Ease.InOutSine);
        sequence.Play();
    }

    private void IsPlayerHeavyAttackAtCooldown(bool info, float timer)
    {
        HeavyAttackCooldownTimer = 0f;
        HeavyAttackCooldownTimer = timer;
        IsHeavyAttackAtCooldown = info;
    }

    public GameObject DisplayStatusEffect(StatusEffectDescription effectDescription)
    {
        GameObject newStatusEffect = Instantiate(_statusEffectImagePrefab, _statusEffectsPanel.transform);
        if(effectDescription.EffectDescription != null && newStatusEffect.TryGetComponent<Image>(out Image img))
        {
            img.sprite = effectDescription.EffectIcon;
        }
        _statusEffectsImages.Add(newStatusEffect);
        return newStatusEffect;
    }

    public void RemoveStatusEffectImage(GameObject effectImage)
    {
        if (_statusEffectsImages.Contains(effectImage))
        {
            _statusEffectsImages.Remove(effectImage);
            Destroy(effectImage);
        }
    }

    private void ShowDeathScreen(bool active)
    {
        if(DeathScreen!=null) DeathScreen.SetActive(active);
    }

    #endregion

    #region Notifications
    private void ShowNotification(string messageText)
    {
        var msgRef = Instantiate(NotificationPrefab, NotificationsPanel.transform);
        if(msgRef.TryGetComponent<Notification>(out Notification script)) script.SetText(messageText);
    }
    #endregion

    #region Command Line Logic
    public void ExecuteCommand()
    {
        //Example: give_item StandardHeal 10 1 (adds 10 healing spells to 1 quick slot if player inventory)
        if (CommandLine.text.StartsWith("give_item"))
        {
            string[] parts = CommandLine.text.Split(' ');

            if (parts.Length >= 2) // Ensure at least "give_item item_name"
            {
                string itemKey = parts[1]; // The item name
                int amount = (parts.Length >= 3) ? int.Parse(parts[2]) : 1; // Default to 1 if no amount is provided

                int slotNumber = (parts.Length >= 4) ? int.Parse(parts[3]) : 0;

                AddItemToInventory(itemKey, amount, slotNumber);
            }
            else
            {
                Debug.LogWarning("Invalid command format! Use: give_item <item_name> [amount]");
            }
        }
    }

    private void AddItemToInventory(string itemKey, int amount, int slotNumber)
    {
        if (ItemsStorage.Instance.ItemsDictionary.TryGetValue(itemKey, out ItemBase requiredItem))
        {
            if (inventorySlots[slotNumber - 1].TryGetComponent<ItemSlotStorage>(out ItemSlotStorage slotStorage))
            {
                slotStorage.AssignItemToSlot(requiredItem, amount);
            }
        }
    }

    #endregion

    #region Inventory Logic

    private void FetchQuickItemsSlots()
    {
        inventorySlots = new();

        foreach (Transform child in InventoryPanel.transform) inventorySlots.Add(child.gameObject);

        for (int i = 0; i < inventorySlots.Count - 1; i++)
            InventorySlotImages.Add(inventorySlots[i].transform.GetChild(0).gameObject);
    }

    #endregion
}
