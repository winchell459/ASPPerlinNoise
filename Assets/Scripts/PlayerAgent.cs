using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAgent : MonoBehaviour
{
    public float power = 5;
    public Rigidbody rb;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        rb.AddForce(new Vector3(horizontal * power, 0, vertical * power));
    }
}
