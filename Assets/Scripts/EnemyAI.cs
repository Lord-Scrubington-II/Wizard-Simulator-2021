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
    
    public enum AIStates {
        Idle, ChasingPlayer, ChasingOther, Attacking
    }
    public AIStates AIState { get; private set; }

    void Start() {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
    }

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
            Idle();
        }
    }

    private void Idle() {
        AIState = AIStates.Idle;
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
        AIState = AIStates.ChasingPlayer;
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(target.position);
    }

    private void AttackTarget() {
        AIState = AIStates.Attacking;
        Debug.Log(name + " is attacking " + target.name);
    }

    /// <summary>
    /// Draw a wiresphere to indicate the aggro FOV
    /// </summary>
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, chaseRange);
    }

}
