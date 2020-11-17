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
    [SerializeField] Renderer rend;

    [Header("Parameters")]
    [SerializeField] float attackJumpForce = 20f;
    [SerializeField] float attackAngle = -30f;

    private void Awake()
    {
        brain = GetComponent<EnemyBrain>();
        brain.target = GameObject.FindGameObjectWithTag("Player");
        if (!agent) agent = GetComponent<NavMeshAgent>();
        if (!rb) rb = GetComponent<Rigidbody>();
        if (!rend) rend = GetComponent<Renderer>();
    }

    void Update()
    {
        Debug.Log("Enemy State is " + enemyState);
        enemyState = brain.UpdateBrain(this);
        if (enemyState == EnemyBrain.States.IDLE)
        {
            rb.velocity = Vector3.zero;
            transform.rotation = Quaternion.identity;
            rend.material.color = Color.white;
        }
        if (enemyState == EnemyBrain.States.MOVING)
        {
            agent.enabled = true;
            MoveToPlayer();
            rend.material.color = Color.cyan;
        }
        if (enemyState == EnemyBrain.States.ATTACK)
        {
            Attack();
            rend.material.color = Color.red;
        }
        if (enemyState == EnemyBrain.States.LOAD)
        {
            transform.LookAt(brain.target.transform);
            rend.material.color = Color.yellow;
            Load();
        }
        if (enemyState == EnemyBrain.States.WAITING)
        {
            rb.angularVelocity = Vector3.zero;
            var lookPos = brain.target.transform.position - transform.position;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 20f);
            rend.material.color = Color.blue;
        }
        if (enemyState == EnemyBrain.States.FALLING)
        {
            rend.material.color = Color.green;
        }
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
