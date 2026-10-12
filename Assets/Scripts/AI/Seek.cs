using UnityEngine;

public class Seek : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float stoppingDistance = 1.5f;
    public float angleOffset = -90f;

    void Update()
    {
        if (target == null) return;

        Vector2 direction = target.position - transform.position;
        float distance = direction.magnitude;

        if (distance > stoppingDistance)
        {
            Vector2 moveDirection = direction.normalized;

            transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime);

            if (moveDirection != Vector2.zero)
            {
                float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0, 0, angle + angleOffset);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
