using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 5f;

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
        transform.position = basePosition;
	}
}
