using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGun: Weapon {

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private float offset;

    // Parent objects for our bullets. If null, bullets will be created in the main hierarchy
    private GameObject bulletContainer;

    public void Start() {
        base.Start();
        bulletContainer = FetchUtils.FetchGameObjectByTag(Tags.BULLET_CONTAINER);
    }

    protected override void Attack(Vector2 direction) {
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
        Physics2D.IgnoreCollision(myCollider, bullet.GetComponent<Collider2D>());
        // dirty approach here.
        //bulletComp.SetParentCollider(myCollider);
    }
}
