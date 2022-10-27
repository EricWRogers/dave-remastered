using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Healing : MonoBehaviour
{
    public int healAmount = 10;
    private Health health;

    // Start is called before the first frame update
    void Start()
    {
       // health = GameObject.Find("Player/PlayerBody").GetComponent<Health>();
    }

    public void healPlayer()
    {
        health.Heal(+healAmount);
    }
}
