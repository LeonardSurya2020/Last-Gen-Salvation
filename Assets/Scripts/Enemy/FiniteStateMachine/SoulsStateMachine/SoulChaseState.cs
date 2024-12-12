using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulChaseState : IEnemySoulState
{
    public void EnterState(SoulsEnemyMovement enemy)
    {
        Debug.Log("Soul entering Idle State");
    }

    public void UpdateState(SoulsEnemyMovement enemy)
    {
        float distanceToTarget = Vector2.Distance(enemy.transform.position, enemy.target.transform.position);

        // Jika keluar dari radius pengejaran, pindah ke idle
        if (distanceToTarget > enemy.chaseRadius)
        {
            enemy.StopChasing();
            enemy.TransitionToState(enemy.idleState);
        }
        else
        {
            // Lanjutkan mengejar pemain
            enemy.animator.SetBool("IsRunning", true);
            enemy.ChasingPlayer();
        }
    }

    public void ExitState(SoulsEnemyMovement enemy)
    {
        Debug.Log("Soul exiting Idle State");
    }
}
