using UnityEngine;
using System.Collections;

public class ItemMalusManager : ItemManager
{

    public override void StartSpawnItems() {
        if(GameManager.instance.currentGameState == GameManager.enumGameState.Game)
        {
            base.StartSpawnItems();
        }
    }
}
