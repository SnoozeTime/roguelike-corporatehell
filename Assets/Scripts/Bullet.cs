using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Bullet: MonoBehaviour {

    // Where the bullet is heading
    private Vector2 direction;

    private int damage = 1;
    public void SetDirection(Vector2 newDirection) {
        direction = newDirection;
    }

    // Speed of the bullet
    private float speed;
    public float Speed {
        get {return speed;}
        set {speed = value;}
    }

    private Collider2D parentCollider;
    public void SetParentCollider(Collider2D collider) {
        parentCollider = collider;
    }

    void Start() {
        speed = 7f;
    }

    void Update() {
        Vector2 currentPosition = transform.position;
        currentPosition += direction * speed * Time.deltaTime;
        transform.position = currentPosition;
    }

    void OnTriggerEnter2D(Collider2D other) {

        // Do not collider with other bullets.
        if (other.gameObject.layer == (int) Layer.BULLET) {
            return;
        }

        Health health = other.gameObject.GetComponent<Health>();
        if (health != null) {
            health.Hit(damage);
        }
        Destroy(gameObject);
    }
}
