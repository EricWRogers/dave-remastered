using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_LowerBarrel : StateMachineBehaviour
{
    private TankManager tank;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        tank = animator.transform.GetComponent<TankManager>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        tank.topTurret.transform.rotation = Quaternion.Slerp(tank.topTurret.transform.rotation, tank.transform.rotation, Time.deltaTime * tank.turretTurnSpeed);

        if (Quaternion.Angle(tank.topTurret.transform.rotation, tank.transform.rotation) <= 1f)
            animator.SetTrigger("FinishedLower");
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("FinishedLower");
    }
}
