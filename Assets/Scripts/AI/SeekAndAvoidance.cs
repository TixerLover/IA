using UnityEngine;

public class SeekAndAvoidance : MonoBehaviour
{
    [Header("Movement")]
    public Transform target;
    public float maxSpeed = 5f;
    public float maxForce = 10f;
    public float mass = 1f;

    [Header("Obstacle Avoidance")]
    public float lookAheadDistance = 3f;
    public float avoidanceStrength = 15f;
    public LayerMask obstacleLayer;

    private Vector2 velocity;

    void Update()
    {
        if (target == null) return;

        Vector2 currentDirection = velocity.sqrMagnitude > 0.01f ? velocity.normalized : (Vector2)transform.up;
        Vector2 steering = Vector2.zero;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, currentDirection, lookAheadDistance, obstacleLayer);

        if (hit.collider != null) // If we hit a 2D collider
        {
            Vector2 desiredAvoidance = hit.normal * avoidanceStrength;
            steering = desiredAvoidance - velocity;
        }
        else
        {
            Vector2 desiredVelocity = ((Vector2)target.position - (Vector2)transform.position).normalized * maxSpeed;
            steering = desiredVelocity - velocity;
        }

        steering = Vector2.ClampMagnitude(steering, maxForce);
        steering /= mass;

        velocity = Vector2.ClampMagnitude(velocity + steering, maxSpeed);

        transform.position += (Vector3)(velocity * Time.deltaTime);

        if (velocity.sqrMagnitude > 0.01f)
        {
            transform.up = velocity.normalized;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 dir = Application.isPlaying && velocity.sqrMagnitude > 0.01f ? velocity.normalized : (Vector2)transform.up;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + dir * lookAheadDistance);
    }
}
