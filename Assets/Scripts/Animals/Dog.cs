
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : Animal
{
    public float deceleration = 1;
    public float runSpeed = 10;
    public float runAcceleration = 5;

    public float walkTime = 3;
    public float runTime = 3;
    public float idleTime = 3;

    public float huntingRadius = 20;
    public float attackRadius = 3;
    public float attackIdleRadius = 1;

    public enum States
    {
        idle,
        walk,
        run,
        attackIdle,
        attackWalk,
        attackRun,
        hitIdle,
        hitWalk,
        hitRun,
        dizzy,
        die,
        dieRecover
    }
    public States state;
    // Start is called before the first frame update
    void Start()
    {
        stateStartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        HandleHunting();
        HandleStates();
        HandleAudio();
    }

    private void HandleStates()
    {
        switch (state)
        {
            case States.idle:
                SetStates(false, false, 0, false);
                break;
            case States.walk:
                SetStates(false, false, 0.5f, false);
                break;
            case States.run:
                SetStates(false, false, 1.1f, false);
                
                break;
            case States.attackIdle:
                SetStates(false, true, 0, false);
                state = States.idle;
                break;
            case States.attackWalk:
                SetStates(false, true, 0.5f, false);
                state = States.walk;
                break;
            case States.attackRun:
                SetStates(false, true, 1.1f, false);
                state = States.run;
                break;
            case States.hitIdle:
                SetStates(true, false, 0, false);
                state = States.idle;
                break;
            case States.hitWalk:
                SetStates(true, false, 0.5f, false);
                state = States.walk;
                break;
            case States.hitRun:
                SetStates(true, false, 1.1f, false);
                state = States.run;
                break;
            //case States.dizzy:
            //    break;
            case States.die:
                SetStates(false, false, 0, true);
                break;
            case States.dieRecover:
                SetStates(false, false, 0, false);
                break;

        }
    }

    void SetStates(bool hit, bool attack, float velocity, bool die)
    {
        if (hit) anim.SetTrigger("hit");
        if (attack) anim.SetTrigger("attack");
        anim.SetFloat("velocity", velocity);
        anim.SetBool("die", die);
    }

    Animal prey = null;
    private void HandleHunting()
    {
        prey = FindClosestAnimal(AnimalType.spider);
        float preyDistance = float.MaxValue;
        if (prey)
        {
            preyDistance = Vector3.Distance(prey.transform.position, transform.position);
        }

        if (preyDistance < attackIdleRadius)
        {
            state = States.attackIdle;
        }
        else if (preyDistance < attackRadius)
        {
            state = States.attackRun;
        }
        else if (preyDistance < huntingRadius)
        {
            state = States.run;
            Vector3 forward = prey.transform.position - transform.position;
            transform.forward = new Vector3(forward.x, 0, forward.z);
        }
        else
        {
            prey = null;
        }
    }

    Animal FindClosestAnimal(Animal.AnimalType animalType)
    {
        Animal prey = null;
        Animal[] animals = FindObjectsOfType<Animal>();
        foreach (Animal animal in animals)
        {
            if(animal.animalType == animalType)
            {
                if (prey)
                {
                    if (Vector3.Distance(transform.position, prey.transform.position) > Vector3.Distance(transform.position, animal.transform.position))
                    {
                        prey = animal;
                    }
                }
                else
                {
                    prey = animal;
                }
            }
            
        }
        return prey;
    }
}
