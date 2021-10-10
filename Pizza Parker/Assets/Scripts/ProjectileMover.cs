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
        if (collision.collider.CompareTag("Player")) {
			CarManager.WinThing(collision.gameObject);
		}

		if (collision.collider.CompareTag("Car")) {
			collision.gameObject.GetComponent<Cars>().StopCar();
		}

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

	void OnTriggerEnter(Collider collider)
	{
		if (collider.CompareTag("Player")) {
			#region hit
			Quaternion rot;
			Vector3 pos;
			if (collider.gameObject.GetComponent<Player>().IsAttacking()) {
				//hit direction code
				transform.rotation = collider.transform.rotation;

				//sparks, same code as before, but adapted for this type of hit
				rot = transform.rotation;
				pos = transform.position - collider.transform.forward * hitOffset;
			}
			else {
				//bounce on back code
				Vector3 ricochetDir = Vector3.Reflect(transform.forward, Quaternion.AngleAxis(180f, Vector3.up) * collider.transform.forward);
				float newRot = 90 - Mathf.Atan2(ricochetDir.z, ricochetDir.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.AngleAxis(newRot, Vector3.up);

				//sparks, same code as before, but adapted for this type of hit
				rot = transform.rotation;	//collider transform is the normal
				pos = transform.position + collider.transform.forward * hitOffset;
			}

			var hitInstance = Instantiate(hit, pos, rot);
			hitInstance.transform.rotation = transform.rotation * Quaternion.Euler(0, 180f, 0);

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
			#endregion
		}
		foreach (var detachedPrefab in Detached)
		{
			if (detachedPrefab != null)
			{
				detachedPrefab.transform.parent = null;
			}
		}
	}
}
