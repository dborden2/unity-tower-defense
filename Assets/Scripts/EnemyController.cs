using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System;
using static UnityEngine.EventSystems.EventTrigger;

// Author: Brett DeWitt

// Created: 4.24.2025

// Description:

// Prototype for basic enemy creep.
// Must be assigned a Lane. The Lane must have child objects which comprise the
// waypoints of the lane. The enemy will visit each of the waypoints in
// sequence until all waypoints have been visited, at which point
// HandleReachedGoal() will be called.

// To assign a lane to the Enemy, use the public function SetLane().
// Example:
//    GameObject enemyGO = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
//    EnemyController enemy = enemyGO.GetComponent<EnemyController>();
//    enemy.SetLane(laneTransform);

// Damage can be dealt to the enemy by using the pulblic function TakeDamage()
// Example:
//    EnemyController enemy = hitEnemyGameObject.GetComponent<EnemyController>();
//    enemy.TakeDamage(10);


public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform lane;
    [SerializeField] private List<Transform> waypoints = new();
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
    // Public API functions
    //


    // Sets the lane for the enemy to follow. Children of lane will be treated
    // as waypoints
    public void SetLane(Transform lane)
    {
        this.lane = lane;
        GetWaypointsFromLane();
    }

    // Reduces enemy's health by the specified damage amount. When health falls
    // below zero, triggers the handler for losing all health.
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            HandleLostAllHealth();
        }
    }

    // Returns the current remaining health of the enemy
    public int GetCurrentHealth()
    {
        return health;
    }


    //
    // Private Helper functions
    //


    // Adds every child of lane to list of waypoints for enemy to follow.
    // The lane must first be set with SetLane()
    private void GetWaypointsFromLane()
    {
        foreach (Transform waypoint in lane) {
            waypoints.Add(waypoint);
        }
        // Handle cases
        //   no lane is set
        //   lane has no waypoints
    }

    // Sets current position to the current waypoint, making sure enemy
    // accuratey follows the path
    private void AlignWithWaypoint()
    {
        transform.position = destination;
        // Handle cases
        //   waypoint is null
    }

    // Sets the current waypoint to the next in the waypoints list, and
    // unpacks its x and z coordinates into a destination vector for the
    // enemy to move toward. The y coordinate is kept the same to ensure
    // that the enemy's height does not change.
    private void SetNextWaypoint()
    {
        currentWaypoint = waypoints[waypointIndex];
        destination = new Vector3(currentWaypoint.transform.position.x,
                                  transform.position.y,
                                  currentWaypoint.transform.position.z);
        waypointIndex += 1;

        // Error handling could include
        //  trying to set the next waypoint
        //  trying to set the final waypoint,
        //  calling HandleReachedGoal()
        //  calling HandleLostAllHealth()
        //  or just Die()
    }

    // Returns true if the enemy's distance to the current waypoint is within
    // a specified margin
    private bool AtCurrentWaypoint()
    {
        float margin = 0.1f;
        float distanceFromWaypoint = Vector3.Distance(transform.position, destination);
        return (distanceFromWaypoint < margin);
        // Handle cases
        //   no current waypoint set
    }

    // Returns true if the last visited waypoint was the final waypoint in the
    // waypoints list
    private bool VistiedAllWaypoints()
    {
        return (waypointIndex >= lane.childCount);
    }

    // Handles logic when the enemy has visited the final waypoint
    private void HandleReachedGoal()
    {
        // penalize player, etc
        // animation?
        // play sounds?
        // destroy self
        Die();
    }

    // Moves towards the current waypoint
    private void MoveTowardWaypoint()
    {
        Vector3 moveTowards = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        transform.position = moveTowards;
    }

    // Handles the logic when enemy's health falls below zero
    private void HandleLostAllHealth()
    {
        // Did we die, or do we do something special when we lose all health?
        //  e.g. self destruct, buff friends, etc.
        // Reward player
        // destroy self
        Die();
    }

    // Destroys the game object, removing it from the game
    private void Die()
    {
        Destroy(gameObject);
    }

}
