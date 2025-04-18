using UnityEngine;
using UnityEngine.UI;

public class ItemSlotStorage : MonoBehaviour
{
    [SerializeField] private Image ItemImageReference;
    //private Sprite ItemSprite;
    public ItemBase ItemInstance;
    public int ItemAmount;

    private void Update()
    {
        if(ItemImageReference.sprite == null) ItemImageReference.gameObject.SetActive(false);
        else if (ItemImageReference.sprite != null) ItemImageReference.gameObject.SetActive(true);

        if(ItemAmount <= 0 && ItemInstance != null)
        {
            ItemInstance = null;
            ItemImageReference.sprite = null;
            ItemAmount = 0;
        }
    }

    public void AssignItemToSlot(ItemBase item, int amount)
    {
        if(ItemInstance == null)
        {
            ItemInstance = Instantiate(item);
            ItemImageReference.sprite = ItemInstance.ItemSprite;
            ItemAmount += amount;
        }
        if(ItemInstance != null)
        {
            if(ItemInstance == item)
            {
                ItemAmount += amount;
            }
            else
            {
                ItemInstance = null;
                ItemImageReference.sprite = null;
                ItemInstance = Instantiate(item);
                ItemImageReference.sprite = ItemInstance.ItemSprite;
                ItemAmount = amount;
            }
        }
    }

    public void RemoveItemFromSlot()
    {
        ItemInstance = null;
        ItemImageReference.sprite = null;
        ItemAmount = 0;
    }

    public void UseItem(GameObject player)
    {
        if(player.TryGetComponent<StatusEffectHandler>(out StatusEffectHandler effectsManager))
        {
            if(ItemInstance.ItemEffect != null)
            {
                effectsManager.ApplyEffectSO(ItemInstance.ItemEffect);
                ItemAmount -= 1;
            }
        }
    }
}
