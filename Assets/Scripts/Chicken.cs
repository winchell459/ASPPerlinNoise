using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : MonoBehaviour
{
    public float walkSpeed = 5;
    public float walkAcceleration = 1;
    public float deceleration = 1;
    public float runSpeed = 10;
    public float runAcceleration = 2;

    public float walkTime = 3;
    public float runTime = 3;
    public float eatTime = 2;
    public float headTurnTime = 1;

    private float stateStartTime = float.MinValue;

    public Animator anim;
    public Rigidbody rb;

    public float health = 5;
    public float birthRate = 5;
    private float lastBirth = float.MinValue;
    public enum States
    {
        idle,
        headTurning,
        walking,
        running,
        eating
    }
    public States state;

    // Start is called before the first frame update
    void Start()
    {
        stateStartTime = Time.time;
        lastBirth = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        HandleStates();
    }

    void HandleStates()
    {
        switch (state)
        {
            case States.idle:
                SetStates(false, false, false, false);
                Move(0, -deceleration);
                if (Time.time > stateStartTime + headTurnTime) SetRandomState();
                break;
            case States.headTurning:
                SetStates(false, false, false, true);
                Move(0, -deceleration);
                if (Time.time > stateStartTime + headTurnTime) SetRandomState();
                break;
            case States.walking:
                SetStates(true, false, false, false);
                Move(walkSpeed, walkAcceleration);
                if (Time.time > stateStartTime + walkTime) SetRandomState();
                break;
            case States.running:
                SetStates(false, true, false, false);
                Move(runSpeed, runAcceleration);
                if (Time.time > stateStartTime + runTime) SetRandomState();
                break;
            case States.eating:
                SetStates(false, false, true, false);
                Move(0, -deceleration);
                if (Time.time > stateStartTime + eatTime) SetRandomState();
                break;
            default:
                SetStates(false, false, false, false);
                break;

        }
    }
    private void SetRandomState()
    {
        int rand = Random.Range(0, 5);
        stateStartTime = Time.time;
        state = (States)rand;

        if (state == States.walking) Turn(true);
    }
    private void SetStates(bool walk, bool run, bool eat, bool turnHead)
    {
        anim.SetBool("Walk", walk);
        anim.SetBool("Run", run);
        anim.SetBool("Eat", eat);
        anim.SetBool("Turn Head", turnHead);
    }

    public void Hit(float damage)
    {
        health -= damage;
        if(health > 0)
        {
            state = States.running;
            stateStartTime = Time.time;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Turn(bool random)
    {
        if (random)
        {
            float dirAngle = Random.Range(0, 360);
            transform.eulerAngles += Vector3.up * dirAngle;
        }
    }

    private void Move(float target, float acc)
    {
        float vel = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
        //Debug.Log(vel);
        if (target < vel && acc > 0) rb.velocity = transform.forward * target + rb.velocity.y * Vector3.up;
        else if (target < vel && acc < 0) rb.AddForce(acc * transform.forward);
        else if (target > vel && acc > 0) rb.AddForce(acc * transform.forward);
        else if (target > vel && acc < 0) rb.velocity = transform.forward * target + rb.velocity.y * Vector3.up;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Chicken>() && lastBirth + birthRate < Time.time)
        {
            Duplicate();
        }
    }

    public void Duplicate()
    {
        lastBirth = Time.time;
        Instantiate(gameObject, transform.position + transform.forward *(-1), Quaternion.identity);

    }
}
