using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulIdleState : IEnemySoulState
{
    public void EnterState(SoulsEnemyMovement enemy)
    {
        Debug.Log("Soul entering Idle State");
    }

    public void UpdateState(SoulsEnemyMovement enemy)
    {
        float distanceToTarget = Vector2.Distance(enemy.transform.position, enemy.target.transform.position);

        Debug.Log("distance = " + distanceToTarget);
        if (distanceToTarget < enemy.chaseRadius)
        {
            enemy.TransitionToState(enemy.chaseState);
        }
    }

    public void ExitState(SoulsEnemyMovement enemy)
    {
        Debug.Log("Soul exiting Idle State");
    }


}
