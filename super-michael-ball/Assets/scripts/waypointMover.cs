using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waypointMover : MonoBehaviour
{
    [SerializeField] private waypoints waypoints;
    [SerializeField] private float moveSpeed = 5f;

    private Transform currentWaypoint;
    // Start is called before the first frame update
    void Start()
    {
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        transform.position = currentWaypoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);
    }
}
