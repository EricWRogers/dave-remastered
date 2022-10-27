using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SpawnPeople : MonoBehaviour
{
    public GameObject personPrefab;
    List<GameObject> spawnPoints = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        GameObject currentObject;
        spawnPoints.AddRange(GameObject.FindGameObjectsWithTag("SpawnPoint"));
        int spawnPointNumber = 0;
        for (int i = 0; i < 10; i++) {
            spawnPointNumber = new Random().Next(0, spawnPoints.Count);
            Instantiate(personPrefab, spawnPoints[spawnPointNumber].transform.position, Quaternion.identity);
            spawnPoints.Remove(spawnPoints[spawnPointNumber]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
