using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 10f;
    private float rotatespeed = 100f;

    // Update is called once per frame
    void Update()
    {
        move();
    }
    void move()
    {
        //Input.GetKeyDown will move us forwards with every button press.
        //GetKey will allow us to hold the button.
        if (Input.GetKey(KeyCode.UpArrow))//This is the specific code for the up arrow.
        {
            //transform.Translate(Vector3.forward); This is the most simple way to move. it will jump forwards with every button press or button hold, depending on what we decided in the if statement.
            transform.Translate(Vector3.forward * speed * Time.deltaTime); //We multiply the forwards movement times speed and then times the time in the game engine.
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.up * rotatespeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.down * rotatespeed * Time.deltaTime);
        }
    }
}
