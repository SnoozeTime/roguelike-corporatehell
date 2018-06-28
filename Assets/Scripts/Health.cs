using UnityEngine;
using System.Collections;
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

    // Frame of invicibility?
    private bool canTakeDmg = true;
    private float invisibilityTime = 0.5f;

    // Event sent when we have no more HP.
    // Will be used by controller to destroy the gameobject
    // by UI to display game over if player dies
    // by stats system to count how many enemy you killed... and so on
    public delegate void NoHpAction(GameObject go);
    public event NoHpAction OnNoHp;

    public delegate void GotHitAction(Health healthComp);
    public event GotHitAction OnHit;

    public void OnEnable() {
        canTakeDmg = true;
    }

    /*
      Can Hit with negative damage (i.e. heal)
      */
    public void Hit(int damage) {
        if (canTakeDmg) {
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
    }

    /* When hit, we want to have a few second of invicibility in order
       not to get hit from the same attack (e.g. melee) too many times.

       This is not only for enemies and player, but also for destructible
       elements. So basically everything that can take damage.
    */
    private void WaitBeforeNextHit() {
        canTakeDmg = false;
        StartCoroutine(WaitAndEnableHit());
    }

    private IEnumerator WaitAndEnableHit() {
        yield return new WaitForSeconds(invisibilityTime);
        canTakeDmg = true;
    }
    public int CurrentHealth() {
        return health;
    }

    public int MaxHealth() {
        return maxHealth;
    }
}
