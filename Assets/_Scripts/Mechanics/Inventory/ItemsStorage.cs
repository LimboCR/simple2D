using System.Collections.Generic;
using UnityEngine;

public class ItemsStorage : MonoBehaviour
{
    public static List<ItemBase> InventoryItems;
    public static Dictionary<string, ItemBase> ItemsDictionary;

    private void Awake()
    {
        if(InventoryItems != null)
        {
            foreach(var item in InventoryItems)
            {
                ItemsDictionary.Add(item.ItemKey, item);
            }
        }
    }
}
