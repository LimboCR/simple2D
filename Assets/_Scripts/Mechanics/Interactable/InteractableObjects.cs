using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObjects : MonoBehaviour
{
    [Header("Action to do on interaction")]
    public UnityEvent eventToDo;

    [SerializeField] private bool _useTriggerZone;
    [SerializeField] private BoxCollider2D _triggerZone;

    [SerializeField] private CircleCollider2D _interactiveZone;
    [SerializeField] private GameObject _interactiveCanvas;

    private NewPlayerController _playerRef;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.IsTouching(_interactiveZone))
        {
            if (other.TryGetComponent<NewPlayerController>(out NewPlayerController player)) _playerRef = player;
            _interactiveCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.IsTouching(_interactiveZone))
        {
            _playerRef = null;
            _interactiveCanvas.SetActive(false);
        }
    }
}
