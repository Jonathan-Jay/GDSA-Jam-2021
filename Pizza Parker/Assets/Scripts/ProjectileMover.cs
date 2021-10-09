using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    public float speed = 50f;
    public float hitOffset = 0.1f;
    public GameObject hit;
    public GameObject flash;
    public GameObject[] Detached;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        #region Start Flash
        if (flash != null)
        {
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null)
            {
                Destroy(flashInstance, flashPs.main.duration);
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }
        #endregion
    }

    void FixedUpdate ()
    {
        rb.velocity = transform.forward * speed;
	}

    void OnCollisionEnter(Collision collision)
    {
        // TODO JJ: Add player collision 

        // TODO Michael: Add car collision 

        #region Ricochet
        //ricochet
        if (collision.collider.CompareTag("Wall") || collision.collider.CompareTag("Car"))
        {
            ContactPoint contact = collision.contacts[0];

            //ricochet code
            Vector3 ricochetDir = Vector3.Reflect(transform.forward, contact.normal);
            float newRot = 90 - Mathf.Atan2(ricochetDir.z, ricochetDir.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, newRot, 0);

            //ricochet sparks
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point + contact.normal * hitOffset;

            var hitInstance = Instantiate(hit, pos, rot);
            hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0);

            var hitPs = hitInstance.GetComponent<ParticleSystem>();
            if (hitPs != null)
            {
                Destroy(hitInstance, 1);
            }
            else
            {
                var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitInstance, 1);
            }

        }
        foreach (var detachedPrefab in Detached)
        {
            if (detachedPrefab != null)
            {
                detachedPrefab.transform.parent = null;
            }
        }
        #endregion
    }
}
