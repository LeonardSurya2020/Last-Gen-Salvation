using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    public void EnterState(EnemyMovement enemy)
    {
        Debug.Log("enemy entering attack state");
    }

    public void UpdateState(EnemyMovement enemy)
    {
        float distanceToTarget = Vector2.Distance(enemy.transform.position, enemy.target.transform.position);

        // Jika dalam jarak serangan, pindah ke state attack
        if (distanceToTarget > enemy.chaseRadius)
        {
            enemy.StopChasing();
            enemy.TransitionToState(enemy.idleState);
        }
        // Jika keluar dari radius pengejaran, pindah ke idle
        else if (distanceToTarget > enemy.attackRadius)
        {
            if(enemy.isAttacking == true)
            {
                return;
            }
            enemy.TransitionToState(enemy.chaseState);
        }
        else
        {
            if (enemy.isAttacking == true)
            {
                return;
            }
            if(enemy.target != null)
            {
                enemy.AttackingPlayer();
            }
            else
            {
                enemy.StopChasing();
                enemy.TransitionToState(enemy.idleState);
            }
            
            // Lanjutkan mengejar pemain
        }
    }

    public void ExitState(EnemyMovement enemy)
    {
        Debug.Log("enemy exit attack state");
    }

}
