using UnityEngine;

public abstract class AgenteBase : MonoBehaviour
{
    [Header("Configuración Principal")]
    public KeyCode teclaActivacion; // Asignar del 1 al 5 según el agente
    public bool activo = false;
    public float velocidadMaxima = 5f;
    public float fuerzaMaxima = 5f;

    [Header("Obstacle Avoidance")]
    public LayerMask capaObstaculos;
    public float distanciaEvasion = 2f;
    public float pesoEvasion = 2f;

    protected Vector2 velocidad;

    void Update()
    {
        if (Input.GetKeyDown(teclaActivacion))
        {
            activo = !activo;
            if (!activo) velocidad = Vector2.zero;
        }

        if (activo)
        {
            Vector2 fuerzaSteering = CalcularComportamiento();
            Vector2 fuerzaEvasion = CalcularObstacleAvoidance();

            // Volvemos a SUMAR las fuerzas. 
            // Esto permite que el agente intente ir a su objetivo MIENTRAS esquiva.
            Vector2 fuerzaTotal = fuerzaSteering + (fuerzaEvasion * pesoEvasion);

            fuerzaTotal = Vector2.ClampMagnitude(fuerzaTotal, fuerzaMaxima);

            // Aplicar físicas
            velocidad += fuerzaTotal * Time.deltaTime;
            velocidad = Vector2.ClampMagnitude(velocidad, velocidadMaxima);
            transform.position += (Vector3)velocidad * Time.deltaTime;

            // Rotar el sprite
            if (velocidad != Vector2.zero)
            {
                float angulo = Mathf.Atan2(velocidad.y, velocidad.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angulo);
            }
        }
    }

    // Cada agente definirá su propio comportamiento aquí
    protected abstract Vector2 CalcularComportamiento();

    Vector2 CalcularObstacleAvoidance()
    {
        Vector2 fuerzaEvasion = Vector2.zero;
        Vector2 direccionActual = velocidad == Vector2.zero ? (Vector2)transform.right : velocidad.normalized;

        // Abrimos un poco más los rayos laterales a 45 grados para mejor visión periférica
        Vector2[] direcciones = {
            direccionActual,
            Quaternion.Euler(0, 0, 45) * direccionActual,
            Quaternion.Euler(0, 0, -45) * direccionActual
        };

        foreach (Vector2 dir in direcciones)
        {
            Debug.DrawRay(transform.position, dir * distanciaEvasion, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, distanciaEvasion, capaObstaculos);

            if (hit.collider != null)
            {
                Vector2 normal = hit.normal;

                // TRUCO ANTI-ATASCO: 
                // Vector2.Dot calcula si están chocando de frente. 
                // Si la normal está casi opuesta a nuestra dirección, forzamos un desvío rotando la normal.
                if (Vector2.Dot(direccionActual, normal) < -0.8f)
                {
                    normal = Quaternion.Euler(0, 0, 45) * normal;
                }

                float cercania = 1f - (hit.distance / distanciaEvasion);
                fuerzaEvasion += normal * fuerzaMaxima * cercania;
            }
        }
        return fuerzaEvasion;
    }

    public Vector2 ObtenerVelocidad()
    {
        return velocidad;
    }
}