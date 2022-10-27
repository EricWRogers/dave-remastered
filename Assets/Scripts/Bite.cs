using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bite : MonoBehaviour
{
 
    public audioManager audioData;
    public GameObject playerBody;
    public Health HH;
    public GameObject Rhands;
    public GameObject Lhands;
    //public OVRGrabber ROVRG;
    //public OVRGrabber LOVRG;
    void Start() //Runs when game starts
    {
        playerBody = GameObject.Find("Player/PlayerBody");
        Rhands = GameObject.Find("Player/OVRCameraRig/TrackingSpace/RightHandAnchor/CustomHandRight");
        Lhands = GameObject.Find("Player/OVRCameraRig/TrackingSpace/LeftHandAnchor/CustomHandLeft");
        audioData = FindObjectOfType<audioManager>(); //Gets component on the game object w the script
        //ROVRG = Rhands.GetComponent<OVRGrabber>();
        //LOVRG = Lhands.GetComponent<OVRGrabber>();
        HH = playerBody.GetComponent<Health>();

    }
    

    private void OnTriggerEnter(Collider other) //Runs when we collide with anything that has a trigger
    {
        if(other.CompareTag("Debris"))
        {
            //ROVRG.OnDestroy();
            //LOVRG.OnDestroy();
            //ROVRG.ForceRelease(OVRGrabbable grabbable);//This should turn off the grab mechanic and not absolutly freak out the Vr grab rig when we eat stuf.
            //LOVRG.ForceRelease(OVRGrabbable grabbable);
            Debug.Log("Checkpoint1 "); //Displays info into the console.
            other.gameObject.SetActive(false); //Destroys gameobject we collided with

            audioData.Play("Bite"); //Plays audio
            HH.currentHealth += 20; //Adds 20 to health
            Debug.Log("Heallth= "); //Displays info into the console.
            
        }

    }

}

