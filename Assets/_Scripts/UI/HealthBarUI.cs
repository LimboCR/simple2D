using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static GlobalEventsManager;
using System;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image _healthBarFillImage;
    [SerializeField] private Image _healthBarTrailingFillImage;
    [SerializeField] private float _trailDelay = 0.4f;

    private float maxHealth = 100;
    private float _currentHealth;

    private void Awake()
    {
        OnPlayerTakeDamage.AddListener(DisplayTookDamage);
        OnPlayerHeal.AddListener(DisplayHealing);
        OnHealthBarReset.AddListener(HealthBarReset);

        HealthBarReset(maxHealth);
    }

    public void DisplayTookDamage(float amount)
    {
        _currentHealth -= amount;

        HealthTakeDamageDisplay();
    }

    public void DisplayHealing(float amount)
    {
        _currentHealth += amount;
        HealthHealingDisplay();
    }

    public void HealthBarReset(float currentHealth)
    {
        float ratio = currentHealth / maxHealth;
        _currentHealth = currentHealth;

        _healthBarFillImage.fillAmount = ratio;
        _healthBarTrailingFillImage.fillAmount = ratio;
    }

    private void HealthTakeDamageDisplay()
    {
        float ratio = _currentHealth / maxHealth;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_healthBarFillImage.DOFillAmount(ratio, 0.25f)).SetEase(Ease.InOutSine);
        sequence.AppendInterval(_trailDelay);
        sequence.Append(_healthBarTrailingFillImage.DOFillAmount(ratio, 0.3f)).SetEase(Ease.InOutSine);

        sequence.Play();
    }

    private void HealthHealingDisplay()
    {
        float ratio = _currentHealth / maxHealth;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_healthBarTrailingFillImage.DOFillAmount(ratio, 0.25f)).SetEase(Ease.InOutSine);
        sequence.AppendInterval(_trailDelay);
        sequence.Append(_healthBarFillImage.DOFillAmount(ratio, 0.3f)).SetEase(Ease.InOutSine);

        sequence.Play();
    }
}
