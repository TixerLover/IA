using UnityEngine;

public class Wander : MonoBehaviour
{
    [Header("Toggle Settings")]
    public bool isActive = true; // Define si empieza encendido o apagado

    [Header("Wander Settings")]
    public float moveSpeed = 3f;
    public float rotationSpeed = 10f;
    public float wanderRadius = 5f;

    [Header("Wait Settings")]
    public float minWaitTime = 1f;
    public float maxWaitTime = 3f;

    [Header("Sprite Setup")]
    public float angleOffset = -90f;

    [Header("Obstacle Avoidance")]
    public float detectionDistance = 1.5f; // Distancia a la que detecta paredes
    public LayerMask obstacleLayer;        // Capa de los obstáculos
    public int numRays = 4;                // Cantidad de rayos extra a cada lado
    public float rayAngleSpacing = 20f;    // Ángulo entre cada rayo de escaneo

    private Vector2 startPosition;
    private Vector2 targetPosition;
    private bool isWaiting;
    private float waitTimer;

    void Start()
    {
        startPosition = transform.position;
        SetNewDestination();
    }

    void Update()
    {
        // Activar / Desactivar con la tecla 2 (Alpha2 en el teclado alfanumérico)
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isActive = !isActive;
            Debug.Log("Wander Mode: " + (isActive ? "ON" : "OFF"));
        }

        // Si está apagado, no ejecutamos el movimiento
        if (!isActive) return;

        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                isWaiting = false;
                SetNewDestination();
            }
            return;
        }

        Vector2 directionToTarget = targetPosition - (Vector2)transform.position;
        float distance = directionToTarget.magnitude;

        // Llegamos al objetivo
        if (distance < 0.1f)
        {
            isWaiting = true;
            waitTimer = Random.Range(minWaitTime, maxWaitTime);
        }
        else
        {
            // Encontrar la mejor ruta disponible evitando obstáculos
            Vector2 moveDirection = GetBestDirection(directionToTarget.normalized);

            // Moverse
            transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime);

            // Rotar hacia la dirección de movimiento
            if (moveDirection != Vector2.zero)
            {
                float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
                Quaternion targetRot = Quaternion.Euler(0, 0, angle + angleOffset);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            }
            else
            {
                // Si no hay ruta (atrapado), buscar un nuevo destino
                isWaiting = true;
                waitTimer = 0.5f;
            }
        }
    }

    void SetNewDestination()
    {
        Vector2 randomOffset = Random.insideUnitCircle * wanderRadius;
        targetPosition = startPosition + randomOffset;
    }

    // Calcula la mejor ruta local lanzando rayos (Raycasts)
    Vector2 GetBestDirection(Vector2 targetDir)
    {
        // 1. Revisar si el camino directo está libre
        if (!Physics2D.Raycast(transform.position, targetDir, detectionDistance, obstacleLayer))
        {
            Debug.DrawRay(transform.position, targetDir * detectionDistance, Color.green);
            return targetDir;
        }

        Debug.DrawRay(transform.position, targetDir * detectionDistance, Color.red);

        // 2. Si hay obstáculo, buscar la mejor alternativa abriendo el ángulo hacia los lados
        float currentAngle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;

        for (int i = 1; i <= numRays; i++)
        {
            float offset = rayAngleSpacing * i;

            // Escanear hacia la izquierda de la dirección principal
            Vector2 dirLeft = Quaternion.Euler(0, 0, currentAngle + offset) * Vector2.right;
            if (!Physics2D.Raycast(transform.position, dirLeft, detectionDistance, obstacleLayer))
            {
                Debug.DrawRay(transform.position, dirLeft * detectionDistance, Color.yellow);
                return dirLeft;
            }
            Debug.DrawRay(transform.position, dirLeft * detectionDistance, Color.red);

            // Escanear hacia la derecha de la dirección principal
            Vector2 dirRight = Quaternion.Euler(0, 0, currentAngle - offset) * Vector2.right;
            if (!Physics2D.Raycast(transform.position, dirRight, detectionDistance, obstacleLayer))
            {
                Debug.DrawRay(transform.position, dirRight * detectionDistance, Color.yellow);
                return dirRight;
            }
            Debug.DrawRay(transform.position, dirRight * detectionDistance, Color.red);
        }

        // Si todos los caminos están bloqueados, nos detenemos temporalmente
        return Vector2.zero;
    }
}