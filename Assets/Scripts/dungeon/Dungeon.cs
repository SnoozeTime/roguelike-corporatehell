using UnityEngine;

/*
  A dungeon is a set of rooms that the player explore. It
  is generated programmatically at the beginning of the floor.

  A dungeon has a layout of rooms. The rooms have all the same size
  and no corridor between them so the layout can just be represented
  as a 2D array of integer...

 */
public class Dungeon: MonoBehaviour {

    private AssetFactory factory;

    public void Start() {
        factory = new AssetFactory();
        factory.LoadAll();

        // then create three enemies.
        InstantiateAsset(factory.GetEnemyPrefab(EnemyType.ENEMY_1),
                                                new Vector2(0, 0));
    }

    private void GenerateRoom() {

    }

    private void InstantiateAsset(GameObject prefab, Vector2 position) {
        Vector3 position3 = (Vector3) position;
        Instantiate(prefab, position3, Quaternion.identity, transform);
    }
}
