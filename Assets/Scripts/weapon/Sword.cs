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

    public void Start() {
        base.Start();
        swordHitBox = GetComponent<BoxCollider2D>();
    }

    public void OnEnable() {
        swordHitBox.enabled = false;
    }

    protected override void Attack(Vector2 direction) {
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
