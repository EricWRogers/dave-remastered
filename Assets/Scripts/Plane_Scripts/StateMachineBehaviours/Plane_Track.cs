using UnityEngine;

public class Plane_Track : StateMachineBehaviour    //handles the plane moving towards the player at any given moment as well as calling shooting functions
{
    private GameObject player;
    private PlaneManager plane;
    private bool doneMoving = false;    //used to 100% guarantee the plane is not moving forward when it shouldn't
    private bool startedGun = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        plane = animator.GetComponent<PlaneManager>();

        plane.startedSound = false;
        startedGun = false;
        doneMoving = false;     //reset the state of movement when re-entering this state
        plane.runningTime = 0;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //the final point the plane should be aiming for with the new desiredHeight set in PlaneManager
        //Vector3 finishPoint = new Vector3(player.transform.position.x, player.transform.position.y + plane.desiredHeight, player.transform.position.z);
        //plane.Distance = Vector3.Distance(finishPoint, animator.transform.parent.position);

        if (plane.Distance > .3f && doneMoving == false)      //checks if it's far enough to destination and checks to see if it can move
        {
            //Vector3 target = new Vector3(player.transform.position.x, player.transform.position.y + plane.desiredHeight, player.transform.position.z);
            //animator.transform.LookAt(target);
            animator.transform.parent.Translate(animator.transform.forward * plane.speed * Time.deltaTime, Space.World);

            plane.RotateGun();  //makes sure the gun is always looking at the player
        }

        if (Vector3.Distance(animator.transform.parent.position, player.transform.position) < plane.gunRange)
        {
            if (!startedGun)
            {
                plane.RepeatFireGun();  //starts the gun
                startedGun = true;
            }

        }


        if (plane.Distance <= .3f)    //once close enough lock the ability to move and potentially lock the plane to straightMovement as well
        {
            doneMoving = true;
            /*if (!plane.secondTime && plane.straightMovement)
            {
                animator.transform.parent.rotation = Quaternion.Euler(0f, 180f, 0f);    //if needed will lock the plane to a flat y so it doesn't move diagonally
            }*/
            //plane.lastRotation = animator.transform.rotation;
            animator.SetTrigger("Fly");
            
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        plane.TurnOffLook();
        plane.CancelGun();      //makes sure to end the InvokeRepeating of the gun call from the start
        animator.ResetTrigger("Fly");
        plane.StartFlyTimer();
        animator.enabled = false;
    }
}
