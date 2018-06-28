using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
  Sword has a melee attack. It relies on a box collider to determine
  the hit box so the weapon gameobject should have one.
 */
[RequireComponent(typeof(BoxCollider2D))]
public class Sword: Weapon {

    [SerializeField]
    private float swordAnimationTime;

    [SerializeField]
    private int damage;

    private BoxCollider2D swordHitBox;

    private static Vector2 northOffset = new Vector2(0, 0.5f);
    private static Vector2 southOffset = new Vector2(0, -0.5f);
    private static Vector2 eastOffset = new Vector2(0.5f, 0);
    private static Vector2 westOffset = new Vector2(-0.5f, 0);

    private static Vector2 northSouthSize = new Vector2(1f, 0.5f);
    private static Vector2 eastWestSize = new Vector2(0.5f, 1f);

    public void Start() {
        base.Start();
        swordHitBox = GetComponent<BoxCollider2D>();
    }

    public void OnEnable() {
        //swordHitBox.enabled = false;
    }

    public override void IsSelected() {
        swordHitBox.enabled = false;
    }

    protected override void Attack(Vector2 direction) {
        // Depending on the direction, will change the collider size and offset.
        if (direction.x > 0) {
            // east
            swordHitBox.offset = eastOffset;
            swordHitBox.size = eastWestSize;
        } else if (direction.x < 0) {
            // west
            swordHitBox.offset = westOffset;
            swordHitBox.size = eastWestSize;
        } else if (direction.y > 0) {
            // north
            swordHitBox.offset = northOffset;
            swordHitBox.size = northSouthSize;
        } else if (direction.y < 0) {
            // south
            swordHitBox.offset = southOffset;
            swordHitBox.size = northSouthSize;
        } else {
            // ERROR
        }

        StartCoroutine(SwingSword(direction));
    }

    private IEnumerator SwingSword(Vector2 direction) {
        swordHitBox.enabled = true;
        yield return new WaitForSeconds(swordAnimationTime);
        swordHitBox.enabled = false;
    }

    protected override float GetTimeBetweenAttacks() {
        return swordAnimationTime+0.1f;
    }

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Hit something with sword");

        Health health = other.gameObject.GetComponent<Health>();
        if (health != null) {
            health.Hit(damage);
        }

    }
}
