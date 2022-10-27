using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerDown : MonoBehaviour
{
    public float timerCount;
    private int timer;
    public TextMeshPro timerDownText;
    
  
    public GameManager GameManager;
    // Update is called once per frame
    void Update()
    {
        timer = Mathf.RoundToInt(timerCount);
        //timerDownText.text = "Time left: " + timer;

        if (GameManager.startAI == true && (GameManager.state == GameManager.LevelState.DEFAULT || GameManager.state == GameManager.LevelState.MILITARY))
        {
            if (timerCount > 0)
            {
                timerCount -= Time.deltaTime;
                //timer = Mathf.RoundToInt(timerCount);
                //timerDownText.text = "Time left: " + timer;
            }
            else
            {
                timerDownText.text = "Time left: 0";
            }
        }
    }
}
