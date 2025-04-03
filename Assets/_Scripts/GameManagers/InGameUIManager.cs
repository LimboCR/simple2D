using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class InGameUIManager : MonoBehaviour
{
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
    [Header("Player Test Usable Inventory")]
    public GameObject InventoryPanel;
    public List<GameObject> InventorySlotImages;
    List<GameObject> inventorySlots;

    public InputField CommandLine;

    private void Awake()
    {
        GlobalEventsManager.OnPlayerHealthChanged.AddListener(HelthStatsDisplay);
        GlobalEventsManager.OnPlayerCoinsChanged.AddListener(CoinsStatsDisplay);
        GlobalEventsManager.OnPlayerSkillAttackCooldown.AddListener(IsPlayerSkillCooldown);
        
        inventorySlots = new();

        foreach (Transform child in InventoryPanel.transform) inventorySlots.Add(child.gameObject);

        for(int i = 0; i< inventorySlots.Count-1; i++)
            InventorySlotImages.Add(inventorySlots[i].transform.GetChild(0).gameObject);

        if (FindAnyObjectByType<NewPlayerController>())
            Player = FindAnyObjectByType<NewPlayerController>();

        _initialSkillRefColor = _skillIcon.color;
    }

    // Update is called once per frame
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
    }

    private void HelthStatsDisplay(float currentHealth)
    {
        _health = currentHealth;
        if (_healthText) _healthText.text = _health.ToString();

    }

    private void CoinsStatsDisplay(int currentCoins)
    {
        _coinsAmount = currentCoins;
        if(_coinsText) _coinsText.text = _coinsAmount.ToString();
    }

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

    public void ExecuteCommand()
    {
        if (CommandLine.text.StartsWith("give_item"))
        {
            string[] parts = CommandLine.text.Split(' ');

            if (parts.Length >= 2) // Ensure at least "give_item item_name"
            {
                string itemKey = parts[1]; // The item name
                int amount = (parts.Length >= 3) ? int.Parse(parts[2]) : 1; // Default to 1 if no amount is provided

                AddItemToInventory(itemKey, amount);
            }
            else
            {
                Debug.Log("Invalid command format! Use: give_item <item_name> [amount]");
            }
        }
    }

    private void AddItemToInventory(string itemKey, int amount)
    {
        if(ItemsStorage.ItemsDictionary.TryGetValue(itemKey, out ItemBase requiredItem))
        {

        }
    }
}
