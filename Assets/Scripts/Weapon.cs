using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon: MonoBehaviour {

    [SerializeField]
    private float timeBetweenShots;

    private bool canAttack;

    // Need to ignore the collision between me and my bullet.
    protected Collider2D myCollider;

    public void Start() {
        canAttack = true;
        // GameObject[] bulletContainers = GameObject.FindGameObjectsWithTag(
        //     EnumUtils.StringValueOf(Tags.BULLET_CONTAINER));
        // if (bulletContainers.Length > 0) {
        //     // pick the first container.
        //     bulletContainer = bulletContainers[0];
        // }

        myCollider = transform.parent.GetComponent<Collider2D>();
    }

    public void OnEnable() {
        canAttack = true;
    }
    /*
      Will fire a bullet along the given direction. The bullet
      position will be the weapon + an offset
     */
    public void Fire(Vector2 direction) {

        if (canAttack) {

            Attack(direction);
// Initial position of the bullet
            // Vector2 position = transform.position;
            // position += offset * direction;

            // GameObject bullet;
            // if (bulletContainer != null) {
            //     bullet = (GameObject) Instantiate(bulletPrefab, position, Quaternion.identity, bulletContainer.transform);
            // } else {
            //     bullet = (GameObject) Instantiate(bulletPrefab, position, Quaternion.identity);
            // }
            // // Set the speed and direction of the bullet. If no bullet component. CRASH
            // Bullet bulletComp = bullet.GetComponent<Bullet>();
            // bulletComp.SetDirection(direction);

            // // Important, to disable collision between the shooter and its bullet.
            // // Somehow does not work. ignored everything... will investigate later.
            // Physics2D.IgnoreCollision(myCollider, bullet.GetComponent<Collider2D>());
            // // dirty approach here.
            // //bulletComp.SetParentCollider(myCollider);

            // Disable shooting for a moment
            StartCoroutine(WaitBetweenShots());
        }
    }

    protected abstract void Attack(Vector2 direction);

    private IEnumerator WaitBetweenShots() {
        canAttack = false;
        yield return new WaitForSeconds(timeBetweenShots);
        canAttack = true;
    }
}
