using UnityEngine;

public class EnemyBrainPatrol : EnemyBrain
{
    public Vector2[] patrolPoints;  // Points for patrolling
    public float waypointTolerance = 0.2f;  // Distance to consider reached

    private int currentWaypoint = 0;

    protected override void Act()
    {
        // Check if patrol points are defined
        if (patrolPoints.Length == 0)
        {
            Debug.LogWarning("No patrol points defined for enemy.");
            return;
        }

        // Get the current target patrol point
        Vector2 targetPoint = patrolPoints[currentWaypoint];

        // Move towards the current patrol point
        Vector2 direction = (targetPoint - (Vector2)transform.position).normalized;
        MoveTowards(direction);

        // Check if close enough to switch to the next patrol point
        if (Vector2.Distance(transform.position, targetPoint) < waypointTolerance)
        {
            currentWaypoint = (currentWaypoint + 1) % patrolPoints.Length;  // Loop through patrol points
        }
    }
}
