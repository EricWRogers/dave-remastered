using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter_OrbitController : StateMachineBehaviour
{
    private GameObject player;
    private HelicopterManager manager;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        manager = animator.GetComponent<HelicopterManager>();
        manager.StartOrbitTimer();
        manager.started = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*float Distance = Vector3.Distance(player.transform.position, animator.transform.parent.position);     //constant distance check while in the state
        if (Distance > animator.GetComponent<Helicopter_Start>().orbitRange)       //if the distance is greater than the orbit range...
        {
            animator.transform.parent.transform.position = Vector3.MoveTowards(animator.transform.parent.transform.position, player.transform.position, animator.GetComponent<Helicopter_Start>().speed * Time.deltaTime);
        }*/
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Up");
        animator.ResetTrigger("Down");
        animator.ResetTrigger("Left");
        animator.ResetTrigger("Right");
    }
}
