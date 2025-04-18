using UnityEngine;

public class CurrencyItem : MonoBehaviour
{
    public ECollectable type;
    public int Amount = 1;

    public void PickUp()
    {
        Destroy(gameObject);
    }
}

public enum ECollectable
{
    Golden,
    Silver,
    Red,
    SkillPoint
}