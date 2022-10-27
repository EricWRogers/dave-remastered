using UnityEngine;

public class Plane_Turn : StateMachineBehaviour     //handles the plane during the turning animation
{
    private PlaneManager plane;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        plane = animator.transform.GetComponent<PlaneManager>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.parent.Translate(animator.transform.parent.forward * plane.speed * Time.deltaTime); //simply moves forward in the animation
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        plane.secondTime = true;    //lets the tracking and loop script know we have now passed into the second rotation and no longer to set some rotation values
        animator.transform.parent.rotation = Quaternion.Euler(0f, 0f, 0f);  //locks the plane to facing the correct direction
    }
}
