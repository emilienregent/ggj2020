using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private static int inGameIndex = 0;

    [SerializeField]
    private float _playtimeTreshold = 0f;
    [SerializeField]
    private float _minimumDelay = 0f;
    [SerializeField]
    private float _maximumDelay = 0f;
    [SerializeField]
    private AnimationCurve _difficultyByTime = new AnimationCurve();
    [SerializeField]
    private AnimationCurve _spawnDelayByDifficulty = new AnimationCurve();


    [SerializeField]
    private List<ItemController> itemPrefabs = new List<ItemController>();

    private List<ItemController> availableItems = new List<ItemController>();
    private Dictionary<int, ItemController> inGameItems = new Dictionary<int, ItemController>();

    private void Start()
    {
        StartCoroutine("SpawnItems");
    }

    private IEnumerator SpawnItems()
    {
        while (true)
        {
            float difficulty = _difficultyByTime.Evaluate(Time.timeSinceLevelLoad / _playtimeTreshold);

            // From maximum delay (no difficulty) to minimum delay
            float delay = Mathf.Clamp(_maximumDelay * _spawnDelayByDifficulty.Evaluate(difficulty),
                _minimumDelay, _maximumDelay);

            Debug.Log("Start for " + Time.timeSinceLevelLoad + "s\nDifficulty of " + difficulty + " --> Delay of " + delay + "s");

            yield return new WaitForSeconds(delay);

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
