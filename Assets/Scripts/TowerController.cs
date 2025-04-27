using System;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

/// Author: Brett DeWitt
/// 
/// Created: 04.27.25
/// 
/// <summary>
/// Basic Tower object that fires at enemies
/// </summary>
/// 

public class NewMonoBehaviourScript : MonoBehaviour
{
    private EnemyController currentTarget;
    [SerializeField] private float maxRange = 6f;
    [SerializeField] private float damage = 5f;
    private float currentCooldown = 0f;
    [SerializeField] private float maxCooldown = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (HasTarget() && TargetIsInRange())
        {
            if (WeaponIsReady())
            {
                FireAtTarget();
            } else {
                CoolDownWeapon();
            }
        } else {
            TryToFindNewTarget();
        }
    }

    private bool HasTarget()
    {
        return (currentTarget != null);
    }

    private bool WeaponIsReady()
    {
        return (currentCooldown <= 0);
    }

    private bool TargetIsInRange()
    {
        float targetDistance = Vector3.Distance(transform.position, currentTarget.transform.position);
        return (targetDistance < maxRange);
    }

    private void FireAtTarget()
    {
        Debug.Log($"Firing at {currentTarget.name}");
        currentTarget.TakeDamage(damage);
        currentCooldown = maxCooldown;
    }

    private void TryToFindNewTarget()
    {
        EnemyController[] enemies = GameObject.FindObjectsOfType<EnemyController>();

        foreach (EnemyController enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance <= maxRange)
            {
                currentTarget = enemy;
                Debug.Log($"Found a new target: {currentTarget}!");
                break;
            }
        }
    }

    private void CoolDownWeapon()
    {
        currentCooldown -= Time.deltaTime;
    }

}
