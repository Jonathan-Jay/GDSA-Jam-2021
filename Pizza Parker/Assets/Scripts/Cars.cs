using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Forces game object to have rigidbody2D component
[RequireComponent(typeof(Rigidbody))]
public class Cars : MonoBehaviour
{
    Rigidbody rb;
	public Color dead;

    public Vector3 dir = Vector3.forward;
    public float speed = 5;
    public float playerKnockback = 10;
    public float carKnockback = 10;
	public float stunTime = 0.5f;
	private float stunCounter = 0;
	private bool stalled = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (stunCounter <= 0f)
        {
			//dont move if stalled
			if (stalled) {
				rb.velocity = Vector3.zero;
				return;
			}

            if (speed != 0)
            {
                rb.velocity = dir * speed;
            }
            //If dir or speed = 0, car will stop moving
        }
        else
        {
            stunCounter -= Time.deltaTime;
			rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime / carKnockback);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //Layer 6 is player layer
        if (collision.gameObject.layer == 6)
        {
            //variable for knockback direction based on collision point
            Vector3 knockbackDir = (collision.GetContact(0).point - transform.position).normalized;
            collision.rigidbody.velocity = knockbackDir * playerKnockback;
			collision.gameObject.GetComponent<Player>().stunned = stunTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {

            if (other.gameObject.GetComponent<Player>().IsAttacking())
            {
                rb.velocity = other.transform.forward * carKnockback;
                stunCounter = carKnockback;
            }
        }
    }

	public void StopCar() {
		if (!stalled) {
			stalled = true;
			rb.velocity = Vector3.zero;
			GetComponentInChildren<SpriteRenderer>().color = dead;
		}
	}
}
