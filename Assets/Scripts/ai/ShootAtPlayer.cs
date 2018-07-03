using System.Collections;
using UnityEngine;

public class ShootAtPlayer: ControllerBehaviour {

    private Transform player;

    void Start() {
        player = FetchUtils.FetchGameObjectByTag(Tags.PLAYER).transform;
    }

    public override Control GetControls() {
        Control control = new Control();
        control.movementControl = new MovementControl();
        control.attackControl = new AttackControl();

        Vector2 direction = (player.position - transform.position).normalized;

        // Raycast in player's direction. If collider is player, then shoot.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);
        if (hit.collider != null) {
            if (hit.transform == player) {
                control.attackControl.shouldFire = true;
                control.attackControl.direction = direction;
            }
        }

        return control;
    }
}
