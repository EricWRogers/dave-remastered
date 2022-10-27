using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandboxBuildingSystem : MonoBehaviour
{
    public List<KeyValuePair<GameObject, float>> destroyedBuildings = new List<KeyValuePair<GameObject, float>>();
    //public GameObject buildingPrefab;
    public float cooldown = 60.0f;

    public void Update()
    {
        if (destroyedBuildings.Count > 0) {
            
            //Debug.Log("Update Sandbox");
            List<KeyValuePair<GameObject, float>> buildingsToRemove = new List<KeyValuePair<GameObject, float>>();
            foreach (KeyValuePair<GameObject, float> currentBuilding in destroyedBuildings)
            {
                Debug.Log(currentBuilding.Key);
                //Debug.Log(Time.realtimeSinceStartup - currentBuilding.Value);
                if (Time.realtimeSinceStartup >= currentBuilding.Value + cooldown)
                {
                    buildingsToRemove.Add(currentBuilding);
                    currentBuilding.Key.SetActive(true);
                    currentBuilding.Key.GetComponent<Animator>().SetTrigger("Start");
                }
            }
            foreach (KeyValuePair<GameObject, float> currentBuilding in buildingsToRemove)
            {
                destroyedBuildings.Remove(currentBuilding);
            }
        }
    }
}
