/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{

  public Animator anim;
  public GameObject cageAnim;
    void Start()
    {
        cageAnim = GameObject.Find("Default/Cage");
        anim = cageAnim.GetComponent<Animator>();
    }
    
    public void OnButtonPressed()
   {
       Debug.Log("ButtonPressed");
       anim.SetBool("Open", true); 
        
   }
}
*/