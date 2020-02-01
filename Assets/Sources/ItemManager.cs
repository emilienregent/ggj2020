using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private static int inGameIndex = 0;

    [SerializeField]
    private List<ItemController> itemPrefabs = new List<ItemController>();

    private List<ItemController> availableItems = new List<ItemController>();
    private Dictionary<int, ItemController> inGameItems = new Dictionary<int, ItemController>();

    private void Update()
    {
        // Replace when to add new items
        if (Input.GetKeyDown("left"))
        {
            int indexToUse = Random.Range(0, itemPrefabs.Count);

            UseItem(itemPrefabs[indexToUse]);
        }
    }

    // Use an item from pool or create one
    private ItemController UseItem(ItemController itemController)
    {
        ItemController item = null;

        // Search from the pool
        for (int i = 0; i < availableItems.Count; ++i)
        {
            if (availableItems[i].itemType == itemController.itemType)
            {
                // Get item from pool
                item = availableItems[i];

                // Remove item from pool
                availableItems.RemoveAt(i);

                break;
            }
        }

        // Nothing in pull, create one
        if (item == null)
        {
            // Create a new item
            item = Instantiate(itemController);

            item.Initialize(this);
        }

        item.SetReady(++inGameIndex);

        // Add item to the dictionnary of in game items
        inGameItems.Add(item.index, item);

        return item;
    }

    // Release an item to get it back to pool
    public void ReleaseItem(ItemController itemController)
    {
        itemController.gameObject.SetActive(false);

        inGameItems.Remove(itemController.index);

        availableItems.Add(itemController);
    }
}
