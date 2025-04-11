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

    public void ShowWorldSpaceUI()
    {
        _interactiveCanvas.SetActive(true);
    }
    public void HideWorldSpaceUI()
    {
        _interactiveCanvas.SetActive(false);
    }

    public void DoAction()
    {
        if(eventToDo != null) eventToDo.Invoke();
    }
}
