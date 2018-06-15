using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 5f;

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

    void Start() {
        animator = GetComponent<Animator>();
        isMoving = false;

        // by default, oriented down
        orientation = new Vector2(0, -1);

        gun = GetComponent<Weapon>();
    }

	// Update is called once per frame
	void Update () {

        // ------------------------------------------------------
        // 1. GET THE INPUT
        // ------------------------------------------------------
        int horizontal = (int) Input.GetAxisRaw(InputConstants.HORIZONTAL);
        int vertical = (int) Input.GetAxisRaw(InputConstants.VERTICAL);
        // If face buttons are pressed, we are going to use them for orientation
        // instead of just walking.
        int faceHorizontal = (int) Input.GetAxisRaw(InputConstants.FACE_HORIZONTAL);
        int faceVertical = (int) Input.GetAxisRaw(InputConstants.FACE_VERTICAL);
        int fire = (int) Input.GetAxisRaw(InputConstants.FIRE);

        // ----------------------------------------------------
        // 2. APPLY MOVEMENT
        // ----------------------------------------------------
        isMoving = Move(horizontal, vertical);


        // ------------------------------------------------------
        // 3. UPDATE ORIENTATION
        // -------------------------------------------------------
        SetNewOrientation(horizontal, vertical, faceHorizontal, faceVertical);

        // --------------------------------------------------
        // 4. FIIIIIRREEEEEE
        // ---------------------------------------------------
        if (fire != 0 && gun != null) {
            gun.Fire(orientation);
        }

        // ----------------------------------------------------
        // 5. ANIMATIONS
        // ----------------------------------------------------

        if (isMoving) {
            if (faceHorizontal != 0 || faceVertical != 0) {
                animator.SetFloat("MoveX", faceHorizontal);
                animator.SetFloat("MoveY", faceVertical);
            } else {
                animator.SetFloat("MoveX", horizontal);
                animator.SetFloat("MoveY", vertical);
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
        if (horizontal > 0) {
            basePosition.x += speed * Time.deltaTime;
        }

        if (horizontal < 0) {
            basePosition.x -= speed * Time.deltaTime;
        }

        if (vertical > 0) {
             basePosition.y += speed * Time.deltaTime;
        }

        if (vertical < 0) {
            basePosition.y -= speed * Time.deltaTime;

        }
        transform.position = basePosition;

        return vertical != 0 || horizontal != 0;
    }

}
