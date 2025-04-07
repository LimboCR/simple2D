using System.Collections.Generic;
using UnityEngine;

public class ItemsStorage : MonoBehaviour
{
    public static ItemsStorage Instance { get; private set; }

    [SerializeField] public List<ItemBase> InventoryItems;
    public Dictionary<string, ItemBase> ItemsDictionary;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        ItemsDictionary = new();
        if (InventoryItems != null && InventoryItems.Count>0)
        {
            foreach(ItemBase item in InventoryItems)
            {
                ItemsDictionary.Add(item.ItemKey, item);
            }
        }
    }
}
