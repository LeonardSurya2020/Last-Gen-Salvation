using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IPlayerState
{
    public void EnterState(PlayerMovement player)
    {
        Debug.Log("Entering Idle State");
    }

    public void UpdateState(PlayerMovement player)
    {
        if(player.horiz > 0 || player.vert > 0 || player.horiz < 0 || player.vert < 0)
        {
            player.TransitionToState(player.runningState);
        }
        else
        {
            
            player.TransitionToState(player.idleState);
        }
    }

    public void ExitState(PlayerMovement player)
    {
        Debug.Log("Exiting Idle State");
    }
}
