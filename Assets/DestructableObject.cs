using UnityEngine;
using Cainos.LucidEditor;
using System.Collections.Generic;

public class DestructableObject : MonoBehaviour, IDamageble
{
    [Space, Header("Settings")]
    public Color ColorOnHit;
    public int RequireHits = 2;
    [field: SerializeField] public int GotDamagedCounter { get; set; }
    public SpriteRenderer Renderer;

    [Space, Header("Loot")]
    public List<GameObject> _droppableLoot = new();
    public float DropForce = 5f;
    public bool IsLootDropped = false;

    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    private void Awake()
    {
        Renderer = this.GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float amount)
    {
        Color refCol = Renderer.color;
        Renderer.color = ColorOnHit;
        GotDamagedCounter++;
        if (GotDamagedCounter >= RequireHits) Die();
        Renderer.color = refCol;
    }

    public void Die()
    {
        DropLoot();
    }

    public void DropLoot()
    {
        IsLootDropped = true;
        if (_droppableLoot.Count > 0)
        {
            foreach (GameObject loot in _droppableLoot)
            {
                Vector3 spawnPos = transform.position + new Vector3(0, 0.4f, 0);
                GameObject drop = Instantiate(loot, spawnPos, Quaternion.identity);

                Vector2 randomDir = Random.insideUnitCircle.normalized + Vector2.up * 0.5f;
                randomDir.Normalize();

                if (drop.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                    rb.AddForce(randomDir * DropForce, ForceMode2D.Impulse);
            }
        }
        Destroy(gameObject);
    }

    #region Unimplemented
    public void TakeHealing(float amount)
    {
        throw new System.NotImplementedException();
    }
    public void SetInitialIDamageble(SafeInstantiation.HealthStats? healthStats)
    {
        throw new System.NotImplementedException();
    }
    public float RegenDelay { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public float RegenRate { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public bool Alive { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public bool TakingDamage { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    #endregion
}
