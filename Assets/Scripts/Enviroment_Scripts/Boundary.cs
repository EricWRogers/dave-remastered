//If you are looking at this code please realign it. VS code is being mean :( also delete this comment when fixed.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
   [SerializeField] 
   private Material myMaterial;

    public Transform player;
    

    void Start() //Transparent off start.
    {
            Color color = myMaterial.color;
            color.a = 0; //Sets to completely transparent. dont change.
            myMaterial.color = color; 
    }

    public void OnTriggerEnter(Collider other)
    {
        
        if(other.CompareTag("Player"))
        {
            Color color = myMaterial.color; //Changes to red
            color.a = 0.75f; //Can change transparancy value here. 100 is opaque. Change as needed
            myMaterial.color = color; 
        }
   }

   public void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Color color = myMaterial.color; //Changes to transparent. Dont change
            color.a = 0;
            myMaterial.color = color; 
        }
    }
}
