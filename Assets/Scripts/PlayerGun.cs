using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : PlayerPointer
{
    public GameObject projectilePrefab;
    public float projectileVelocity = 10;
    public GameObject muzzle;
    public float muzzleLength = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.name);
        SetPointer();
        if (targetFound && inputManager.rightTriggerUp() || Input.GetMouseButtonUp(0))
        {
            GameObject projectile = Instantiate(projectilePrefab, muzzle.transform.position + muzzle.transform.forward * muzzleLength, Quaternion.identity);
            //projectile.transform.position = muzzle.transform.position + muzzle.transform.forward * muzzleLength;
            projectile.GetComponent<Rigidbody>().velocity = muzzle.transform.forward * projectileVelocity;
            projectile.transform.forward = muzzleDirection();
        }
    }

    Vector3 muzzleDirection()
    {
        return (RightHand_target.position - transform.position).normalized;
    }
}
