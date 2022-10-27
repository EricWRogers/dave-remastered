using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_Building : MonoBehaviour
{
    public float velocityThreshold = 10.0f;
    public int pointValue = 1;
    public GameObject DestroyedPrefab;
    private bool destroyed = false;
    private GameManager gm;
    private PointManager pm; //declares PointManager Script

    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        //pm = gm.GetComponent<PointManager>(); //gets point manager sets it to pm
        SimplePool.Preload(DestroyedPrefab, 1); //preloads the destroyed version into the pool
        gm.buildingCount++;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger building hit");
        if (other.gameObject.tag == "Player")
        {
            if (!destroyed)
            {
                Debug.Log("Trigger Building destroy");
                breakBuilding();
            }
        }
    }
   

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision building hit");
        if ((collision.gameObject.tag == "Debris" || collision.gameObject.tag == "PlayerProjectile") && !destroyed)
        {
            if (collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude >= velocityThreshold)
            {
                breakBuilding();
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == "FireBreath" && !destroyed)
        {
            breakBuilding();
        }
    }

    private void breakBuilding()
    {
        if (!destroyed)
        {
            Debug.Log("Building destruction started");
            //pm.score += 1; //adds score
            destroyed = true;
            //gm.buildingCount--;
            GameObject temp = SimplePool.Spawn(DestroyedPrefab, transform.position, transform.rotation);
            temp.transform.localScale = gameObject.transform.localScale * 1.2f;
            if(gameObject.TryGetComponent<Building_Healing>(out Building_Healing healing))
            {
                healing.healPlayer();
            }
            if (temp.TryGetComponent<Piece_Manager>(out Piece_Manager manager))
            {
                Debug.Log("Piece Manager found");
                if (manager.loaded)
                {                    
                    manager.init();
                }
            }
            if(gameObject.TryGetComponent<EnemySpawner>(out EnemySpawner spawner))
            {
                spawner.CancelEnemySpawn();
            }
            if (gameObject.TryGetComponent<Color_Picker>(out Color_Picker picker))
            {
                picker.paintObject(temp, destroyed);
            }
            Debug.Log("Building destroyed");
            Destroy(gameObject);
        }
    }

    public bool isDestroyed()
    {
        return destroyed;
    }
}