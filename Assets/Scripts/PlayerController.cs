using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 5f;

    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
    }

	// Update is called once per frame
	void Update () {
        int horizontal = (int) Input.GetAxisRaw(InputConstants.HORIZONTAL);
        int vertical = (int) Input.GetAxisRaw(InputConstants.VERTICAL);

        int fire = (int) Input.GetAxisRaw(InputConstants.FIRE);

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

        if (horizontal != 0 || vertical != 0) {

            // If face buttons are pressed, we are going to use them for orientation
            // instead of just walking.
            int faceHorizontal = (int) Input.GetAxisRaw(InputConstants.FACE_HORIZONTAL);
            int faceVertical = (int) Input.GetAxisRaw(InputConstants.FACE_VERTICAL);

            if (faceHorizontal != 0 || faceVertical != 0) {
                animator.SetFloat("MoveX", faceHorizontal);
                animator.SetFloat("MoveY", faceVertical);
                animator.SetFloat("LastMoveX", faceHorizontal);
                animator.SetFloat("LastMoveY", faceVertical);
            } else {
                animator.SetFloat("MoveX", horizontal);
                animator.SetFloat("MoveY", vertical);
                animator.SetFloat("LastMoveX", horizontal);
                animator.SetFloat("LastMoveY", vertical);
            }
            animator.SetBool("PlayerMoving", true);
        } else {
            animator.SetBool("PlayerMoving", false);
        }
        transform.position = basePosition;
	}
}
