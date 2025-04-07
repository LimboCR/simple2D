using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using static GlobalEventsManager;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager Instance;

    [Header("UI TextMeshPro References \n--(currently manual)--")]
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _coinsText;

    [Space]
    [Header("Stats variables")]
    [SerializeField] private float _health = 0;
    [SerializeField] private int _coinsAmount = 0;

    [Space]
    [Header("Skill Attack")]
    [SerializeField] private Image _skillAttackCooldown;
    [SerializeField] private Image _skillIcon;
    bool IsSkillAtCooldown = false;
    bool resetSkillCooldown = false;

    [SerializeField] private Color _skillAtCooldownColor;
    private Color _initialSkillRefColor;

    private NewPlayerController Player;

    [Space]
    [Header("Test Player HUD")]
    public GameObject InventoryPanel;
    public List<GameObject> InventorySlotImages;
    List<GameObject> inventorySlots;

    public TMP_InputField CommandLine;

    [Header("StatusEffect Display")]
    [SerializeField] private GameObject _statusEffectsPanel;
    [SerializeField] private List<GameObject> _statusEffectsImages;
    [SerializeField] private GameObject _statusEffectImagePrefab;

    private GameManager _gameManager;

    //Clock staff
    [SerializeField] private TMP_Text _hoursText, _minutesText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        GettersAndFinders();
        EventSubscriber();
        FetchQuickItemsSlots();

        _initialSkillRefColor = _skillIcon.color;
    }
    
    void Update()
    {
        if (IsSkillAtCooldown == true)
        {
            if (resetSkillCooldown == false)
            {
                resetSkillCooldown = true;
                _skillAttackCooldown.fillAmount = 0;
                _skillIcon.color = _skillAtCooldownColor;
            }
            ShowSkillCooldown();
        }
        else if (IsSkillAtCooldown == false)
        {
            _skillAttackCooldown.fillAmount = 0;
            _skillIcon.color = _initialSkillRefColor;
            resetSkillCooldown = false;
        }

        if (Player != null)
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                if (inventorySlots[0].TryGetComponent<ItemSlotStorage>(out ItemSlotStorage slotStorage))
                {
                    if (slotStorage.ItemInstance != null)
                        slotStorage.ConsumeItem(Player.gameObject);
                }
            }

            if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                if (inventorySlots[1].TryGetComponent<ItemSlotStorage>(out ItemSlotStorage slotStorage))
                {
                    if (slotStorage.ItemInstance != null)
                        slotStorage.ConsumeItem(Player.gameObject);
                }
            }

            if (Keyboard.current.digit3Key.wasPressedThisFrame)
            {
                if (inventorySlots[2].TryGetComponent<ItemSlotStorage>(out ItemSlotStorage slotStorage))
                {
                    if (slotStorage.ItemInstance != null)
                        slotStorage.ConsumeItem(Player.gameObject);
                }
            }

            if (Keyboard.current.digit4Key.wasPressedThisFrame)
            {
                if (inventorySlots[3].TryGetComponent<ItemSlotStorage>(out ItemSlotStorage slotStorage))
                {
                    if (slotStorage.ItemInstance != null)
                        slotStorage.ConsumeItem(Player.gameObject);
                }
            }

            if (Keyboard.current.digit5Key.wasPressedThisFrame)
            {
                if (inventorySlots[4].TryGetComponent<ItemSlotStorage>(out ItemSlotStorage slotStorage))
                {
                    if (slotStorage.ItemInstance != null)
                        slotStorage.ConsumeItem(Player.gameObject);
                }
            }

            if (Keyboard.current.digit6Key.wasPressedThisFrame)
            {
                if (inventorySlots[5].TryGetComponent<ItemSlotStorage>(out ItemSlotStorage slotStorage))
                {
                    if (slotStorage.ItemInstance != null)
                        slotStorage.ConsumeItem(Player.gameObject);
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
        OnPlayerHealthChanged.AddListener(HelthStatsDisplay);
        OnPlayerCoinsChanged.AddListener(CoinsStatsDisplay);
        OnPlayerSkillAttackCooldown.AddListener(IsPlayerSkillCooldown);
    }

    private void HelthStatsDisplay(float currentHealth)
    {
        _health = currentHealth;
        if (_healthText) _healthText.text = _health.ToString();

    }

    private void CoinsStatsDisplay(int currentCoins)
    {
        _coinsAmount = currentCoins;
        if (_coinsText) _coinsText.text = _coinsAmount.ToString();
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
        float ratio = Player.SkillCooldownElapsedTime / Player.SkillCooldownTimer;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_skillAttackCooldown.DOFillAmount(ratio, 0.01f)).SetEase(Ease.InOutSine);
        sequence.Play();
    }

    private void IsPlayerSkillCooldown(bool info)
    {
        IsSkillAtCooldown = info;
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
