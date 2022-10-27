using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthpack : MonoBehaviour
{
    public GameObject playerBody;
    public Health HH;
    // Start is called before the first frame update
    void Start()
    {
        playerBody = GameObject.Find("Player/PlayerBody");
        HH = playerBody.GetComponent<Health>();
    }

    // Update is called once per frame
   private void OnTriggerEnter(Collider other) //Runs when we collide with anything that has a trigger
    {
        if(other.CompareTag("Player"))
        {
            HH.currentHealth += 20;
        }
    }
}
