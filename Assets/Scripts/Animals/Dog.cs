
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : Animal
{
    public float deceleration = 1;
    public float walkSpeed = 10;
    public float runSpeed = 20;
    public float runAcceleration = 200;

    public float walkTime = 3;
    public float runTime = 3;
    public float idleTime = 3;

    public float huntingRadius = 20;
    public float attackRadius = 3;
    public float attackWalkRadius = 2;
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
        spawnPoint = transform.position;
        stateStartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMapFalloff();
        if (!dead) HandleHunting();
        HandleStates();
        HandleAudio();
    }

    private void HandleStates()
    {
        switch (state)
        {
            case States.idle:
                SetStates(false, false, 0, false);
                Move(0, -deceleration);
                if (Time.time > stateStartTime + idleTime) SetRandomState();
                break;
            case States.walk:
                SetStates(false, false, 0.5f, false);
                Move(walkSpeed, runAcceleration);
                if (Time.time > stateStartTime + walkTime) SetRandomState();
                break;
            case States.run:
                SetStates(false, false, 1.1f, false);
                Move(runSpeed, runAcceleration);
                if (Time.time > stateStartTime + runTime) SetRandomState();
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
                if (dead)
                {
                    SetStates(true, false, 0, true);
                    state = States.die;
                }
                else
                {
                    SetStates(true, false, 0, false);
                    state = States.idle;
                }
                
                break;
            case States.hitWalk:
                if (dead)
                {
                    SetStates(true, false, 0.5f, true);
                    state = States.die;
                }
                else
                {
                    SetStates(true, false, 0.5f, false);
                    state = States.walk;
                }
                
                break;
            case States.hitRun:
                if (dead)
                {
                    SetStates(true, false, 1.1f, true);
                    state = States.die;

                }
                else
                {
                    SetStates(true, false, 1.1f, false);
                    state = States.run;
                }
                
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
        anim.SetBool("attack", attack);
        anim.SetFloat("velocity", velocity);
        anim.SetBool("die", die);
    }

    private void SetRandomState()
    {
        int rand = random.Next(0, 3);
        stateStartTime = Time.time;
        state = (States)rand;

        if (state == States.run) Turn(true, transform.forward);
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
        else if (preyDistance < attackWalkRadius)
        {
            state = States.attackWalk;
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
            if(!animal.dead && animal.animalType == animalType)
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

    public override void AttackAnimTrigger()
    {
        if (prey)
        {
            float preyDistance = Vector3.Distance(prey.transform.position, transform.position);
            bool kill = false;
            if (preyDistance < attackIdleRadius)
            {
                kill = prey.Hit(2, transform);
            }
            else if (preyDistance < attackRadius)
            {
                kill = prey.Hit(1, transform);
            }

            
        }
        else
        {
            SetRandomState();
        }

    }

    protected override void DieState()
    {
        if (state == States.attackIdle || state == States.idle || state == States.hitIdle) state = States.hitIdle;
        if (state == States.attackWalk || state == States.walk || state == States.hitWalk) state = States.hitWalk;
        if (state == States.attackRun || state == States.run || state == States.hitRun) state = States.hitRun;
        
        //Destroy(GetComponent<Rigidbody>());
        //GetComponent<Collider>().enabled = false;
    }
}
