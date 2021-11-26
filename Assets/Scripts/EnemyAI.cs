using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float chaseRange;
    [SerializeField] float exitRange;

    NavMeshAgent navMeshAgent;
    float distanceToTarget = float.PositiveInfinity;
    bool isProvoked = false;

    // Start is called before the first frame update
    void Start() {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update() {

        // update the distance to the target and route the state
        distanceToTarget = Vector3.Distance(target.position, gameObject.transform.position);

        if (isProvoked) {

            if (distanceToTarget > exitRange) {
                isProvoked = false;

            } else {
                EngageTarget();
            }

        } else if (distanceToTarget <= chaseRange) {
            isProvoked = true;

        } else {
            isProvoked = false;
            Idle();
        }
    }

    private void Idle() {
        navMeshAgent.isStopped = true;
    }

    private void EngageTarget() {
        if (distanceToTarget >= navMeshAgent.stoppingDistance) {
            ChaseTarget();
        } 

        if (distanceToTarget <= navMeshAgent.stoppingDistance) {
            AttackTarget();
        }
    }

    private void ChaseTarget() {
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(target.position);
    }

    private void AttackTarget() {
        Debug.Log(name + " is attacking " + target.name);
    }


    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, chaseRange);
    }
}
