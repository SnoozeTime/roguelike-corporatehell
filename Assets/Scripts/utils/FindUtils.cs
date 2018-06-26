/*
  Some utilities to avoid duplication - For example, a few classes need to fetch a
  unique object based on its tag.
*/
using UnityEngine;
using System.Collections.Generic;

public static class FetchUtils {

    /*
      We fetch the first object that has the given tag.
      If nothing, returns null.
     */
    public static GameObject FetchGameObjectByTag(Tags tag) {
        // need to follow the player.
        GameObject[] players = GameObject.FindGameObjectsWithTag(
            EnumUtils.StringValueOf(tag)
            );

        if (players.Length > 0) {
            return players[0];
        }

        return null;
    }

    /*
      We need that for example to fetch all the doors in a room, or all
      the weapons attached to somebody
    */
    public static List<T> FetchChildrenWithComponent<T>(Transform parent) {
        List<T> childrenWithComp = new List<T>();
        foreach (Transform child in parent) {
            T comp = child.GetComponent<T>();
            if (comp != null) {
                childrenWithComp.Add(comp);
            }
        }
        return childrenWithComp;
    }
}
