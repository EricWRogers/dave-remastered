using UnityEngine;

public class FIREBallController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileLifespan = 5.0f;
    public float cooldownTime = 4;

    private float nextFireTime = 0;
    public audioManager audioData;

    void Start() 
    {
        audioData = FindObjectOfType<audioManager>(); //call audio manager
    }

    void Update()
    {
        if(Time.time > nextFireTime)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                audioData.Play("MonsterFireBall_1"); //play sound

                var projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
                Destroy(projectile, projectileLifespan);
                Debug.Log("ability used, cooldown started"); //use debug.log u nerd
                nextFireTime = Time.time + cooldownTime;
            }
        }
    }
}
