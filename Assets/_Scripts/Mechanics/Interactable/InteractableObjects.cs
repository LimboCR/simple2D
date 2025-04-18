using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObjects : MonoBehaviour, IInteractable
{
    [field: SerializeField] public UnityEvent ActionToDo { get; set; }
    [field: SerializeField] public bool Interactable { get; set; }

    [SerializeField] private GameObject _interactiveCanvas;

    public void DoAction()
    {
        if(ActionToDo != null && Interactable) ActionToDo.Invoke();
    }

    public void ShowActionIcon(bool show)
    {
        if(Interactable) _interactiveCanvas.SetActive(show);
    }
}
