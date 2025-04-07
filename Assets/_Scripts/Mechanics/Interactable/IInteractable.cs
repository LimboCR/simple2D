using UnityEngine;

public interface IInteractable
{
    public void OnSelect();
    public void OnDeselect();
    public void OnPointerEnter();
    public void OnPointerLeave();
}
