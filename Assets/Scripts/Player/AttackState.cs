using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IPlayerState
{
    public void EnterState(PlayerMovement player)
    {
        Debug.Log("Entering Attacking state");
    }

    public void UpdateState(PlayerMovement player)
    {
        if(player.isAttacking)
        {
            player.rb.velocity = new Vector2(0, 0);
        }
        else if(player.horiz > 0 || player.vert > 0 || player.horiz < 0 || player.vert < 0)
        {
            player.TransitionToState(player.runningState);
        }
        else
        {
            player.animator.SetBool("isRunning", false);
            player.TransitionToState(player.idleState);
        }
    }

    public void ExitState(PlayerMovement player)
    {
        Debug.Log("Exiting Attacking state");
    }
}
