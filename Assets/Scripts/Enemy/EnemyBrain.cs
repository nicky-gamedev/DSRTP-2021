using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBrain : MonoBehaviour
{
    public enum States { IDLE, MOVING, LOAD, ATTACK, WAITING, FALLING }


    [Header("References")]
    public GameObject target;
    public GameObject floorSensor;

    [Header("Parameters")]
    [SerializeField] float detectionRange;
    [SerializeField] float attackRadius;
    [SerializeField] float attackDelay;
    [SerializeField] float attackCooldown;
    [SerializeField] LayerMask ignoreLayer;

    [Header("Debug")]
    [SerializeField] bool showGizmos;

    float nextActionTime;
    bool canAttack;
    bool isWaiting;
    
    public States UpdateBrain(Enemy enemy)
    {
        //SECURITY: Idle if no target is setted
        if (target == null) return States.IDLE;

        bool isGrounded = Physics.Linecast(transform.position, floorSensor.transform.position);
        if (isGrounded)
        {
            //If the player isn't near, means that we dont need to do anything.
            //But we return idle, for animation stuff
            Bounds detectionBounds = new Bounds(transform.position, Vector3.one * detectionRange);
            if (!detectionBounds.Contains(target.transform.position) && isGrounded) return States.IDLE;

            //If the player is inside the attack range and isn't idle
            // proceed with the attack or load actions
            bool inAttackRange = Vector3.Distance(target.transform.position, transform.position) <= attackRadius;
            if (inAttackRange)
            {
                //If the time is smaller than the next action time, means that some action needs to be waited
                //Also, it needs to be on the ground
                if (Time.time <= nextActionTime && isGrounded)
                {
                    isWaiting = false;
                    return States.WAITING;
                }
                //If the AI has loaded, and is on the ground, then the attack is valid
                if (canAttack && isGrounded)
                {
                    nextActionTime = Time.time + attackCooldown;
                    canAttack = false;
                    isWaiting = true;
                    return States.ATTACK;
                }
                //If the AI can't attack but is on the floor, it needs to load
                else if (isGrounded)
                {
                    nextActionTime = Time.time + attackDelay;
                    canAttack = true;
                    isWaiting = true;
                    return States.LOAD;
                }
            }
            //If isn't on attack range, but is inside the detection range, it needs to move.
            else
            {
                return States.MOVING;
            }
        }
        
        if (isWaiting)
        {
            Debug.LogWarning("This was waiting but now it isn't on the ground. Should correct it's position?");
        }

        //if isn't on the ground, and wasn't waiting before, is falling
        //so no action should be done until it is on the ground
        return States.FALLING;
    }

    #region Debug
    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Bounds debugBounds = new Bounds(transform.position, Vector3.one * detectionRange);
        DrawBounds(debugBounds);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, floorSensor.transform.position);
    }

    void DrawBounds(Bounds b, float delay = 0)
    {
        // bottom
        var p1 = new Vector3(b.min.x, b.min.y, b.min.z);
        var p2 = new Vector3(b.max.x, b.min.y, b.min.z);
        var p3 = new Vector3(b.max.x, b.min.y, b.max.z);
        var p4 = new Vector3(b.min.x, b.min.y, b.max.z);

        Debug.DrawLine(p1, p2, Color.blue, delay);
        Debug.DrawLine(p2, p3, Color.red, delay);
        Debug.DrawLine(p3, p4, Color.yellow, delay);
        Debug.DrawLine(p4, p1, Color.magenta, delay);

        // top
        var p5 = new Vector3(b.min.x, b.max.y, b.min.z);
        var p6 = new Vector3(b.max.x, b.max.y, b.min.z);
        var p7 = new Vector3(b.max.x, b.max.y, b.max.z);
        var p8 = new Vector3(b.min.x, b.max.y, b.max.z);

        Debug.DrawLine(p5, p6, Color.blue, delay);
        Debug.DrawLine(p6, p7, Color.red, delay);
        Debug.DrawLine(p7, p8, Color.yellow, delay);
        Debug.DrawLine(p8, p5, Color.magenta, delay);

        // sides
        Debug.DrawLine(p1, p5, Color.white, delay);
        Debug.DrawLine(p2, p6, Color.gray, delay);
        Debug.DrawLine(p3, p7, Color.green, delay);
        Debug.DrawLine(p4, p8, Color.cyan, delay);
    }
    #endregion
}
