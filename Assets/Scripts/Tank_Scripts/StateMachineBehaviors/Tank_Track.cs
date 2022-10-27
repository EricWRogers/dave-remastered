using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Track : StateMachineBehaviour
{
    private GameObject player;
    private float stoppingDistance;
    private UnityEngine.AI.NavMeshAgent agent;
    private TankManager tank;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Move");
        animator.ResetTrigger("Stop");
        player = GameObject.FindGameObjectWithTag("PlayerGround");
        tank = animator.transform.GetComponent<TankManager>();
        stoppingDistance = animator.GetComponent<TankManager>().stoppingDistance;
        agent = animator.GetComponentInParent<UnityEngine.AI.NavMeshAgent>();

        animator.transform.parent.GetComponent<AudioSource>().Play();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float Distance = Vector3.Distance(player.transform.position, animator.transform.parent.position);     //constant distance check while in the state
        if (Distance <= stoppingDistance)       //if the distance is greater than the orbit range...
        {
            animator.SetTrigger("Stop");
        }

        if (Distance > stoppingDistance)
        {
            if (agent != null)
                agent.SetDestination(player.transform.position);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Stop");
        if (agent != null)
            agent.ResetPath();

        animator.transform.parent.GetComponent<AudioSource>().Stop();
    }
}
