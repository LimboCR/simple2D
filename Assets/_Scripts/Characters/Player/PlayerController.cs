using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(PlayerMovement), typeof(Health), typeof(AnimationStateHandler))]
[RequireComponent (typeof(PlayerCombat), typeof(Messanger))]
public class PlayerController : MonoBehaviour
{
    [Header("Neccessary Scripts")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Health _health;
    [SerializeField] private PlayerCombat _combat;
    
    [Space]
    [Header("Stats UI")]
    public TMP_Text healthText;
    public TMP_Text coinsText;

    [Space]
    [Header("Stats Variables")]
    [SerializeField] private bool _isAlive;
    [SerializeField] private int _playerCoins;

    public bool IsAlive { get { return _isAlive; } set { _isAlive = value; } }
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _health = GetComponent<Health>();

        _health.currentHealth = _health.maxHealth;
        _health.alive = true;
        IsAlive = true;
    }

    void Update()
    {
        statsHandler();
    }

    private void statsHandler()
    {
        if (healthText == null) return;

        healthText.text = _health.currentHealth.ToString();
        //coinsText.text = _playerCoins.ToString();

        if (_health.currentHealth == _health.maxHealth && _health.currentHealth >= _health.maxHealth /2)
        {
            healthText.color = Color.green;
        }
        else if(_health.currentHealth < _health.maxHealth /2 && _health.currentHealth >= _health.maxHealth *0.3f)
        {
            healthText.color = Color.yellow;
        }
        else if (_health.currentHealth < _health.maxHealth *0.3f && _health.currentHealth >= 0)
        {
            healthText.color = Color.red;
        }

        if(_health.currentHealth <= 0)
        {
            Die();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            _playerCoins += 1;
            GlobalEventsManager.SendCoinsChanged(_playerCoins);
            Destroy(collision.gameObject);
        }
    }

    public void Die()
    {
        this.tag = "Dead";
        IsAlive = false;
        _animator.SetTrigger("Death");
    }
}
