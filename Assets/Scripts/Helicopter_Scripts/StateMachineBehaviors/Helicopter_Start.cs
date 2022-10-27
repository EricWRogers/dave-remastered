using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter_Start : StateMachineBehaviour
{
    public float speed = .5f;
    private float orbitRange;

    private GameObject player;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        orbitRange = animator.transform.GetComponent<HelicopterManager>().orbitRange;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float Distance = Vector3.Distance(player.transform.position, animator.transform.parent.position);     //constant distance check while in the state
        if (Distance < orbitRange)       //if the distance is greater than the orbit range...
        {
            animator.SetTrigger("Orbit");
        }

        if (Distance > orbitRange)
        {
            animator.transform.parent.transform.position = Vector3.MoveTowards(animator.transform.parent.transform.position, player.transform.position, speed * Time.deltaTime);
        }

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Orbit");
    }
}
