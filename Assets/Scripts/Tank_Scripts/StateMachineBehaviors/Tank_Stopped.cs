using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Stopped : StateMachineBehaviour
{
    private GameObject player;
    private TankManager tank;
    private bool started = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("PlayerGround");
        tank = animator.GetComponent<TankManager>();
        started = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        tank.RotateTurret();

        if (tank.foundPlayer && !started)
        {
            tank.RepeatMissile();
            started = true;
        }

        float Distance = Vector3.Distance(player.transform.position, animator.transform.parent.position);     //constant distance check while in the state

        if (Distance > (tank.stoppingDistance + .5f))
        {
            animator.SetTrigger("Lower");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        tank.CancelMissile();
        animator.ResetTrigger("Lower");
    }
}
