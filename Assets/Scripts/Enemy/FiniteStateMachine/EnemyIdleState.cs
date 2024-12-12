using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : IEnemyState
{
    public void EnterState(EnemyMovement enemy)
    {
        Debug.Log("enemy entering idle state");
    }

    public void UpdateState(EnemyMovement enemy)
    {
        
        float distanceToTarget = Vector2.Distance(enemy.transform.position, enemy.target.transform.position);

        Debug.Log("distance = " + distanceToTarget);
        if(distanceToTarget < enemy.chaseRadius)
        {
            enemy.TransitionToState(enemy.chaseState);
        }
    }

    public void ExitState(EnemyMovement enemy)
    {
        Debug.Log("enemy exiting idle state");
    }

}
