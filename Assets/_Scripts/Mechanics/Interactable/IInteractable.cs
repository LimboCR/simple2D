using UnityEngine.Events;

public interface IInteractable
{
    public bool Interactable { get; set; }
    public UnityEvent ActionToDo { get; set; }
    public void DoAction() { }
    public void ShowActionIcon(bool show) { }
}
