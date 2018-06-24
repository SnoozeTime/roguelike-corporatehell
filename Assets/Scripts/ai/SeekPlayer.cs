using UnityEngine;

/*
 Following the player is apparently a trend among enemies, so it deserves
 its own behaviour
 */
public class SeekPlayer: Seek {

    void Start() {
        // We don't care about checking for null. If no player, the game
        // won't work anyway.
        Transform playerTransform = FetchUtils.FetchGameObjectByTag(Tags.PLAYER).transform;

        this.Target = playerTransform;
    }
}
