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
            animator.SetFloat("MoveX", horizontal);
            animator.SetFloat("MoveY", vertical);
            animator.SetFloat("LastMoveX", horizontal);
            animator.SetFloat("LastMoveY", vertical);
            animator.SetBool("PlayerMoving", true);
        } else {
            animator.SetBool("PlayerMoving", false);
        }
        transform.position = basePosition;
	}
}
