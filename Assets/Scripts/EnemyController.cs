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
// waypoints of the lane. The Enemy will visit each of the waypoints in
// sequence until all waypoints have been visited, at which point the goal
// sequence will be triggered. If the Enemy loses all of its health, the
// death sequence will be triggered.

// To assign a lane to the Enemy, use the public function SetLane().
// Example:
//    GameObject enemyGO = Instantiate(enemyPrefab,
//                                     spawnPoint.position,
//                                     spawnPoint.rotation);
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
    [SerializeField] private float health = 100;


    // Start is called once before the first execution of Update after the
    // MonoBehaviour is created
    void Start()
    {
        GetWaypointsFromLane();
        SetNextWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (AtCurrentWaypoint())
        {
            AlignWithWaypoint();
            if (VisitedAllWaypoints())
            {
                HandleReachedGoal();
            }
            else
            {
                SetNextWaypoint();
            }
        }
        MoveTowardWaypoint();
    }


    //
    // Public API functions
    //


    // Sets the lane for the enemy to follow. Children of lane will be treated
    // as waypoints.
    // If lane has no waypoints, enemy will be destroyed.
    // Throws ArgumentNullException if lane is Null.
    public void SetLane(Transform lane)
    {
        if (lane == null)
        {
            throw new ArgumentNullException("Lane must not be null");
        }
        this.lane = lane;

        GetWaypointsFromLane();

        if (waypoints.Count <= 0)
        {
            Debug.Log("Lane has no waypoints. Enemy cannot proceed and will" +
                      "be destroyed.");
            Die();
        }
    }

    // Reduces enemy's health by the specified damage amount. When health falls
    // below zero, triggers the handler for losing all health.
    public void TakeDamage(float damage)
    {
        if (damage < 0)
        {
            Debug.Log("Damage must be a non-negative value. Setting damage" +
                      "to 0.");
            damage = 0;
        }
        health -= damage;
        Debug.Log($"Ouch! I took {damage} damage!");
        if (health < 0)
        {
            HandleLostAllHealth();
        }
    }

    // Returns the current remaining health of the enemy
    public float GetCurrentHealth()
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
        if (lane == null)
        {
            Debug.Log("GetWaypointsFromLane must have non-null lane. Use" +
                "SetLane() to assign a lane to Enemy.");
        }
        else
        {
            waypoints.Clear();
            foreach (Transform waypoint in lane)
            {
                waypoints.Add(waypoint);
            }
        }
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
        if (waypoints == null)
        {
            Debug.Log("Cannot set next waypoint. Waypoints is null.");
            Die();
            return;
        }

        if (waypoints.Count == 0)
        {
            Debug.Log("Cannot set next waypoint. Waypoints list is empty.");
            Die();
            return;
        }

        if (waypointIndex < 0 || waypointIndex >= waypoints.Count)
        {
            Debug.Log("Cannt set next waypoint. Index is out of range.");
            Die();
            return;
        }

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
        if (currentWaypoint == null)
        {
            Debug.Log("No current waypoint set.");
            return false;
        }
        else
        {
            float margin = 0.1f;
            float distanceFromWaypoint = Vector3.Distance(transform.position,
                                                          destination);
            return (distanceFromWaypoint < margin);
        }
    }

    // Returns true if the last visited waypoint was the final waypoint in the
    // waypoints list
    private bool VisitedAllWaypoints()
    {
        return (waypointIndex >= lane.childCount);
    }

    // Handles logic when the enemy has visited the final waypoint
    private void HandleReachedGoal()
    {
        // penalize player
        // animation?
        // play sounds?
        // destroy self
        Die();
    }

    // Moves towards the current waypoint
    private void MoveTowardWaypoint()
    {
        Vector3 moveTowards = Vector3.MoveTowards(transform.position,
                                                  destination,
                                                  speed * Time.deltaTime);
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


