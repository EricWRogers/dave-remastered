using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private GameObject player;
    private PlaneManager plane;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        plane = GetComponentInChildren<PlaneManager>();
    }

    void Update()
    {
        Vector3 target = new Vector3(player.transform.position.x, player.transform.position.y + plane.desiredHeight, player.transform.position.z);
        transform.parent.LookAt(target);
    }
}
