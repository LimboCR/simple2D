using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZones : MonoBehaviour
{
    [Header("Colliders Assignment")]
    public BoxCollider2D InteractableZone;
    public CircleCollider2D AttackZone;
    public BoxCollider2D PickUpZone;

    [Space]
    [Header("Targets at attack zone")]
    public List<GameObject> AttackableObjectsAtZone;

    [Space]
    [Header("Objects in interactive zone")]
    public List<Collider2D> InteractableObjectsAtZone;

    [Space]
    [Header("Objects in pickup zone")]
    public List<Collider2D> ItemsAtPickZone;

    private NewPlayerController _player;
    private bool IsAttacking = false;

    [Space, Header("Garbage collector")]
    public List<GameObject> _garbageCollector = new();
    private void Awake()
    {
        _player = GetComponent<NewPlayerController>();
    }

    private void Update()
    {
        if(!IsAttacking && _garbageCollector.Count > 0)
        {
            CleanGarbage();
        }
    }

    private void CleanGarbage()
    {
        Debug.Log($"GarbageCollector working");
        foreach(var obj in _garbageCollector)
        {
            if (AttackableObjectsAtZone.Contains(obj))
            {
                AttackableObjectsAtZone.Remove(obj);
            }
        }
        _garbageCollector.Clear();
        Debug.Log($"GarbageCollector finished");
    }

    private void AttackableListCleaner()
    {
        AttackableObjectsAtZone.RemoveAll(item => item == null);
        AttackableObjectsAtZone.RemoveAll(item => item.CompareTag("Dead"));
        //if (AttackableObjectsAtZone.Contains(null))
        //    AttackableObjectsAtZone.Remove(null);

        /*foreach (var obj in AttackableObjectsAtZone)
        {
            if (obj.CompareTag("Dead") || obj == null)
            {
                AttackableObjectsAtZone.Remove(obj);
                break;
            }
        }*/
    }

    #region Interactive Functions
    public void TryInteract()
    {
        if (InteractableObjectsAtZone != null && InteractableObjectsAtZone.Count > 0)
        {
            foreach (var col in InteractableObjectsAtZone)
            {
                if (col.gameObject.TryGetComponent<IInteractable>(out IInteractable logic))
                {
                    logic.DoAction();
                    break;
                }
            }
        }
    }

    #endregion

    #region Attack Zone Functions
    public void TryAttack(float amount) // add later StatusEffectSO[] effect = null
    {
        AttackableListCleaner();
        if (AttackableObjectsAtZone != null && AttackableObjectsAtZone.Count > 0)
        {
            IsAttacking = true;
            AttackableListCleaner();
            //List<GameObject> copy = AttackableObjectsAtZone;
            foreach (var obj in AttackableObjectsAtZone)
            {
                if (obj.TryGetComponent<IDamageble>(out IDamageble damageble))
                {
                    damageble.TakeDamage(amount);
                }
            }
            AttackableListCleaner();
            IsAttacking = false;
        }
        IsAttacking = false;
        return;
    }
    #endregion

    #region On Trigger Enter/Exit
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.IsTouching(AttackZone))
        {
            if (other.CompareTag("Dead")) return;
            AttackTriggerEnter(other);
        }
        
        if (other.IsTouching(InteractableZone))
        {
            InteractableTriggerEnter(other);
        }

        if (other.IsTouching(PickUpZone))
        {
            PickUpTriggerEnter(other);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (AttackableObjectsAtZone.Contains(other.gameObject))
        {
            AttackTriggerExit(other);
        }

        if (InteractableObjectsAtZone.Contains(other))
        {
            InteractableTriggerExit(other);
        }

        if (!other.IsTouching(PickUpZone))
        {
            PickUpTriggerExit(other);
        }
    }

    #endregion

    #region Colider-based triggers logic

    #region Interactable
    private void InteractableTriggerEnter(Collider2D other)
    {
        if (other.TryGetComponent<IInteractable>(out IInteractable logic))
        {
            InteractableObjectsAtZone.Add(other);
            logic.ShowActionIcon(true);
        }
    }
    private void InteractableTriggerExit(Collider2D other)
    {
        if (!other.IsTouching(InteractableZone))
        {
            if (other.TryGetComponent<IInteractable>(out IInteractable logic)) logic.ShowActionIcon(false);
            InteractableObjectsAtZone.Remove(other);
        }
    }
    #endregion

    #region AttackZone
    private void AttackTriggerEnter(Collider2D other)
    {
        AttackableObjectsAtZone.Add(other.gameObject);
    }
    private void AttackTriggerExit(Collider2D other)
    {
        if(IsAttacking)
        {
            _garbageCollector.Add(other.gameObject);
            return;
        }
        if (!other.IsTouching(AttackZone)) AttackableObjectsAtZone.Remove(other.gameObject);
    }
    #endregion

    #region PickUp Zone
    private void PickUpTriggerEnter(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            if(other.TryGetComponent<CurrencyItem>(out CurrencyItem coin))
            {
                _player.GiveCoin(coin.type, coin.Amount);
                coin.PickUp();
            }
        }
    }
    private void PickUpTriggerExit(Collider2D other)
    {

    }
    #endregion

    #endregion
}
