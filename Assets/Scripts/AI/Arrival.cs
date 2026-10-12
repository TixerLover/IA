using UnityEngine;

public class Arrival2D : MonoBehaviour
{
    [Header("Control de Encendido")]
    public bool isActive = true; // Define si el agente se mueve o no

    [Header("Configuración del Agente")]
    public Transform target;
    public float maxSpeed = 5f;
    public float maxForce = 0.1f;
    public float mass = 1f;

    [Header("Configuración del Arrival")]
    public float slowingRadius = 3f;
    public float stoppingDistance = 0.1f;

    [Header("Búsqueda de Ruta y Sensores (Avoidance)")]
    public float sensorDistance = 3f;
    public float sensorRadius = 0.4f;
    public LayerMask obstacleLayer;

    private Vector2 velocity;
    private readonly float[] anglesToTest = { 0f, 15f, -15f, 30f, -30f, 45f, -45f, 60f, -60f, 90f, -90f };

    void Update()
    {
        // --- NUEVO: Apagar y prender con la tecla '1' (teclado alfanumérico) ---
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isActive = !isActive; // Invierte el estado (si estaba prendido, lo apaga y viceversa)

            if (!isActive)
            {
                velocity = Vector2.zero; // Frena al agente inmediatamente al apagarlo
            }
        }

        // Si está apagado o no hay objetivo, no calculamos nada más
        if (!isActive || target == null) return;

        Vector2 currentPos = transform.position;
        Vector2 targetPos = target.position;

        // --- 1. Calcular la dirección deseada hacia el objetivo ---
        Vector2 targetOffset = targetPos - currentPos;
        float distance = targetOffset.magnitude;

        if (distance < stoppingDistance)
        {
            velocity = Vector2.zero;
            return;
        }

        float desiredSpeed = maxSpeed;
        if (distance < slowingRadius)
        {
            desiredSpeed = maxSpeed * (distance / slowingRadius);
        }

        Vector2 idealDirection = targetOffset.normalized;
        Vector2 bestClearDirection = idealDirection;

        // --- 2. Buscar la Mejor Ruta (Esquivar Obstáculos) ---
        bool pathFound = false;

        foreach (float angle in anglesToTest)
        {
            Vector2 testDirection = RotateVector(idealDirection, angle);
            RaycastHit2D hit = Physics2D.CircleCast(currentPos, sensorRadius, testDirection, sensorDistance, obstacleLayer);

            if (hit.collider == null)
            {
                bestClearDirection = testDirection;
                pathFound = true;
                break;
            }
        }

        if (!pathFound)
        {
            bestClearDirection = Vector2.zero;
        }

        // --- 3. Aplicar las fuerzas de movimiento (Steering) ---
        Vector2 desiredVelocity = bestClearDirection * desiredSpeed;
        Vector2 steering = desiredVelocity - velocity;

        steering = Vector2.ClampMagnitude(steering, maxForce);
        steering /= Mathf.Max(mass, 0.1f);

        velocity += steering;
        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);

        transform.position += (Vector3)velocity * Time.deltaTime;

        // --- 4. Rotar el Sprite ---
        if (velocity != Vector2.zero)
        {
            float angleZ = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angleZ - 90f);
        }
    }

    private Vector2 RotateVector(Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        return new Vector2(
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos
        );
    }

    private void OnDrawGizmosSelected()
    {
        if (target == null) return;

        Vector2 currentPos = transform.position;
        Vector2 idealDirection = (target.position - transform.position).normalized;

        foreach (float angle in anglesToTest)
        {
            Vector2 testDir = RotateVector(idealDirection, angle);
            RaycastHit2D hit = Physics2D.CircleCast(currentPos, sensorRadius, testDir, sensorDistance, obstacleLayer);

            if (hit.collider != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(currentPos, currentPos + testDir * hit.distance);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(currentPos, currentPos + testDir * sensorDistance);
                break;
            }
        }
    }
}