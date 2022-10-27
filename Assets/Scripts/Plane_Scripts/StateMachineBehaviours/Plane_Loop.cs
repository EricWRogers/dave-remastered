using UnityEngine;

public class Plane_Loop : StateMachineBehaviour     //handles the time after the plane is directly above the player
{
    private PlaneManager plane;
    private float runningTime = 0f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        plane = animator.GetComponent<PlaneManager>();
        runningTime = 0f;

        plane.atkSound.Stop();
    }

    //adds time to the running total until it hits the max flyTime set by PlaneManager
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        runningTime += Time.smoothDeltaTime;
        //animator.transform.LookAt(Vector3.forward);
        //animator.transform.rotation = plane.lastRotation;
        animator.transform.parent.Translate(animator.transform.forward * plane.speed * Time.deltaTime);
        //plane.FlyPast();
        if (runningTime >= plane.flyTime)
        {
            animator.SetTrigger("Turn");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Turn");
    }
}
