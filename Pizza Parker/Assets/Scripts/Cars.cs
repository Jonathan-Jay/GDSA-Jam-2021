using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Forces game object to have rigidbody2D component
[RequireComponent(typeof(Rigidbody2D))]
public class Cars : MonoBehaviour
{
    Rigidbody2D rb;

    public Vector2 dir = Vector2.up;
    public float speed = 5;
    public float knockback = 10;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (speed != 0)
        {
            rb.velocity = dir * speed;
            
        }
        //If dir or speed = 0, car will stop moving
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Layer 6 is player layer
        if (collision.gameObject.layer == 6)
        {
            //variable for knockback direction based on collision point
            Vector2 knockbackDir = (collision.GetContact(0).point - (Vector2)transform.position).normalized;
            collision.rigidbody.velocity = knockbackDir * knockback;

        }
    }
}
