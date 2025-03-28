using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour
{
    [Header("Events to Trigger")]
    public UnityEvent onTargetEnter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onTargetEnter?.Invoke(); // Trigger assigned events
            this.gameObject.SetActive(false);
        }
    }
}
