using UnityEngine;

/*
 Following the player is apparently a trend among enemies, so it deserves
 its own behaviour
 */
public class SeekPlayer: Seek {

    void Start() {
        // need to follow the player.
        GameObject[] players = GameObject.FindGameObjectsWithTag(
            EnumUtils.StringValueOf(Tags.PLAYER)
            );

        // We don't care about checking the length. if no player, the game
        // won't work anyway.
        Transform playerTransform = players[0].transform;

        this.Target = playerTransform;
    }
}
