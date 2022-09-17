using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public Vector3 spawnPoint;
    public AnimalType animalType;
    public enum AnimalType { chicken, spider, dog}
    protected float stateStartTime = float.MinValue;

    public Animator anim;
    public Rigidbody rb;
    public AIHud aiHud;

    public int experience = 1;
    public float health = 5;
    public float food = 0;
    public float birthRate = 5;
    protected float lastBirth = float.MinValue;
    public int maxPopulationDensity = 5;
    public float populationRadius = 5;

    public AudioSource audioSource;
    public AudioClip[] audioVariety;
    public float audioRate = 5;
    protected float lastAudioTime = float.MinValue;

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

    protected void Turn(bool random, Vector3 direction)
    {
        if (random)
        {
            float dirAngle = this.random.Next(0,360);//Random.Range(0, 360);
            transform.eulerAngles += Vector3.up * dirAngle;
        }
        else
        {
            transform.forward = direction;
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

    public bool dead = false;
    public bool Hit(float damage, Transform source)
    {
        if (health <= 0) return false;
        health -= damage;
        if (health > 0)
        {
            HitState(source);
            stateStartTime = Time.time;
            return false;
        }
        else
        {
            dead = true;
            FindObjectOfType<PlayerGrabber>().animalSelection.RemoveAnimal(this);
            DieState();
            return true;
        }
    }
    public virtual void AttackAnimTrigger() { }

    protected virtual void HitState(Transform source)
    {
        //state = States.running;
    }

    protected virtual void DieState()
    {
        StartCoroutine(Die(0));
    }

    protected virtual IEnumerator Die(float wait)
    {
        yield return null;
        Destroy(gameObject,wait);
    }

    protected virtual void HandleAudio()
    {
        if(lastAudioTime + audioRate < Time.time)
        {
            Debug.Log("Audio Time");
            lastAudioTime = Time.time;
            if(random.Next(0,3)< 1)
            {
                Debug.Log("Playing 0");
                if (audioVariety.Length > 0)
                {
                    Debug.Log("Playing");
                    audioSource.clip = audioVariety[random.Next(0, audioVariety.Length)];
                    audioSource.Play();
                }
            }
        }
    }
    float resetYOffset = 1;
    protected virtual void HandleMapFalloff()
    {
        if (transform.position.y < -10)
        {
            transform.position = spawnPoint + Vector3.up * resetYOffset;
            resetYOffset *= 1.1f;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            HandleWater(other);
        }
    }
    protected virtual void HandleWater(Collider water)
    {

        Move(-rb.mass * 10, -rb.mass * 100);
    }
}

public class Stats
{
    public int level = 1;
    public int strength; //heath and damage
    public int dexterity; //speed and agility
    public int constitution; //desirablity or gestation duration
    public int intellegence; //memory of food and water locations and rejecting mates
    public int wisdom; //sensory distance and perception of spiders before attack
    public int charisma; //desirablility
}