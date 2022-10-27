using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate45 : MonoBehaviour
{
    bool found = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Ray Origin

    // Update is called once per frame
    void Update()
    {
        if (!found) {
            bool rayFound = false;
            Debug.Log("Look");
            int count = 0;

            foreach(Transform trans in transform.GetComponentInChildren<Transform>()) {
                if(transform == trans.parent) {
                    count++;
                }
            }

            Debug.Log("Count : " + count);

            if (count == 4) {
                count = 0;
                Debug.Log("Ray Found");
                foreach(Transform trans in transform.GetComponentInChildren<Transform>()) {
                    if(transform == trans.parent) {
                        count++;
                        if (count == 4)
                            trans.Rotate(new Vector3(45.0f,0.0f,0.0f));
                    }
                }
                found = true;
            }
        }
        
    }
}
