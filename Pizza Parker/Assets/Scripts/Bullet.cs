using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(bullet, transform.position, transform.rotation);
    }

}
