using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    EnemyBrain brain;
    public EnemyBrain.States enemyState = EnemyBrain.States.IDLE;

    [Header("References")]
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] Rigidbody rb;

    [Header("Parameters")]
    [SerializeField] float attackJumpForce = 20f;
    [SerializeField] float attackAngle = -30f;

    private void Awake()
    {
        brain = GetComponent<EnemyBrain>();
        brain.target = GameObject.FindGameObjectWithTag("Player");
        if (!agent) agent = GetComponent<NavMeshAgent>();
        if (!rb) rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        enemyState = brain.UpdateBrain(this);
        if(enemyState == EnemyBrain.States.IDLE)
        {
            rb.velocity = Vector3.zero;
        }
        if(enemyState == EnemyBrain.States.MOVING)
        {
            MoveToPlayer();
        }
        if (enemyState == EnemyBrain.States.ATTACK)
        {
            Attack();
        }
        if(enemyState == EnemyBrain.States.LOAD)
        {
            transform.LookAt(brain.target.transform);
            Load();
        }
        if(enemyState == EnemyBrain.States.WAITING)
        {
            transform.LookAt(brain.target.transform);
        }
        Debug.Log("Enemy is: " + enemyState);
    }

    void MoveToPlayer()
    {
        rb.velocity = Vector3.zero;
        if (!agent.isOnNavMesh) return;
        if (agent.isStopped) agent.isStopped = false;
        agent.SetDestination(brain.target.transform.position);
    }

    void Attack()
    {
        rb.isKinematic = false;
        transform.rotation = Quaternion.Euler(attackAngle, transform.eulerAngles.y, transform.eulerAngles.z);
        rb.AddForceAtPosition(transform.forward * attackJumpForce, transform.position, ForceMode.Impulse);
    }

    void Load()
    {
        rb.velocity = Vector3.zero;
        if (agent.hasPath)
        {
            agent.isStopped = true;
        }
        agent.enabled = false;
        transform.LookAt(brain.target.transform);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!agent.enabled && collision.collider.gameObject.layer == 8 && enemyState != EnemyBrain.States.ATTACK)
        {
            agent.enabled = true;
        }
    }
}
