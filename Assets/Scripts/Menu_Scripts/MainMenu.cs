using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator anim;
    public GameObject cageAnim;
    public bool startAI = false;

    void Start()
    {
        FindObjectOfType<audioManager>().Play("MainMenuMusic");
        cageAnim = GameObject.Find("DefaultLevel/Door");
        anim = cageAnim.GetComponent<Animator>();

    }

    public void PlayGame()
    {
        startAI = true;
        Debug.Log("ButtonPressed");
        anim.SetBool("Open", true);
        FindObjectOfType<audioManager>().Stop("MainMenuMusic");
        FindObjectOfType<audioManager>().Play("Wasteland Showdown");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

}
