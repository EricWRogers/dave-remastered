using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandboxDestroy : MonoBehaviour
{
    public float velocityThreshold = 10.0f;
    public GameObject DestroyedPrefab;
    private GameManager gm;
    private PointManager pm; //declares PointManager Script
    private SandboxBuildingSystem sbs; //Sandbox system, for respawning buildings in sandbox mode

    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        pm = gm.GetComponent<PointManager>(); //gets point manager sets it to pm
        if (gm != null)
        {
            Debug.Log("game manager found");
        }
        gm.buildingCount++;
        sbs = GameObject.Find("SandboxBuildingController").GetComponent<SandboxBuildingSystem>();
        GetComponent<Animator>().SetTrigger("Start");
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger building hit");
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Trigger Building destroy");
            breakBuilding();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision tag:" + collision.gameObject.tag);
        //Debug.Log("Collision building hit");
        if (collision.gameObject.tag == "Debris" || collision.gameObject.tag == "PlayerProjectile")
        {
            if (collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude >= velocityThreshold)
            {
                //Debug.Log("Trigger Building destroy " + collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude);
                breakBuilding();
            }
        }
    }

    private void breakBuilding()
    {
        //Debug.Log("Building destruction started");
        pm.score += 1; //adds score
        gm.buildingCount--;
        GameObject temp = SimplePool.Spawn(DestroyedPrefab, transform.position, transform.rotation);
        temp.transform.localScale = gameObject.transform.localScale;
        if (temp.TryGetComponent<Piece_Manager>(out Piece_Manager manager))
        {
            if (manager.loaded)
            {
                manager.init();
            }
        }
        if (gameObject.TryGetComponent<Building_Healing>(out Building_Healing healing))
        {
            healing.healPlayer();
        }
        if (gameObject.TryGetComponent<EnemySpawner>(out EnemySpawner spawner))
        {
            spawner.CancelEnemySpawn();
        }
        if (gameObject.TryGetComponent<Color_Picker>(out Color_Picker picker))
        {
            picker.paintObject(temp, true);
        }

        sbs.destroyedBuildings.Add(new KeyValuePair<GameObject, float>(gameObject, Time.realtimeSinceStartup));
        gameObject.SetActive(false);

    }

    public void turnOffColiders()
    {
        Component[] colliders = gameObject.GetComponents(typeof(BoxCollider));
        foreach(BoxCollider box in colliders)
        {
            box.enabled = false;
        }
    }
    public void turnOnColiders()
    {
        Component[] colliders = gameObject.GetComponents(typeof(BoxCollider));
        foreach (BoxCollider box in colliders)
        {
            box.enabled = true;
        }
    }
}
