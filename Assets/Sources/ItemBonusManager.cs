using UnityEngine;
using System.Collections;

public class ItemBonusManager : ItemManager
{
    private void Awake()
    {
        GameManager.instance.itemBonusManager = this;
    }
}
