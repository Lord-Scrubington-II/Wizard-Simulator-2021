using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float chaseRange;

    NavMeshAgent navMeshAgent;
    float distanceToTarget = float.PositiveInfinity;
    bool isProvoked = false;

    // Start is called before the first frame update
    void Start() {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update() {
        distanceToTarget = Vector3.Distance(target.position, gameObject.transform.position);
        if (distanceToTarget <= chaseRange) {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(target.position);
        } else navMeshAgent.isStopped = true;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, chaseRange);
    }
}
