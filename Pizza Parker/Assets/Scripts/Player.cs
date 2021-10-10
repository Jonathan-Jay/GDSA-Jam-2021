using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
	public Transform tempShape;
	bool attacked = false;

	BoxCollider paddle;
	public Vector3 backPos = new Vector3(0f, 0f, 0.6f);
	public Vector3 backShape = new Vector3(1f, 1f, 0.2f);
	public Vector3 hitPos = new Vector3(0f, 0f, -1.2f);
	public Vector3 hitShape = new Vector3(1.2f, 1.2f, 1.2f);
	Rigidbody rb;
	Animator anim;


	public float speed = 5f;
	public float stunned;

	private int playerID;
	private static int players = 1;
	float root2 = 1f / Mathf.Sqrt(2f);

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
		rb.useGravity = false;
		anim = GetComponentInChildren<Animator>();


		playerID = players++;
		stunned = 0f;

		//get the paddle collider
		BoxCollider[] colliders = GetComponents<BoxCollider>();
		foreach (BoxCollider box in colliders) {
			if (box.isTrigger) {
				paddle = box;
				break;
			}
		}
		paddle.center = backPos;
		paddle.size = backShape;

		tempShape.localPosition = backPos;
		tempShape.localScale = backShape;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = new Vector3(
			Input.GetAxisRaw("Horizontal " + playerID),
			0f,
			Input.GetAxisRaw("Vertical " + playerID)
		);

		if (anim.GetInteger("State") != 2) {
			//don't move if too slow, this fixes sliding
			if (direction.magnitude > 0.25f) {
				anim.SetInteger("State", 1);

				//set rotation and movement
				rb.rotation = Quaternion.AngleAxis(90f - Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg,
					Vector3.up);

				//don't move if stunned, but allow rotation i guess
				if (stunned > 0) {
					stunned -= Time.deltaTime;
				}	//dont move if attacking
				else {
					//normalize stuff
					if (Mathf.Abs(direction.x) > 0.5f && Mathf.Abs(direction.z) > 0.5f) {
						direction *= root2;
					}
					rb.velocity = direction * speed;
				}
			}
			else {
				anim.SetInteger("State", 0);
				rb.velocity = Vector3.zero;
			}
		}

		if (stunned <= 0) {
			//Swing
			if (Input.GetButtonDown("Swing " + playerID)) {
				//stop
				if (!attacked) {
					anim.SetInteger("State", 2);
					attacked = true;
					rb.velocity = Vector3.zero;

					tempShape.localPosition = hitPos;
					tempShape.localScale = hitShape;
					paddle.center = hitPos;
					paddle.size = hitShape;
				}
			}
			else if (attacked && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1) {
				anim.SetInteger("State", 0);
				attacked = false;

				tempShape.localPosition = backPos;
				tempShape.localScale = backShape;
				paddle.center = backPos;
				paddle.size = backShape;
			}
		}
		else {
			anim.SetInteger("State", 3);
		}
	}

	public bool IsAttacking() {
		return attacked;
	}
}
