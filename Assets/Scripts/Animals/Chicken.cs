using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : Animal
{
    public float walkSpeed = 5;
    public float walkAcceleration = 3;
    public float deceleration = 1;
    public float runSpeed = 10;
    public float runAcceleration = 5;

    public float walkTime = 3;
    public float runTime = 3;
    public float eatTime = 2;
    public float headTurnTime = 1;


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
        int rand = random.Next(0, 5);//Random.Range(0, 5);
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



    protected override void HitState()
    {
        state = States.running;
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
        Chicken[] chickens = FindObjectsOfType<Chicken>();
        bool duplicate = false;
        if(chickens.Length < maxPopulationDensity - 1)
        {
            duplicate = true;
        }
        else
        {
            int radiusCount = maxPopulationDensity + 1;
            foreach(Chicken chicken in chickens)
            {
                if(Vector3.Distance(transform.position, chicken.transform.position) < populationRadius)
                {
                    radiusCount -= 1;
                    if (radiusCount < 0) break;
                }

                
            }
            if (radiusCount > 0) duplicate = true;
        }

        if (duplicate)
        {
            lastBirth = Time.time;
            Animal animal = Instantiate(gameObject, transform.position + transform.forward * (-1), Quaternion.identity).GetComponent<Animal>();
            FindObjectOfType<PlayerGrabber>().animalSelection.AddAnimal(animal);
            
        }
        

    }
}
