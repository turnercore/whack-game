using UnityEngine;

public class EnemyBrainPatrol : EnemyBrain
{
    public Vector2[] patrolPoints; // Points for patrolling
    public float waypointTolerance = 0.2f; // Distance to consider reached
    private int currentWaypoint = 0;

    protected override void Start()
    {
        base.Start();
        DefinePatrolPoints();
    }

    private void DefinePatrolPoints()
    {
        // Get screen boundaries in world units
        Vector2 screenMin = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 screenMax = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        // Randomly determine a spawn point off-screen
        Vector2 spawnPoint;
        int side = Random.Range(0, 4); // 0 = top, 1 = bottom, 2 = left, 3 = right

        switch (side)
        {
            case 0: // Top
                spawnPoint = new Vector2(Random.Range(screenMin.x, screenMax.x), screenMax.y + 1);
                break;
            case 1: // Bottom
                spawnPoint = new Vector2(Random.Range(screenMin.x, screenMax.x), screenMin.y - 1);
                break;
            case 2: // Left
                spawnPoint = new Vector2(screenMin.x - 1, Random.Range(screenMin.y, screenMax.y));
                break;
            case 3: // Right
                spawnPoint = new Vector2(screenMax.x + 1, Random.Range(screenMin.y, screenMax.y));
                break;
            default:
                spawnPoint = Vector2.zero;
                break;
        }

        // Set the enemy's starting position
        transform.position = spawnPoint;

        // Determine the opposite point off-screen
        Vector2 oppositePoint;
        switch (side)
        {
            case 0: // Top
                oppositePoint = new Vector2(spawnPoint.x, screenMin.y - 1);
                break;
            case 1: // Bottom
                oppositePoint = new Vector2(spawnPoint.x, screenMax.y + 1);
                break;
            case 2: // Left
                oppositePoint = new Vector2(screenMax.x + 1, spawnPoint.y);
                break;
            case 3: // Right
                oppositePoint = new Vector2(screenMin.x - 1, spawnPoint.y);
                break;
            default:
                oppositePoint = Vector2.zero;
                break;
        }

        // Set patrol points
        patrolPoints = new Vector2[] { spawnPoint, oppositePoint };
    }

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
            currentWaypoint = (currentWaypoint + 1) % patrolPoints.Length; // Loop through patrol points
        }
    }
}
