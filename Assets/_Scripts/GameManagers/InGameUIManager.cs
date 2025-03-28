using TMPro;
using UnityEngine;

public class InGameUIManager : MonoBehaviour
{
    [Header("UI TextMeshPro References \n--(currently manual)--")]
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _coinsText;

    [Space]
    [Header("Stats variables")]
    [SerializeField] private int _health = 0;
    [SerializeField] private int _coinsAmount = 0;

    private void Awake()
    {
        GlobalEventsManager.OnPlayerHealthChanged.AddListener(HelthStatsDisplay);
        GlobalEventsManager.OnPlayerCoinsChanged.AddListener(CoinsStatsDisplay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HelthStatsDisplay(int currentHealth)
    {
        _health = currentHealth;
        _healthText.text = _health.ToString();
    }

    private void CoinsStatsDisplay(int currentCoins)
    {
        _coinsAmount = currentCoins;
        _coinsText.text = _coinsAmount.ToString();
    }

    private void HealthBarManager()
    {
        
    }
}
