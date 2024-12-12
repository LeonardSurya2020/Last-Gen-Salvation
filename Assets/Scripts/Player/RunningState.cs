using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : IPlayerState
{
    public void EnterState(PlayerMovement player)
    {
        Debug.Log("Entering Running state");
    }

    public void UpdateState(PlayerMovement player)
    {
        if (player.horiz > 0 || player.vert > 0 || player.horiz < 0 || player.vert < 0)
        {
            player.animator.SetBool("isRunning", true);
            player.Running(player.horiz, player.vert);
        }
        else
        {
            player.animator.SetBool("isRunning", false);
            player.rb.velocity = new Vector2(0, 0);
            player.TransitionToState(player.idleState);
        }
    }

    public void ExitState(PlayerMovement player)
    {
        Debug.Log("Exiting Running state");
    }
}
