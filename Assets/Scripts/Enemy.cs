using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform target; // The player or target
    [SerializeField] float viewDistance = 10f; // How far the enemy can "see"
    [SerializeField] float viewAngle = 90f; // Field of view angle
    [SerializeField] float rotationSpeed = 5f; // Speed of rotation when moving
    [SerializeField] float chaseDuration = 3f; // Time to keep chasing after losing sight
    [SerializeField] float chaseSpeed = 3f; // Speed the enemy has when chasing
    [SerializeField] float patrolSpeed = 1.8f; // Speed the enemy has when patrolling
    [SerializeField] List<Transform> patrolPoints; // List of patrol waypoints
    [SerializeField] float waypointThreshold = 1f; // Distance threshold to reach a waypoint

    private NavMeshAgent agent;
    private bool isChasing = false; // Tracks whether the enemy is chasing the player
    private float chaseTimer = 0f; // Tracks time since last sighting
    private int currentPatrolIndex = 0; // Current index of patrol points
    private bool isReturningToPatrol = false; // Flag for returning to patrol after chase

    private void Start()
{
    agent = GetComponent<NavMeshAgent>();
    agent.updateRotation = false; // Disable automatic rotation for custom control
    agent.updateUpAxis = false;   // For top-down rotation

    // Locate the player dynamically if not assigned
    if (target == null)
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogError("Player object not found! Ensure the player GameObject is named 'Player' or adjust the name in the script.");
        }
    }

    // Ensure patrol points exist and start patrolling
    if (patrolPoints.Count > 0)
    {
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    // Adjust the speed of the agent for patrolling
    agent.speed = 2.5f;
}

    private void Update()
    {
        if (CanSeeTarget())
        {
            if (!isChasing) // If not already chasing
            {
                isChasing = true;
                chaseTimer = chaseDuration; // Reset chase timer when the player is seen
                agent.speed = chaseSpeed; // Set speed to 3 for chasing
            }
        }
        else if (isChasing)
        {
            chaseTimer -= Time.deltaTime;
            if (chaseTimer <= 0f)
            {
                isChasing = false;
                isReturningToPatrol = true; // Flag to start returning to patrol
                agent.SetDestination(patrolPoints[currentPatrolIndex].position); // Immediately go to current patrol point
                agent.speed = patrolSpeed; // Set speed to 1.8 for patrolling
            }
        }

        if (isChasing)
        {
            // Chase the player
            agent.SetDestination(target.position);
            RotateTowardsMovementDirection();
        }
        else
        {
            // Check if the enemy is close to the patrol point
            if (Vector3.Distance(transform.position, patrolPoints[currentPatrolIndex].position) <= waypointThreshold)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count; // Cycle through patrol points
                agent.SetDestination(patrolPoints[currentPatrolIndex].position);
            }

            // Return to patrol and rotation
            if (isReturningToPatrol)
            {
                RotateTowardsPatrolPoint();

                // Check if the enemy has reached the current patrol point
                if (Vector3.Distance(transform.position, patrolPoints[currentPatrolIndex].position) <= waypointThreshold)
                {
                    isReturningToPatrol = false; // Stop returning to patrol once the enemy reaches the patrol point
                    agent.SetDestination(patrolPoints[currentPatrolIndex].position); // Continue patrolling from here
                }
            }
            else
            {
                // Rotate towards the patrol point even when not returning to patrol
                RotateTowardsPatrolPoint();
            }
        }

        // Ensure patrol speed is applied when not chasing
        if (!isChasing)
        {
            agent.speed = patrolSpeed; // Set speed to 1.8f when patrolling
        }
    }

    private bool CanSeeTarget()
    {
        // Calculate direction to the target
        Vector3 directionToTarget = (target.position - transform.position).normalized;

        // Check if the player is within view distance
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget > viewDistance)
        {
            Debug.Log("Not distance");
            return false;
        }

        // Check if the player is within the field of view angle
        float angleToTarget = Vector3.Angle(transform.up, directionToTarget);  // transform.up for top-down view
        if (angleToTarget > viewAngle / 2f)
        {
            Debug.Log("Not angle");
            return false;
        }

        return true; // The player is within sight
    }

    private void RotateTowardsMovementDirection()
    {
        // Get the movement direction from the NavMeshAgent's velocity
        Vector3 direction = agent.velocity;

        // Only rotate if the enemy is moving
        if (direction.sqrMagnitude > 0.01f)
        {
            // Calculate the angle for the Z-axis rotation
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Smoothly rotate towards the movement direction
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void RotateTowardsPatrolPoint()
    {
        // Get the direction towards the current patrol point
        Vector3 direction = (patrolPoints[currentPatrolIndex].position - transform.position).normalized;

        // Rotate smoothly towards the patrol point
        if (direction.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 leftBoundary = Quaternion.Euler(0, 0, viewAngle / 2) * transform.up;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, -viewAngle / 2) * transform.up;

        Gizmos.DrawRay(transform.position, leftBoundary * viewDistance);
        Gizmos.DrawRay(transform.position, rightBoundary * viewDistance);
    }
}
