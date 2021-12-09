using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class defines the finite state machine responsible for controlling
/// the default enemy AI and its corresponding animations.
/// 
/// ( ͡° ͜ʖ ͡°) Detroit: Become Gamer
/// </summary>
[RequireComponent(typeof(Enemy))][RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float chaseRange;
    [SerializeField] float exitRange;
    [SerializeField] float turnSpeed = 5.0f;

    NavMeshAgent navMeshAgent;
    Enemy thisEnemy;
    float distanceToTarget = float.PositiveInfinity;
    bool isProvoked = false;

    private Dictionary<AIStates, string> animStateDict = new Dictionary<AIStates, string>() {
        {AIStates.Idle, "onIdle"}, 
        {AIStates.ChasingPlayer, "onMove"}, 
        {AIStates.ChasingOther, "onMove"}, 
        {AIStates.Attacking, "onAttack"}
    };

    public enum AIStates {
        Idle, ChasingPlayer, ChasingOther, Attacking
    }
    public AIStates AIState { get; private set; }
    public Transform Target { get => target; set => target = value; }

    void Start() {
        Target = FindObjectOfType<Player>().transform;
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        thisEnemy = gameObject.GetComponent<Enemy>();
    }

    void Update() {
        
        // turn off the brain when dead
        if  (thisEnemy.HasDied) {
            this.enabled = false;
            navMeshAgent.enabled = false;
            return;
        }

        // update the distance to the target and route the state
        distanceToTarget = Vector3.Distance(Target.position, gameObject.transform.position);

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
        GetComponent<Animator>().SetTrigger(animStateDict[AIState]);
        navMeshAgent.isStopped = true;
    }

    /// <summary>
    /// Begin combat engagement
    /// </summary>
    private void EngageTarget() {
        FaceTarget();
        if (distanceToTarget >= navMeshAgent.stoppingDistance) {
            ChaseTarget();
        } else if (distanceToTarget <= navMeshAgent.stoppingDistance) {
            AttackTarget();
        }
    }

    /// <summary>
    /// Message: on damage take, provoke this enemy
    /// </summary>
    /// <param name="amount"></param>
    private void OnDamageTaken(int amount) {
        isProvoked = true;
    }

    private void ChaseTarget() {
        AIState = AIStates.ChasingPlayer;
        GetComponent<Animator>().SetBool(animStateDict[AIStates.Attacking], false);
        GetComponent<Animator>().SetTrigger(animStateDict[AIState]);
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(Target.position);
    }

    private void AttackTarget() {
        AIState = AIStates.Attacking;
        GetComponent<Animator>().SetBool(animStateDict[AIState], true);
        Debug.Log(name + " is attacking " + Target.name);
    }

    private void FaceTarget() {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion newRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * turnSpeed);
    }

    /// <summary>
    /// Draw a wiresphere to indicate the aggro FOV
    /// </summary>
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, chaseRange);
    }

}
