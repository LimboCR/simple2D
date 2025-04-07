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

        _currentHealth = maxHealth;
        _healthBarFillImage.fillAmount = 1f;
        _healthBarTrailingFillImage.fillAmount = 1f;
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
