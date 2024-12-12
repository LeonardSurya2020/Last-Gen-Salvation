using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemySoulState
{
    void EnterState(SoulsEnemyMovement enemy);
    void UpdateState(SoulsEnemyMovement enemy);
    void ExitState(SoulsEnemyMovement enemy);
}
