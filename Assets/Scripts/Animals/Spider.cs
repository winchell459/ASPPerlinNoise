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
        spawnPoint = transform.position;
        stateStartTime = Time.time;
        lastBirth = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMapFalloff();
        if (!dead)
        {
            if (!enemy || enemy.dead)
            {
                prey = FindClosestAnimal();
                HandleHunting(ref prey);
            }
            else 
            {
                HandleHunting(ref enemy);
            }

            
        }
        HandleStates();
    }
    Animal prey = null;
    [SerializeField] Animal enemy = null;
    Animal hunting = null;

    private void HandleHunting(ref Animal hunting)
    {
        
        float preyDistance = float.MaxValue;
        if (hunting)
        {
            preyDistance = Vector3.Distance(hunting.transform.position, transform.position);
            this.hunting = hunting;
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
            Vector3 forward = hunting.transform.position - transform.position;
            transform.forward = new Vector3(forward.x, 0, forward.z);
        }
        else
        {
            hunting = null;
            this.hunting = null;
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
                if(!chicken.dead && Vector3.Distance(transform.position, prey.transform.position) > Vector3.Distance(transform.position, chicken.transform.position))
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

        if (state == States.run) Turn(true, transform.forward);
    }

    public override void AttackAnimTrigger()
    {
        if (hunting)
        {
            float preyDistance = Vector3.Distance(hunting.transform.position, transform.position);
            bool kill = false;
            if (preyDistance < attackIdleRadius)
            {
                kill = hunting.Hit(2,transform);
            }
            else if (preyDistance < attackRadius)
            {
                kill = hunting.Hit(1, transform);
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
    protected override void HitState(Transform source)
    {
        if (state == States.idle || state == States.attackIdle) state = States.hitIdle;
        else if (state == States.attackRun || state == States.run) state = States.hitRun;

        if (source.GetComponent<Dog>())
        {
            enemy = source.GetComponent<Dog>();
            
        }
    }

    protected void Duplicate()
    {
        lastBirth = Time.time;
        Animal animal = Instantiate(gameObject, transform.position + transform.forward * (-1), Quaternion.identity).GetComponent<Animal>();
        FindObjectOfType<PlayerGrabber>().animalSelection.AddAnimal(animal);
        
    }

    protected override void DieState()
    {
        state = States.hitDie;
        //Destroy(GetComponent<Rigidbody>());
        //GetComponent<Collider>().enabled = false;
        StartCoroutine(Die(shrinkTime));
    }

    public float shrinkTime = 3;
    public float shrinkPercent = 100;
    protected override IEnumerator Die(float wait)
    {
        float shrinkStart = Time.time;
        Vector3 shrinkSteps = ((shrinkPercent / 100f) / shrinkTime) * transform.localScale;
        while (wait + shrinkStart > Time.time)
        {
            transform.localScale -= shrinkSteps * Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
