using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("AUTOMATIC! EDIT ONLY AT RUNTIME!")]
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange;
    [SerializeField] private int _damage;
    [SerializeField] private LayerMask _attackableLayers;
    [SerializeField] private float _weaponAttackRadius;


    public float AttackRange { get { return _attackRange; } set { _attackRange = value; } }
    public int Damage { get { return _damage; } set { _damage = value; } }
    public Transform AttackPoint { get { return _attackPoint; } set { _attackPoint = value; } }
    public LayerMask AttackableLayers { get { return _attackableLayers; } set { _attackableLayers = value; } }
    public float WeaponAttackRadius { get { return _weaponAttackRadius; } set { _weaponAttackRadius = value; } }


    public void CombatAttack(int inflictDamage = 5)
    {
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(AttackPoint.position, WeaponAttackRadius, _attackableLayers);

        foreach (Collider2D target in hitTargets)
        {
            if (target.TryGetComponent(out Health targetHealth))
            {
                targetHealth.TakeDamage(Damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(AttackPoint.position, WeaponAttackRadius);
    }
}
