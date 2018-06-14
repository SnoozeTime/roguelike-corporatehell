using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Weapon: MonoBehaviour {

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private float timeBetweenShots;

    [SerializeField]
    private float offset;

    private bool canShoot;

    // Need to ignore the collision between me and my bullet.
    private Collider2D myCollider;

    // Parent objects for our bullets. If null, bullets will be created in the main hierarchy
    private GameObject bulletContainer;
    void Start() {
        canShoot = true;
        GameObject[] bulletContainers = GameObject.FindGameObjectsWithTag(
            EnumUtils.StringValueOf(Tags.BULLET_CONTAINER));
        if (bulletContainers.Length > 0) {
            // pick the first container.
            bulletContainer = bulletContainers[0];
        }

        myCollider = GetComponent<Collider2D>();
    }

    /*
      Will fire a bullet along the given direction. The bullet
      position will be the weapon + an offset
     */
    public void Fire(Vector2 direction) {

        if (canShoot) {
            // Initial position of the bullet
            Vector2 position = transform.position;
            position += offset * direction;

            GameObject bullet;
            if (bulletContainer != null) {
                bullet = (GameObject) Instantiate(bulletPrefab, position, Quaternion.identity, bulletContainer.transform);
            } else {
                bullet = (GameObject) Instantiate(bulletPrefab, position, Quaternion.identity);
            }
            // Set the speed and direction of the bullet. If no bullet component. CRASH
            Bullet bulletComp = bullet.GetComponent<Bullet>();
            bulletComp.SetDirection(direction);

            // Important, to disable collision between the shooter and its bullet.
            // Somehow does not work. ignored everything... will investigate later.
            //Physics2D.IgnoreCollision(myCollider, bullet.GetComponent<Collider2D>());
            // dirty approach here.
            bulletComp.SetParentCollider(myCollider);

            // Disable shooting for a moment
            StartCoroutine(WaitBetweenShots());
        }
    }

    private IEnumerator WaitBetweenShots() {
        canShoot = false;
        yield return new WaitForSeconds(timeBetweenShots);
        canShoot = true;
    }
}
