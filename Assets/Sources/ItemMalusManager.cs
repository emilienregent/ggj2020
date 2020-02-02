using UnityEngine;
using System.Collections;

public class ItemMalusManager : ItemManager
{
    private void Awake()
    {
        GameManager.instance.itemMalusManager = this;
    }

    public override void StartSpawnItems() 
    {
        if(GameManager.instance.currentGameState == GameManager.enumGameState.Game)
        {
            base.StartSpawnItems();
        }
    }
}
