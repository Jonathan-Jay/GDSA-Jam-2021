using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
	Rigidbody2D rb;

	public float speed = 5f;
	private int playerID;
	private static int players = 1;

	float root2 = 1f / Mathf.Sqrt(2f);

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		playerID = players++;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = new Vector2(
			Input.GetAxisRaw("Horizontal " + playerID),
			Input.GetAxisRaw("Vertical " + playerID)
		);

		if (direction != Vector2.zero) {
			//set rotation
			rb.rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;

			//normalize stuff
			if (Mathf.Abs(direction.x) > 0.5f && Mathf.Abs(direction.y) > 0.5f) {
				direction *= root2;
			}
			rb.velocity = direction * speed;
		}
    }
}
