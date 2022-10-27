using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat_Attack : StateMachineBehaviour
{
    private BoatManager boat;
    private GameObject player;
    private Transform playerGround;
    private UnityEngine.AI.NavMeshAgent agent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boat = animator.GetComponent<BoatManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerGround = GameObject.FindGameObjectWithTag("PlayerGround").transform;
        agent = animator.GetComponentInParent<UnityEngine.AI.NavMeshAgent>();

        boat.RepeatFireGun();
        boat.atkSound.Play();

        foreach (ParticleSystem effect in boat.waterEffects)
        {
            if (effect.isEmitting)
            {
                effect.Stop();
            }
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Rotate();

        float Distance = Vector3.Distance(player.transform.position, animator.transform.parent.position);     //constant distance check while in the state
        if (Distance > boat.gunRange)       //if the distance is greater than the orbit range...
        {
            animator.SetTrigger("Adjust");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Adjust");
        boat.CancelInvoke();
        boat.atkSound.Stop();
    }

    void Rotate()
    {
        Vector3 targetPoint = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z) - boat.gun.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetPoint, Vector3.up);
        boat.gun.transform.rotation = Quaternion.Slerp(boat.gun.transform.rotation, targetRotation, Time.deltaTime * boat.gunTurnSpeed);
    }
}
