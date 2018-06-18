using UnityEngine;
using UnityEditor;
/*
  Just an helper to create the enemies. Enemy prefabs SHOULD be stored in
  Resources/Enemies folder.

  This will load a lot of resources so it should be done only once...
*/
public class EnemyFactory: MonoBehaviour {

    void Start() {
        Object[] enemies = Resources.LoadAll("Enemies");

        // Check the name against the enemy enumeration and add it to the map.
    }
}
