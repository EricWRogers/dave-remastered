using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerUp : MonoBehaviour
{
    public int maxTime;
    public float timerCount;
    public int timer;
    public TextMeshPro timerUpText;

    

    public GameManager GameManager;
    void Start()
    {
        timerCount = 0;
    }

    // Update is called once per frame
    void Update()
    {   
        if (GameManager.startAI == true && GameManager.state == GameManager.LevelState.SANDBOX)
        {
            if (timerCount < maxTime)
            {
                timerCount += Time.deltaTime;
                timer = Mathf.RoundToInt(timerCount);
                timerUpText.text = "Time: " + timer;
             }
            else
            {
                timerUpText.text = "Time: " + maxTime;
            }
        }
    }
}
