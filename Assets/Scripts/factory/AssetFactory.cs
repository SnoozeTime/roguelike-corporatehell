using UnityEngine;
using System;
using System.Collections.Generic;

/*
  Helper to create game objects from prefabs.
  The prefabs will be named after enumerations and should be in a specific folder
  For example, can only load enemies that have a name in EnemyType enum and are in
  Resources/Enemies
*/
public class AssetFactory {

    // Enemy loader
    AssetLoader<EnemyType> enemyLoader;

    // Room templates
    AssetLoader<InteriorType> roomLoader;

    public AssetFactory() {
        // instantiate the loaders.
        enemyLoader = new AssetLoader<EnemyType>("Enemies");
        roomLoader = new AssetLoader<InteriorType>("Rooms");
    }

    public void LoadAll() {
        // load the data
        enemyLoader.LoadAssets();
        roomLoader.LoadAssets();
    }

    public GameObject GetEnemyPrefab(EnemyType enemyType) {
        return enemyLoader.Get(enemyType);
    }

    public GameObject GetRoomPrefab(InteriorType roomType) {
        return roomLoader.Get(roomType);
    }
}
