/*
  Some utilities to avoid duplication - For example, a few classes need to fetch a
  unique object based on its tag.
*/
using UnityEngine;

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
}
