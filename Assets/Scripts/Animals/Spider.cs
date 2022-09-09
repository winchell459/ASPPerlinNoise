using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Animal
{
    public float deceleration = 1;
    public float runSpeed = 10;
    public float runAcceleration = 5;

    public float walkTime = 3;
    public float runTime = 3;
    public float idleTime = 3;

    public States state;
    public enum States
    {
        idle,
        run,
        attackIdle,
        attackRun,
        hitIdle,
        hitRun,
        hitDie,
        die
    }

    public float huntingRadius = 20;
    public float attackRadius = 3;
    public float attackIdleRadius = 1;

    public int killCountBirthRate = 5;
    protected int killCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        stateStartTime = Time.time;
        lastBirth = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        HandleHunting();
        HandleStates(); 
    }
    Animal prey = null;
    private void HandleHunting()
    {
        prey = FindClosestAnimal();
        float preyDistance = float.MaxValue;
        if (prey )
        {
            preyDistance = Vector3.Distance(prey.transform.position, transform.position);
        }

        if(preyDistance < attackIdleRadius)
        {
            state = States.attackIdle;
        }else if(preyDistance < attackRadius)
        {
            state = States.attackRun;
        }
        else if(preyDistance < huntingRadius)
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

    Animal FindClosestAnimal()
    {
        Animal prey = null;
        Chicken[] chickens = FindObjectsOfType<Chicken>();
        foreach(Chicken chicken in chickens)
        {
            if (prey)
            {
                if(Vector3.Distance(transform.position, prey.transform.position) > Vector3.Distance(transform.position, chicken.transform.position))
                {
                    prey = chicken;
                }
            }
            else
            {
                prey = chicken;
            }
        }
        return prey;
    }

    private void HandleStates()
    {
        switch (state)
        {
            case States.idle:
                SetStates(false, false, false, false);
                Move(0, -deceleration);
                if (Time.time > stateStartTime + idleTime) SetRandomState();
                break;
            case States.run:
                SetStates(true, false, false, false);
                Move(runSpeed, runAcceleration);
                if (Time.time > stateStartTime + runTime) SetRandomState();
                break;
            case States.attackIdle:
                SetStates(false, true, false, false);
                Move(0, -deceleration);
                break;
            case States.attackRun:
                SetStates(true, true, false, false);
                Move(0, -deceleration);
                break;
            case States.hitIdle:
                SetStates(false, false, true, false);
                Move(0, -deceleration);
                break;
            case States.hitRun:
                SetStates(true, false, true, false);
                Move(0, -deceleration);
                break;
            case States.hitDie:
                SetStates(false, false, true, true);
                Move(0, -deceleration);
                break;
            case States.die:
                SetStates(false, false, false, true);
                Move(0, -deceleration);
                break;
        }
    }

    private void SetStates(bool run, bool attackTrigger, bool hitTrigger, bool die)
    {
        anim.SetBool("run", run);
        anim.SetBool("die", die);
        if (attackTrigger) anim.SetTrigger("attack");
        if (hitTrigger) anim.SetTrigger("hurt");
    }

    private void SetRandomState()
    {
        int rand = random.Next(0, 2);//Random.Range(0, 2);
        stateStartTime = Time.time;
        state = (States)rand;

        if (state == States.run) Turn(true);
    }

    public void AttackAnimTrigger()
    {
        if (prey)
        {
            float preyDistance = Vector3.Distance(prey.transform.position, transform.position);
            bool kill = false;
            if (preyDistance < attackIdleRadius)
            {
                kill = prey.Hit(2);
            }
            else if (preyDistance < attackRadius)
            {
                kill = prey.Hit(1);
            }

            if (kill) killCount += 1;

            if(killCount >= killCountBirthRate)
            {
                killCount = 0;
                Duplicate();
            }
        }
        else
        {
            SetRandomState();
        }
        
    }

    protected void Duplicate()
    {
        lastBirth = Time.time;
        Animal animal = Instantiate(gameObject, transform.position + transform.forward * (-1), Quaternion.identity).GetComponent<Animal>();
        FindObjectOfType<PlayerGrabber>().animalSelection.AddAnimal(animal);
        
    }
}
