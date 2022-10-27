using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSpawner : MonoBehaviour
{
    public GameObject enemy;
    public GameObject _boat;
    public GameObject _plane;
    public GameObject _tank;
    public GameObject _helicopter;

    public void Spawn()
    {
        GameObject instance = Instantiate(enemy, transform.position, transform.rotation);
    }

    public void SetBoat()
    {
        GameObject instance = Instantiate(_boat, transform.position, transform.rotation);
    }

    public void SetTank()
    {
        GameObject instance = Instantiate(_tank, transform.position, transform.rotation);
    }

    public void SetHelicopter()
    {
        GameObject instance = Instantiate(_helicopter, transform.position, transform.rotation);
    }

    public void SetPlane()
    {
        GameObject instance = Instantiate(_plane, transform.position, transform.rotation);
    }
}
