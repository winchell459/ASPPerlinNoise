using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public AnimalType animalType;
    public enum AnimalType { chicken, spider}
    protected float stateStartTime = float.MinValue;

    public Animator anim;
    public Rigidbody rb;

    public float health = 5;
    public float birthRate = 5;
    protected float lastBirth = float.MinValue;
    public int maxPopulationDensity = 5;
    public float populationRadius = 5;

    protected int seed { get { return FindObjectOfType<Sebastian.MapGenerator>().seed; } }
    protected System.Random random
    {
        get
        {
            if(randomObj == null)
            {
                randomObj = new System.Random(seed);
            }
            return randomObj;
        }
    }
    private static System.Random randomObj;

    protected void Turn(bool random)
    {
        if (random)
        {
            float dirAngle = this.random.Next(0,360);//Random.Range(0, 360);
            transform.eulerAngles += Vector3.up * dirAngle;
        }
    }

    protected void Move(float target, float acc)
    {
        float vel = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
        //Debug.Log(vel);
        if (target < vel && acc > 0) rb.velocity = transform.forward * target + rb.velocity.y * Vector3.up;
        else if (target < vel && acc < 0) rb.AddForce(acc * transform.forward);
        else if (target > vel && acc > 0) rb.AddForce(acc * transform.forward);
        else if (target > vel && acc < 0) rb.velocity = transform.forward * target + rb.velocity.y * Vector3.up;
    }

    public bool Hit(float damage)
    {
        if (health <= 0) return false;
        health -= damage;
        if (health > 0)
        {
            HitState();
            stateStartTime = Time.time;
            return false;
        }
        else
        {
            FindObjectOfType<PlayerGrabber>().animalSelection.RemoveAnimal(this);
            DieState();
            return true;
        }
    }

    protected virtual void HitState()
    {
        //state = States.running;
    }

    protected virtual void DieState()
    {
        StartCoroutine(Die());
    }

    protected IEnumerator Die()
    {
        yield return null;
        Destroy(gameObject);
    }
}
