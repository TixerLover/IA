using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum EnemyState
    {
        Chase,
        Die
    }

    [Header("Stats del Enemigo")]
    public EnemyState currentState = EnemyState.Chase;
    public float speed = 3f;
    public int maxHearts = 3;

    private int currentHearts;

    // Cacheamos el jugador para no buscarlo constantemente
    private Transform player;

    void OnEnable()
    {
        ResetEnemy();
        FindPlayer();
    }

    void Update()
    {
        // En un Bullet Heaven, el Switch sigue siendo útil si luego añades un estado de "Knockback"
        switch (currentState)
        {
            case EnemyState.Chase:
                UpdateChaseState();
                break;
            case EnemyState.Die:
                // No hacemos nada, el enemigo se está desactivando
                break;
        }
    }

    private void UpdateChaseState()
    {
        if (player == null) return;

        // Movimiento en enjambre directo hacia el jugador
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        // Voltear sprite (muy ligero en rendimiento)
        if (direction.x > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (direction.x < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si ya está muerto (ej. frame de animación de muerte), ignorar nuevas balas
        if (currentState == EnemyState.Die) return;

        if (other.CompareTag("Bullet"))
        {
            // En un Bullet Heaven, podrías querer leer el daño de la bala en lugar de quemar "1"
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHearts -= damage;

        if (currentHearts <= 0)
        {
            ChangeState(EnemyState.Die);
            DieRoutine();
        }
    }

    private void ChangeState(EnemyState newState)
    {
        currentState = newState;
    }

    private void DieRoutine()
    {
        // TODO: Instanciar gema de experiencia (XP) aquí

        // Apagamos el objeto para reciclarlo en el Object Pool
        gameObject.SetActive(false);
    }

    private void ResetEnemy()
    {
        currentHearts = maxHearts;
        currentState = EnemyState.Chase; // Arranca persiguiendo de inmediato
    }

    private void FindPlayer()
    {
        // Llama directamente a tu clase Movement
        if (Movement.Instance != null)
        {
            player = Movement.Instance.transform;
        }
    }
}



