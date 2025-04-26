using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform lane;
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    [SerializeField] private int waypointIndex = 0;
    private Vector3 destination;
    private Transform currentWaypoint;

    [SerializeField] private float speed = 4f;
    [SerializeField] private int health = 100;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetWaypointsFromLane();
        SetNextWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (AtCurrentWaypoint()) {
            AlignWithWaypoint();
            if (VistiedAllWaypoints())
            {
                HandleReachedGoal();
            } else {
                SetNextWaypoint();
            }
        }
        MoveTowardWaypoint();
    }

    //
    // Helper functions
    //

    void GetWaypointsFromLane()
    {
        foreach (Transform waypoint in lane) {
            waypoints.Add(waypoint);
        }
    }

    void AlignWithWaypoint()
    {
        transform.position = destination;
    }

    void SetNextWaypoint()
    {
        currentWaypoint = waypoints[waypointIndex];
        destination = new Vector3(currentWaypoint.transform.position.x,
                                  transform.position.y,
                                  currentWaypoint.transform.position.z);
        waypointIndex += 1;

        // Error handling could include
        //  trying to set it to the next waypoint
        //  trying to set it to the final waypoint,
        //  calling HandleReachedGoal()
        //  calling KilledByPlayer() or whatever we end up with
    }

    bool AtCurrentWaypoint()
    {
        float distanceFromWaypoint = Vector3.Distance(transform.position, destination);
        return (distanceFromWaypoint < 0.1f);
    }

    bool VistiedAllWaypoints()
    {
        return (waypointIndex >= lane.childCount);
    }

    void HandleReachedGoal()
    {
        // penalize player, etc
        // animation?
        // play sounds?
        // destroy self
        Destroy(gameObject);
    }

    void MoveTowardWaypoint()
    {
        Vector3 moveTowards = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        transform.position = moveTowards;
    }

}
