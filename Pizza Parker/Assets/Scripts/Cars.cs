using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Forces game object to have rigidbody2D component
[RequireComponent(typeof(Rigidbody))]
public class Cars : MonoBehaviour
{
    Rigidbody rb;

    public Vector3 dir = Vector3.forward;
    public float speed = 5;
    public float knockback = 10;
	private float stunTime;
	private float stunCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

		stunTime = 1f / speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (stunCounter <= 0f)
        {
            if (speed != 0)
            {
                rb.velocity = dir * speed;


            }
            //If dir or speed = 0, car will stop moving
        }
        else
        {
            stunCounter -= Time.deltaTime;
            //Add decceleration?
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //Layer 6 is player layer
        if (collision.gameObject.layer == 6)
        {
            //variable for knockback direction based on collision point
            Vector3 knockbackDir = (collision.GetContact(0).point - transform.position).normalized;
            collision.rigidbody.velocity = knockbackDir * knockback;
			collision.gameObject.GetComponent<Player>().stunned = stunTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {

            if (other.gameObject.GetComponent<Player>().IsAttacking())
            {
                rb.velocity = other.transform.forward * 5;
                stunCounter = 5;
            }
        }
    }
}
