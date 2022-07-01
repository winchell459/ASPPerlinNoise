using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAgent : MonoBehaviour
{
    public NavMeshAgent navAgent;
    public Transform target;


    // Update is called once per frame
    void Update()
    {
       
        navAgent.SetDestination(target.position);
    }
}
