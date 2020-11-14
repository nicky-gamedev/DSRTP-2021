using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    public enum States { IDLE, MOVING, ATTACK, WAITING }
    public GameObject target;

    [Header("Parameters")]
    [SerializeField] float detectionRange;
    [SerializeField] float attackRadius;
    [SerializeField] float attackCooldown;
    [SerializeField] LayerMask ignoreLayer;

    [Header("Debug")]
    [SerializeField] bool showGizmos;

    float lastAttackTime;
    float nextAttackTime;
    
    public States UpdateBrain()
    {
        if (target == null) return States.IDLE;

        Bounds detectionBounds = new Bounds(transform.position, Vector3.one * detectionRange);
        if (!detectionBounds.Contains(target.transform.position)) return States.IDLE;

        bool inAttackRange = Vector3.Distance(target.transform.position, transform.position) <= attackRadius;
        if (inAttackRange)
        {
            if(Time.time <= nextAttackTime)
            {
                return States.WAITING;
            }

            lastAttackTime = Time.time;
            nextAttackTime = lastAttackTime + attackCooldown;
            return States.ATTACK;
        }

        return States.MOVING;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = new Color(255f, 0f, 0f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Gizmos.color = new Color(0f, 255f, 0f, 0.5f);
        Bounds debugBounds = new Bounds(transform.position, Vector3.one * detectionRange);
        DrawBounds(debugBounds);
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
}
