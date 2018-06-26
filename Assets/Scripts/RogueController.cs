using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
  Controller is used by Player and enemy. Each frame, it will
  get an orientation and a direction to move from somewhere else (e.g. AI, or input)
  as well as a decision to fire.
 */
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Health))]
public class RogueController : MonoBehaviour {

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private ControllerBehaviour behaviour;

    private Animator animator;

    // true if player is moving
    private bool isMoving;

    // true if player can shoot
    private bool canShoot;
    private float reloadTime;

    // orientation of the player
    private Vector2 orientation;

    // Stuff to shoot with
    private Weapon gun;
    private List<Weapon> weapons;
    private int currentWeaponIndex;
    // The current dungeon.
    private Dungeon dungeon;

    // Access to the properties
    public Vector2 Orientation {
        get {return orientation;}
        set {orientation = value;}
    }

    // --------------------------------------
    void Start() {

        // Weapon is a child gameObject. The reason is that it can have its own
        // animations and colliders
        weapons = FetchUtils.FetchChildrenWithComponent<Weapon>(transform);
        currentWeaponIndex = 0;
        UpdateWeapon();

        animator = GetComponent<Animator>();
        isMoving = false;

        // by default, oriented down
        orientation = new Vector2(0, -1);

        GetComponent<Health>().OnNoHp += OnNoHp;

        dungeon = FetchUtils.FetchGameObjectByTag(Tags.DUNGEON).GetComponent<Dungeon>();
    }

	// Update is called once per frame
	void Update () {

        // ------------------------------------------------------
        // 1. GET THE INPUT
        // ------------------------------------------------------
        Control control = behaviour.GetControls();

        // ----------------------------------------------------
        // 2. APPLY MOVEMENT
        // ----------------------------------------------------
        isMoving = Move(control.horizontalMovement,
                        control.verticalMovement);


        // ------------------------------------------------------
        // 3. UPDATE ORIENTATION
        // -------------------------------------------------------
        SetNewOrientation(control.horizontalMovement,
                          control.verticalMovement,
                          control.horizontalOrientation,
                          control.verticalOrientation);

        // -----------------------------------------------
        // 4. Switch weapon
        // -----------------------------------------------
        SelectNextWeapon(control.selectNextWeapon);

        // --------------------------------------------------
        // 5. FIIIIIRREEEEEE
        // ---------------------------------------------------
        if (control.shouldFire && GetCurrentWeapon() != null) {
            GetCurrentWeapon().Fire(orientation);
        }

        // ----------------------------------------------------
        // 6. ANIMATIONS
        // ----------------------------------------------------

        if (isMoving) {
            if (control.horizontalOrientation != 0 || control.verticalOrientation != 0) {
                animator.SetFloat("MoveX", control.horizontalOrientation);
                animator.SetFloat("MoveY", control.verticalOrientation);
            } else {
                animator.SetFloat("MoveX", control.horizontalMovement);
                animator.SetFloat("MoveY", control.verticalMovement);
            }
        }

        animator.SetFloat("LastMoveX", orientation.x);
        animator.SetFloat("LastMoveY", orientation.y);
        animator.SetBool("PlayerMoving", isMoving);
	}

    private void SetNewOrientation(int horizontal,
                                   int vertical,
                                   int faceHorizontal,
                                   int faceVertical) {

        // orientation will help determine in what direction the
        // bullets will be shot, and what animation to play.
        if (faceHorizontal != 0) {
            orientation.x = faceHorizontal;
            orientation.y = 0;
        }

        if (faceVertical != 0) {
            orientation.y = faceVertical;
            orientation.x = 0;
        }

        if (isMoving) {
            if (faceHorizontal == 0 && faceVertical == 0) {
                // orientation is the direction of the movement
                // if nothing else is specified.

                if (horizontal != 0) {
                    orientation.x = horizontal;
                    orientation.y = 0;
                }

                if (vertical != 0) {
                    orientation.x = 0;
                    orientation.y = vertical;
                }
            }
        }
    }

    // move the player and returns true if in movement.
    private bool Move(int horizontal, int vertical) {

        Vector2 basePosition = transform.position;
        if (horizontal > 0 && dungeon.CanGoEast(basePosition)) {
            basePosition.x += speed * Time.deltaTime;
        }

        if (horizontal < 0 && dungeon.CanGoWest(basePosition)) {
            basePosition.x -= speed * Time.deltaTime;
        }

        if (vertical > 0 && dungeon.CanGoNorth(basePosition)) {
             basePosition.y += speed * Time.deltaTime;
        }

        if (vertical < 0 && dungeon.CanGoSouth(basePosition)) {
            basePosition.y -= speed * Time.deltaTime;

        }
        transform.position = basePosition;

        return vertical != 0 || horizontal != 0;
    }

    // go is this very same gameobject.
    private void OnNoHp(GameObject go) {
        Destroy(gameObject);
    }

    private Weapon GetCurrentWeapon() {
        if (currentWeaponIndex >= weapons.Count) {
            return null;
        }

        return weapons[currentWeaponIndex];
    }

    private void SelectNextWeapon(int direction) {
        if (direction == 0 || weapons.Count == 0) {
            return;
        }
        int offset = direction > 0 ? 1 : -1;

        int nextWeaponIndex;
        if (currentWeaponIndex == 0 && offset == -1) {
            nextWeaponIndex = weapons.Count -1;
        } else {
            nextWeaponIndex = (currentWeaponIndex + offset) % weapons.Count;
        }

        if (nextWeaponIndex != currentWeaponIndex) {
            // Disable all weapon but the current
            currentWeaponIndex = nextWeaponIndex;
            UpdateWeapon();
        }
    }

    private void UpdateWeapon() {
        for (int i = 0; i < weapons.Count; i++) {
            if (i == currentWeaponIndex) {
                weapons[i].gameObject.SetActive(true);
            } else {
                weapons[i].gameObject.SetActive(false);
            }
        }
    }
}
