using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementNoise : MonoBehaviour
{
    
    CharacterController cc;

    public float minSpeed = 2f;
    public float timeBetweenSteps = 3f;

    public float softestStep = 0.8f;
    public float lowestPitch = 0.8f;
    public float highestPitch = 1.2f;

    public GameObject StepFX;

    private AudioSource monsterNoises;

    private float timeElapsed = 0f;

    private Vector3 lastPosition;

    void Start()
    {
        monsterNoises = GetComponent<AudioSource>();
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 lastPosition = transform.position;

        if (cc.isGrounded == true && cc.velocity.magnitude > minSpeed && timeElapsed >= timeBetweenSteps)
        {
            monsterNoises.volume = Random.Range(softestStep, 1);
            monsterNoises.pitch = Random.Range(lowestPitch, highestPitch);
            monsterNoises.Play(0);
            Instantiate(StepFX, new Vector3(lastPosition.x, lastPosition.y - 1, lastPosition.z), StepFX.transform.rotation);
            timeElapsed = 0f;
        }
        else
        {
            timeElapsed += Time.deltaTime;
        }
       
    }

}
