using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class TriggerZone : MonoBehaviour
{
    [Header("Events to Trigger")]
    public UnityEvent onTargetEnter;
    public bool IsArmed = true;

    private void Awake()
    {
        IsArmed = true;
        GlobalEventsManager.OnResetTriggerZones.AddListener(ResetZone);
    }

    private void ResetZone()
    {
        IsArmed = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsArmed && collision.CompareTag("Player"))
        {
            onTargetEnter?.Invoke(); // Trigger assigned events
            IsArmed = false;
            this.gameObject.SetActive(false);
        }
    }
}
