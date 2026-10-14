using UnityEngine;

// Clase abstracta que sirve como plantilla para crear diferentes tipos de agentes.
// Al ser abstracta, no se puede asignar directamente a un GameObject; necesita un script que la herede.
public abstract class AgenteBase : MonoBehaviour
{
    [Header("Configuración Principal")]
    public KeyCode teclaActivacion;
    // Estado actual del agente
    public bool activo = false;
    // Límite de rapidez a la que puede moverse el agente.
    public float velocidadMaxima = 5f;
    // Límite de la fuerza de empuje aplicada para cambiar de dirección
    public float fuerzaMaxima = 5f;

    [Header("Obstacle Avoidance")]
    // Define qué objetos son considerados obstáculos
    public LayerMask capaObstaculos;
    // Longitud de los rayos para detectar obstáculos
    public float distanciaEvasion = 2f;
    // Multiplicador que define qué tanta prioridad tiene esquivar frente a ir al objetivo.
    public float pesoEvasion = 2f;

    // Velocidad actual de desplazamiento (protegida para que las clases hijas puedan leerla/usarla).
    protected Vector2 velocidad;

    void Update()
    {
        // Detecta si se presiona la tecla asignada para alternar el estado activo/inactivo.
        if (Input.GetKeyDown(teclaActivacion))
        {
            activo = !activo;
            // Si se desactiva, detiene el movimiento inmediatamente.
            if (!activo) velocidad = Vector2.zero;
        }

        // Si el agente está activo, calcula las fuerzas y actualiza su posición.
        if (activo)
        {
            // 1. Obtiene la fuerza principal de movimiento (definida en las clases hijas: Seek, Flee, etc.).
            Vector2 fuerzaSteering = CalcularComportamiento();
            // 2. Obtiene la fuerza de repulsión si hay un obstáculo cerca.
            Vector2 fuerzaEvasion = CalcularObstacleAvoidance();

            // 3. Mezcla ambas fuerzas. La evasión se multiplica por su peso para darle más o menos prioridad.
            Vector2 fuerzaTotal = fuerzaSteering + (fuerzaEvasion * pesoEvasion);

            // 4. Limita la fuerza total para que el agente no acelere de forma infinita o irreal.
            fuerzaTotal = Vector2.ClampMagnitude(fuerzaTotal, fuerzaMaxima);

            // --- Aplicar físicas (Integración de Euler) ---
            // Acelera la velocidad actual basándose en la fuerza y el tiempo transcurrido.
            velocidad += fuerzaTotal * Time.deltaTime;
            // Asegura que la velocidad resultante no supere el límite máximo permitido.
            velocidad = Vector2.ClampMagnitude(velocidad, velocidadMaxima);
            // Mueve el objeto en la escena.
            transform.position += (Vector3)velocidad * Time.deltaTime;

            // --- Rotar el sprite ---
            // Si el agente se está moviendo, lo rota para que mire hacia la dirección de su velocidad.
            if (velocidad != Vector2.zero)
            {
                // Calcula el ángulo en grados a partir del vector de velocidad.
                float angulo = Mathf.Atan2(velocidad.y, velocidad.x) * Mathf.Rad2Deg;
                // Aplica la rotación en el eje Z (ideal para 2D).
                transform.rotation = Quaternion.Euler(0, 0, angulo);
            }
        }
    }


    protected abstract Vector2 CalcularComportamiento();

    // Lógica para detectar y esquivar obstáculos.
    Vector2 CalcularObstacleAvoidance()
    {
        Vector2 fuerzaEvasion = Vector2.zero;
        // Determina hacia dónde está "mirando" el agente. Si está quieto, usa su derecha local.
        Vector2 direccionActual = velocidad == Vector2.zero ? (Vector2)transform.right : velocidad.normalized;

        // Crea 3 rayos direccionales uno al frente, uno a +45 grados y uno a -45 grados.
        Vector2[] direcciones = {
            direccionActual,
            Quaternion.Euler(0, 0, 45) * direccionActual,
            Quaternion.Euler(0, 0, -45) * direccionActual
        };

        // Revisa cada una de las 3 direcciones.
        foreach (Vector2 dir in direcciones)
        {
            Debug.DrawRay(transform.position, dir * distanciaEvasion, Color.red);
            // Lanza el rayo para detectar colisiones en la capa de obstáculos.
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, distanciaEvasion, capaObstaculos);

            // Si el rayo choca con un obstáculo
            if (hit.collider != null)
            {
                // Toma la normal de la superficie impactada 
                Vector2 normal = hit.normal;

                // Si el agente va directamente de frente hacia la pared 
                if (Vector2.Dot(direccionActual, normal) < -0.8f)
                {
                    // Tuerce la normal un poco para forzar al agente a elegir un lado y evitar que se quede atascado.
                    normal = Quaternion.Euler(0, 0, 45) * normal;
                }

                // Calcula qué tan cerca está el obstáculo 
                float cercania = 1f - (hit.distance / distanciaEvasion);
                // Aplica una fuerza de evasión que es mayor cuanto más cerca esté del obstáculo.
                fuerzaEvasion += normal * fuerzaMaxima * cercania;
            }
        }

        // Retorna la fuerza de evasión combinada de los 3 rayos.
        return fuerzaEvasion;
    }

    // Método público para que otros scripts puedan consultar a qué velocidad va este agente.
    public Vector2 ObtenerVelocidad()
    {
        return velocidad;
    }
}