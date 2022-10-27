using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalSceneChange : MonoBehaviour
{
    public bool SandboxLevel;
    public bool DefaultLevel;
    public bool TutorialLevel;


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collision building hit");
        if (other.gameObject.tag == "Player")
        {
            Teleport();
        }
    }

    void Teleport()
    {
        if (SandboxLevel)
        {

        }
        else if (DefaultLevel)
        {
            SceneManager.LoadScene(1);
        }
        else if (TutorialLevel)
        {
            SceneManager.LoadScene(0);
        }
    }
}
