using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 20);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<Animal>())
        {
            collision.transform.GetComponent<Animal>().Hit(1);
        }
        Destroy(gameObject);
    }
}
