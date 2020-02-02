using UnityEngine;
using System.Collections;

public class ItemMalusManager : ItemManager
{
    private void Awake()
    {
        GameManager.instance.itemMalusManager = this;
    }
}
