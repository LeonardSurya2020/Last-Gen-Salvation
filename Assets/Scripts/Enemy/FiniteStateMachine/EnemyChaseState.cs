using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : IEnemyState
{
    public void EnterState(EnemyMovement enemy)
    {
        Debug.Log("enemy entering chasing state");
    }

    public void UpdateState(EnemyMovement enemy)
    {
        float distanceToTarget = Vector2.Distance(enemy.transform.position, enemy.target.transform.position);

        // Jika dalam jarak serangan, pindah ke state attack
        if (distanceToTarget <= enemy.attackRadius)
        {
            enemy.StopChasing();
            enemy.TransitionToState(enemy.attackState);
        }
        // Jika keluar dari radius pengejaran, pindah ke idle
        else if (distanceToTarget > enemy.chaseRadius)
        {
            enemy.StopChasing();
            enemy.TransitionToState(enemy.idleState);
        }
        else
        {
            // Lanjutkan mengejar pemain
            enemy.animator.SetBool("IsRunning", true);
            if(enemy.target != null)
            {
                enemy.ChasingPlayer();
            }
            else
            {
                enemy.StopChasing();
                enemy.TransitionToState(enemy.idleState);
            }
           
        }
    }

    public void ExitState(EnemyMovement enemy)
    {
        Debug.Log("enemy exit chasing state");
    }


}
