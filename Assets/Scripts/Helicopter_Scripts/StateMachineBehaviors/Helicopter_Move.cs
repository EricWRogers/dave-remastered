using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter_Move : StateMachineBehaviour
{
    private GameObject player;
    private HelicopterManager manager;
    public float speed = .5f;
    private bool moving = false;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        manager = animator.transform.GetComponent<HelicopterManager>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Up"))                   
        {
            animator.transform.parent.transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);

            if (!moving)
            {
                manager.StartMoveTimer(manager.minTimeUp, manager.maxTimeUp);
                moving = true;
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Down"))
        {
            animator.transform.parent.transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);

            if (!moving)
            {
                manager.StartMoveTimer(manager.minTimeDown, manager.maxTimeDown);
                moving = true;
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Left"))
        {
            animator.transform.parent.transform.RotateAround(player.transform.position, Vector3.down, speed * Time.deltaTime);

            if (!moving)
            {
                manager.StartMoveTimer(manager.minTimeLeft, manager.maxTimeLeft);
                moving = true;
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Right"))
        {
            animator.transform.parent.transform.RotateAround(player.transform.position, Vector3.up, speed * Time.deltaTime);

            if (!moving)
            {
                manager.StartMoveTimer(manager.minTimeRight, manager.maxTimeRight);
                moving = true;
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        moving = false;
    }
}
