using UnityEngine;

/*
  Just keep track of how many time an object, player...
  can be hit before it dies
*/
public class Health: MonoBehaviour {

    [SerializeField]
    private int health;

    [SerializeField]
    private int maxHealth;

    [SerializeField]
    private int armor;

    // Event sent when we have no more HP.
    // Will be used by controller to destroy the gameobject
    // by UI to display game over if player dies
    // by stats system to count how many enemy you killed... and so on
    public delegate void NoHpAction(GameObject go);
    public event NoHpAction OnNoHp;

    public delegate void GotHitAction(Health healthComp);
    public event GotHitAction OnHit;

    /*
      Can Hit with negative damage (i.e. heal)
      */
    public void Hit(int damage) {
        // Reduce the hit points
        health -= damage;

        if (health <= 0) {
            // emit only if delegates
            if (OnNoHp != null) {
                OnNoHp(gameObject);
            }
        }

        if (OnHit != null) {
            OnHit(this);
        }
    }

    public int CurrentHealth() {
        return health;
    }

    public int MaxHealth() {
        return maxHealth;
    }
}
