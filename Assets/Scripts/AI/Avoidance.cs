using UnityEngine;

public class Avoidance : MonoBehaviour
{
    [Header("Target Setup")]
    public Transform target;

    [Header("Avoidance Settings")]
    public float moveSpeed = 6f;
    public float rotationSpeed = 15f;

    [Tooltip("Distancia a la que la IA se asusta y empieza a huir")]
    public float panicDistance = 5f;

    [Header("Sprite Setup")]
    public float angleOffset = -90f;

    void Update()
    {
        if (target == null) return;

        // Vector2.Distance es para saber qué tan cerca están
        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        if (distanceToTarget < panicDistance)
        {
            Vector2 fleeDirection = transform.position - target.position;

            fleeDirection = fleeDirection.normalized;
            transform.position += (Vector3)(fleeDirection * moveSpeed * Time.deltaTime);

            if (fleeDirection != Vector2.zero)
            {
                float angle = Mathf.Atan2(fleeDirection.y, fleeDirection.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0, 0, angle + angleOffset);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, panicDistance);
    }
}
