
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

    public float maxHealth = 10;
    public float maxFood = 10;
    public Transform player;

    public Vector3 lastPos;
    public float lastPosBuffer = 0.1f;
    public float lastPosWait = 1;
    private float lastPosStart;
    
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
        aiHud.Display();
        SetLastPos();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMapFalloff();
        if (!dead && lastPosStart + lastPosWait < Time.time)
        {
            HandleHunting();

        }
        HandleStates();
        HandleAudio();
        aiHud.Display();
        if (CheckStuckPos())
        {
            Debug.Log("Stuck turn");
            Turn(true, Vector3.zero);
            SetLastPos();
        }
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
    bool CheckStuckPos()
    {
        if (state == States.idle || state == States.hitIdle || state == States.attackIdle || state == States.attackRun || state == States.attackWalk || state == States.die) return false;
        if (lastPosStart + lastPosWait < Time.time && Vector3.Distance(lastPos, transform.position) < lastPosBuffer)
        {
            return true;
        }
        else return false;
    }

    void SetLastPos()
    {
        lastPosStart = Time.time;
        lastPos = transform.position;
    }

    void SetStates(bool hit, bool attack, float velocity, bool die)
    {
        if (hit) anim.SetTrigger("hit");
        anim.SetBool("attack", attack);
        anim.SetFloat("velocity", velocity);
        anim.SetBool("die", die);
    }

    bool followPlayer { get { return Vector3.Distance(player.transform.position, transform.position) > followPlayerDistance; } }
    [SerializeField]float followPlayerDistance = 30;
    private void SetRandomState()
    {
        int rand = random.Next(0, 3);
        stateStartTime = Time.time;
        state = (States)rand;

        if(state == States.idle)
        {
            if (health < maxHealth && food > 0)
            {
                food -= 1;
                health += 1;
            }
        } 
        if (state == States.walk) Turn(followPlayer? false:true, new Vector3(player.position.x - transform.position.x,0, player.position.z - transform.position.z).normalized);
        SetLastPos();
    }

    Animal prey = null;
    private void HandleHunting()
    {
        
        prey = CheckClosestPrey(FindClosestAnimal(AnimalType.spider));
        if(!prey && health < maxHealth && food <= maxFood /*&& random.Next(0, (int)(maxHealth - health) + 1) < 1*/)
        {
            prey = CheckClosestPrey(FindClosestAnimal(AnimalType.chicken));
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

    Animal CheckClosestPrey(Animal prey)
    {
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
            if (kill && prey.GetComponent<Chicken>())
            {
                food += 1;
                experience += prey.GetComponent<Animal>().experience;
            }
            else if (kill && prey.GetComponent<Spider>())
            {
                experience += prey.GetComponent<Animal>().experience;
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
