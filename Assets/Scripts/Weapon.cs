using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon: MonoBehaviour {

    private bool canAttack;

    // Need to ignore the collision between me and my bullet.
    protected Collider2D myCollider;

    public void Start() {
        canAttack = true;

        myCollider = transform.parent.GetComponent<Collider2D>();
    }

    public void OnEnable() {
        canAttack = true;
    }

    /*
      Called when the weapon is selected. Can be used for example to disable
      colliders.
     */
    public abstract void IsSelected();

    /*
      Will fire a bullet along the given direction. The bullet
      position will be the weapon + an offset
     */
    public void Fire(Vector2 direction) {

        if (canAttack) {
            Attack(direction);

            // Disable shooting for a moment
            StartCoroutine(WaitBetweenShots());
        }
    }

    // Do the attack. For example, create a bullet for a gun, or animate
    // a collider for a sword
    protected abstract void Attack(Vector2 direction);

    // This is weapon specific. For example, a sword time between attack would be
    // the animation time of the sword. For guns, it could be the time to reload
    protected abstract float GetTimeBetweenAttacks();

    private IEnumerator WaitBetweenShots() {
        canAttack = false;
        yield return new WaitForSeconds(GetTimeBetweenAttacks());
        canAttack = true;
    }
}
