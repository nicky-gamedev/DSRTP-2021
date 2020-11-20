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
    //[SerializeField] Renderer rend;

    [Header("Parameters")]
    [SerializeField] float attackJumpForce = 20f;
    [SerializeField] float attackAngle = -30f;

    public Animator anim;

    [Header("Test")]
    [SerializeField] float timeFalling = 0;
    public float timeFallingLimit = 5;

    public ParticleSystem deathParticle;

    public AudioSource[] sources = new AudioSource[4];

    private void Awake()
    {
        brain = GetComponent<EnemyBrain>();
        brain.target = GameObject.FindGameObjectWithTag("Player");
        if (!agent) agent = GetComponent<NavMeshAgent>();
        if (!rb) rb = GetComponent<Rigidbody>();
        //if (!rend) rend = GetComponent<Renderer>();
        deathParticle = GetComponentInChildren<ParticleSystem>();
        deathParticle.playOnAwake = false;
    }

    void Update()
    {
        Debug.Log("Enemy State is " + enemyState);
        enemyState = brain.UpdateBrain(this);
        if (enemyState == EnemyBrain.States.IDLE)
        {
            rb.velocity = Vector3.zero;
            transform.rotation = Quaternion.identity;
            //rend.material.color = Color.white;

            StopSource(sources[1]);
            StopSource(sources[2]);
            StopSource(sources[3]);

            StartSource(sources[0]);

            timeFalling = 0;
        }
        if (enemyState == EnemyBrain.States.MOVING)
        {
            agent.enabled = true;
            MoveToPlayer();
            //rend.material.color = Color.cyan;

            StopSource(sources[0]);
            StopSource(sources[1]);
            StopSource(sources[3]);

            StartSource(sources[2]);

            timeFalling = 0;
        }
        if (enemyState == EnemyBrain.States.ATTACK)
        {
            Attack();
            //rend.material.color = Color.red;

            anim.Play("Attack");

            StopSource(sources[0]);
            StopSource(sources[2]);
            StopSource(sources[3]);

            StartSource(sources[1]);

            timeFalling = 0;
        }
        if (enemyState == EnemyBrain.States.LOAD)
        {
            //transform.LookAt(brain.target.transform);
            //rend.material.color = Color.yellow;
            Load();

            timeFalling = 0;
        }
        if (enemyState == EnemyBrain.States.WAITING)
        {
            rb.angularVelocity = Vector3.zero;
            Vector3 lookPos = new Vector3(brain.target.transform.position.x - transform.position.x, 0, brain.target.transform.position.z - transform.position.z);
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 20f);
            //rend.material.color = Color.blue;

            timeFalling = 0;
        }
        if (enemyState == EnemyBrain.States.FALLING)
        {

            //rend.material.color = Color.green;
            timeFalling += Time.deltaTime;
            if (timeFalling >= timeFallingLimit)
            {
                Debug.Log("Caindo a muito tempo");
                RaycastHit hit;
                Ray ray = new Ray(transform.position, new Vector3(0, -75, 0));
                Debug.DrawRay(transform.position, new Vector3(0, -75, 0), Color.blue, 6f);
                Physics.Raycast(ray, out hit);
                if (Mathf.Abs(Vector3.Distance(transform.position, hit.point)) < 2f)
                {
                    Debug.Log("Chao está perto, resetando");
                    agent.enabled = true;
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

        anim.SetBool("Running", true);
    }

    void Attack()
    {
        anim.SetBool("Running", false);

        rb.isKinematic = false;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        rb.AddForceAtPosition((transform.forward + new Vector3(0,.2f,0)) * attackJumpForce, transform.position, ForceMode.Impulse);
    }

    void Load()
    {
        anim.SetBool("Running", false);

        rb.velocity = Vector3.zero;
        if (agent.hasPath)
        {
            agent.isStopped = true;
        }
        agent.enabled = false;
        transform.LookAt(new Vector3(0, brain.target.transform.position.y, 0));
    }

    public void Kill()
    {
        anim.Play("Squash");
        deathParticle.Play();
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void StopSource(AudioSource ad)
    {
        if (ad.isPlaying)
        {
            ad.Stop();
        }
    }

    private void StartSource(AudioSource ad)
    {
        if (!ad.isPlaying)
        {
            ad.Play();
        }
    }
}
