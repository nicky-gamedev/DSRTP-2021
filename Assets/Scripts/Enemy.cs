using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyBrain brain;
    public EnemyBrain.States enemyState = EnemyBrain.States.IDLE;

    private void Awake()
    {
        brain = GetComponent<EnemyBrain>();
        brain.target = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        enemyState = brain.UpdateBrain();
        if(enemyState == EnemyBrain.States.MOVING)
        {
            MoveToPlayer();
        }
        if (enemyState == EnemyBrain.States.ATTACK)
        {
            Attack();
        }
    }

    void MoveToPlayer()
    {

    }

    void Attack()
    {

    }
}
