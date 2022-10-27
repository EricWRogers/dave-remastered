using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnEnemy : MonoBehaviour
{
    public float shrinkDelay = 5.0f;
    public float despawnTime = 10.0f;
    public float shrinkRate = 0.3f;
    public float scaleCutOff = .05f;

    private float delayTimer = 0.0f;
    private float despawnTimer = 0.0f;
    private float baseDelay;
    private float baseRate;
    private bool hasExited = false;
    private bool start = false;

    private void Start()
    {
        baseDelay = shrinkDelay;
        baseRate = shrinkRate;
    }

    public void StartShrink()
    {
        start = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (start)
        {
        if (delayTimer > shrinkDelay)
        {
            if (despawnTimer >= despawnTime || gameObject.transform.localScale.x <= scaleCutOff
                || gameObject.transform.localScale.y <= scaleCutOff || gameObject.transform.localScale.z <= scaleCutOff)
            {
                gameObject.SetActive(false);
            }
            gameObject.transform.localScale += new Vector3(1, 1, 1) * -shrinkRate * Time.deltaTime;
            despawnTimer += Time.deltaTime;

        }
        else
        {
            delayTimer += Time.deltaTime;
        }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Destroy" && hasExited)
        {
            shrinkDelay *= 2.0f;
            shrinkRate /= 2.0f; 
            hasExited = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Destroy" && !hasExited)
        {
            shrinkDelay /= 2.0f;
            shrinkRate *= 2.0f;
            hasExited = true;
        }
    }

    public void init()
    {
        delayTimer = 0.0f;
        despawnTimer = 0.0f;
        shrinkDelay = baseDelay;
        shrinkRate = baseRate;
        hasExited = false;
    }
}

