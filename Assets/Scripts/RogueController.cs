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

        if (control.movementControl != null) {
            // ----------------------------------------------------
            // 2. APPLY MOVEMENT
            // ----------------------------------------------------
            isMoving = Move((int) control.movementControl.movement.x,
                            (int) control.movementControl.movement.y);


            // ------------------------------------------------------
            // 3. UPDATE ORIENTATION
            // -------------------------------------------------------
            SetNewOrientation((int) control.movementControl.movement.x,
                              (int) control.movementControl.movement.y,
                              (int) control.movementControl.orientation.x,
                              (int) control.movementControl.orientation.y);
        }

        if (control.attackControl != null) {
            // -----------------------------------------------
            // 4. Switch weapon
            // -----------------------------------------------
            SelectNextWeapon(control.attackControl.selectNextWeapon);

            // --------------------------------------------------
            // 5. FIIIIIRREEEEEE
            // ---------------------------------------------------
            if (control.attackControl.shouldFire && GetCurrentWeapon() != null) {
                GetCurrentWeapon().Fire(control.attackControl.direction);
            }
        }

        // ----------------------------------------------------
        // 6. ANIMATIONS
        // ----------------------------------------------------

        if (isMoving && control.movementControl != null) {
            if (control.movementControl.orientation.x != 0 ||
                control.movementControl.orientation.y != 0) {
                animator.SetFloat("MoveX", control.movementControl.orientation.x);
                animator.SetFloat("MoveY", control.movementControl.orientation.y);
            } else {
                animator.SetFloat("MoveX", control.movementControl.movement.x);
                animator.SetFloat("MoveY", control.movementControl.movement.y);
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
        float increment = speed * Time.deltaTime;
        // add 0.5 (half size). TODO For bigger enemy, put that as attribube
        if (horizontal > 0 && dungeon.CanGoEast(basePosition, increment+0.5f)) {
            basePosition.x += increment;
        }

        if (horizontal < 0 && dungeon.CanGoWest(basePosition, increment+0.5f)) {
            basePosition.x -= increment;
        }

        if (vertical > 0 && dungeon.CanGoNorth(basePosition, increment+0.5f)) {
             basePosition.y += increment;
        }

        if (vertical < 0 && dungeon.CanGoSouth(basePosition, increment+0.5f)) {
            basePosition.y -= increment;

        }
        transform.position = basePosition;

        return vertical != 0 || horizontal != 0;
    }

    // ----------------------------------------------------------------
    // HEALTH AND HITS
    // ----------------------------------------------------------------

    // go is this very same gameobject.
    private void OnNoHp(GameObject go) {
        Destroy(gameObject);
    }

    // ----------------------------------------------------------------
    // WEAPON SELECTION
    // ----------------------------------------------------------------
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
                weapons[i].IsSelected();
            } else {
                weapons[i].gameObject.SetActive(false);
            }
        }
    }
}
