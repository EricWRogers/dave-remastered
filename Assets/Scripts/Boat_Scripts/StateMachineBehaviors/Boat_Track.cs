using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat_Track : StateMachineBehaviour
{
    private BoatManager boat;
    private UnityEngine.AI.NavMeshAgent agent;
    private GameObject player;
    private bool startedSound = false;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boat = animator.GetComponent<BoatManager>();
        agent = animator.GetComponentInParent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (!startedSound)
        {
            boat.boatSoundSource.Play();
            startedSound = true;
        }

        foreach (ParticleSystem effect in boat.waterEffects)
        {
            if (!effect.isEmitting)
            {
                effect.Play();
            }
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float Distance = Vector3.Distance(player.transform.position, animator.transform.parent.position);     //constant distance check while in the state
        if (Distance < boat.gunRange)       //if the distance is greater than the orbit range...
        {
            animator.SetTrigger("Attack");
        }

        if (Distance > boat.gunRange)
        {
            if (agent != null)
                agent.SetDestination(player.transform.position);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        if (agent != null)
            agent.ResetPath();

        if (startedSound)
        {
            boat.boatSoundSource.Stop();
            startedSound = false;
        }
    }
}
