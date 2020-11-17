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

    [Header("Test")]
    [SerializeField] float timeFalling = 0;

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

            timeFalling = 0;
        }
        if (enemyState == EnemyBrain.States.MOVING)
        {
            agent.enabled = true;
            MoveToPlayer();
            rend.material.color = Color.cyan;

            timeFalling = 0;
        }
        if (enemyState == EnemyBrain.States.ATTACK)
        {
            Attack();
            rend.material.color = Color.red;

            timeFalling = 0;
        }
        if (enemyState == EnemyBrain.States.LOAD)
        {
            //transform.LookAt(brain.target.transform);
            rend.material.color = Color.yellow;
            Load();

            timeFalling = 0;
        }
        if (enemyState == EnemyBrain.States.WAITING)
        {
            rb.angularVelocity = Vector3.zero;
            Vector3 lookPos = new Vector3(brain.target.transform.position.x - transform.position.x, 0, brain.target.transform.position.z - transform.position.z);
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 20f);
            rend.material.color = Color.blue;

            timeFalling = 0;
        }
        if (enemyState == EnemyBrain.States.FALLING)
        {
            rend.material.color = Color.green;
            timeFalling += Time.deltaTime;
            if (timeFalling >= 7.5f)
            {
                Debug.Log("Caindo a muito tempo");
                RaycastHit hit;
                Ray ray = new Ray(transform.position, new Vector3(0, -75, 0));
                Debug.DrawRay(transform.position, new Vector3(0, -75, 0), Color.blue, 6f);
                Physics.Raycast(ray, out hit);
                if (Mathf.Abs(Vector3.Distance(transform.position, hit.point)) < 1f)
                {
                    Debug.Log("Chao está perto, resetando");
                    agent.enabled = true;
                }
                else if(Mathf.Abs(Vector3.Distance(transform.position, hit.transform.position)) > 50)
                {
                    //se saiu voando pra longe
                    Destroy(this.gameObject);
                }
                timeFalling = 0;
            }
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
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        rb.AddForceAtPosition((transform.forward + new Vector3(0,.2f,0)) * attackJumpForce, transform.position, ForceMode.Impulse);
    }

    void Load()
    {
        rb.velocity = Vector3.zero;
        if (agent.hasPath)
        {
            agent.isStopped = true;
        }
        agent.enabled = false;
        transform.LookAt(new Vector3(0, brain.target.transform.position.y, 0));
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*
        if (!agent.enabled && collision.collider.gameObject.layer == 8 && enemyState != EnemyBrain.States.ATTACK)
        {
            agent.enabled = true;
        }
        */
    }
}
