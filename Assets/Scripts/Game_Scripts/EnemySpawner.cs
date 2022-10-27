using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//#if UNITY_EDITOR
//using UnityEditor;
//#endif

public class EnemySpawner : MonoBehaviour
{
    [Header("Variables")]
    public float spawnInterval = 5f;
    [Tooltip("Spawn Selected Enemy from the Start of the Game on the Spawn Interval (Starts Before Play Button)")] 
    public bool spawnOnStart = false;
    public Transform spawnPosition;

    [Header("Type of Enemy")]
    public bool _helicopter;
    public bool _tank;
    public bool _plane;
    public bool _boat;

    [Header("Enemy GameObject")]
    public GameObject helicopter;
    public GameObject tank;
    public GameObject plane;
    public GameObject boat;

    private MainMenu menu;
    private GameManager gManager;
    private bool started = false;

    void Start()
    {
        gManager = FindObjectOfType<GameManager>();
        started = false;
        if (spawnOnStart)
            SpawnEnemyOnInterval();

        if (spawnPosition == null)
        {
            spawnPosition = transform;
        }
    }

    private void Update()
    {
        if (gManager.state == GameManager.LevelState.MILITARY || gManager.state == GameManager.LevelState.SANDBOX)
        {
            if (gManager.startAI)
            {

                if (!started)
                {
                    SpawnEnemyOnInterval();
                    started = true;
                }
            }
        }
    }

    void SpawnEnemyOnInterval()
    {
        InvokeRepeating("SpawnEnemy", 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (_helicopter)
        {
            Instantiate(helicopter, spawnPosition.position, spawnPosition.rotation);
        }

        if (_tank)
        {
            Instantiate(tank, spawnPosition.position, spawnPosition.rotation);
        }

        if (_plane)
        {
            Instantiate(plane, spawnPosition.position, spawnPosition.rotation);
        }

        if (_boat)
        {
            Instantiate(boat, spawnPosition.position, spawnPosition.rotation);
        }
    }

    public void CancelEnemySpawn()
    {
        CancelInvoke("SpawnEnemyOnInterval");
    }

    public void ResumeEnemySpawn()
    {
        started = false;
    }
}

//#if UNITY_EDITOR
//[CustomEditor(typeof(EnemySpawner))]
//public class EnemySpawnEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector(); // for other non-HideInInspector fields

//        EnemySpawner script = (EnemySpawner)target;

//        // draw checkbox for the bool
//        if (!script._tank && !script._plane && !script._boat)
//            script._helicopter = EditorGUILayout.Toggle("Is Helicopter?", script._helicopter);
//        if (script._helicopter) // if bool is true, show other fields
//        {
//            script.helicopter = (GameObject)EditorGUILayout.ObjectField("Helicopter Prefab", script.helicopter, typeof(GameObject), true);
//        }

//        if (!script._helicopter && !script._plane && !script._boat)
//            script._tank = EditorGUILayout.Toggle("Is Tank?", script._tank);
//        if (script._tank)
//        {
//            script.tank = (GameObject)EditorGUILayout.ObjectField("Tank Prefab", script.tank, typeof(GameObject), true);
//        }

//        if (!script._tank && !script._helicopter && !script._boat)
//            script._plane = EditorGUILayout.Toggle("Is Plane?", script._plane);
//        if (script._plane)
//        {
//            script.plane = (GameObject)EditorGUILayout.ObjectField("Plane Prefab", script.plane, typeof(GameObject), true);
//        }
        
        
//        if (!script._tank && !script._plane && !script._helicopter)
//            script._boat = EditorGUILayout.Toggle("Is Boat?", script._boat);
//        if (script._boat)
//        {
//            script.boat = (GameObject)EditorGUILayout.ObjectField("Boat Prefab", script.boat, typeof(GameObject), true);
//        }
//    }
//}
//#endif
