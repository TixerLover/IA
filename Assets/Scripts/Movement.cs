using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    // 1. AQUÍ ESTÁ EL SINGLETON: Permite que los enemigos te encuentren sin causar lag.
    public static Movement Instance { get; private set; }

    PlayerControls controls;
    Vector2 move;
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Transform cameraTransform;
    private bool isFacingRight = true;
    private Animator anim;
    public Transform firePoint;
    private RadialShotWeapon radialWeapon;

    [Header("HP")]
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    [Header("Cooldowns")]
    public float radialCooldown = 5f;
    private float nextRadialTime = 0f;
    public Image cooldownWheel;

    private void Awake()
    {
        // 2. CONFIGURACIÓN DEL SINGLETON
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        controls = new PlayerControls();
        anim = GetComponent<Animator>();
        radialWeapon = GetComponent<RadialShotWeapon>();
        rb = GetComponent<Rigidbody2D>();

        if (cooldownWheel != null) cooldownWheel.fillAmount = 0;
        if (rb == null)
        {
            Debug.LogError("The Rigidbody component is required for physics movement.");
        }

        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
        else
        {
            Debug.LogError("No object tagged 'MainCamera' was found!");
        }

        controls.Player.move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Player.move.canceled += ctx => move = Vector2.zero;
        controls.Player.Attack.performed += ctx => Shoot();
        controls.Player.Attack1.performed += ctx => Fire();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        if (anim != null)
        {
            anim.SetBool("isMoving", move.sqrMagnitude > 0.01f);
        }

        Vector2 desiredDirection = (cameraTransform.up * move.y) + (cameraTransform.right * move.x);
        desiredDirection.Normalize();
        rb.linearVelocity = desiredDirection * moveSpeed;

        Flip();

        // 3. LIMPIEZA: Tenías la lógica de actualizar la rueda repetida dos veces en el Update.
        // Dejé solo la llamada al método que ya tenías hecho abajo.
        HandleCooldownUI();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        // 4. MEJORA: Cambiado "==" por "<=" para evitar bugs si recibes más de 1 de daño de golpe.
        if (currentHealth <= 0)
        {
            GameOver();
            if (anim != null)
            {
                anim.SetTrigger("die");
            }
        }
    }

    private void GameOver()
    {
        Invoke("LoadDefeatScene", 2f);
    }

    private void LoadDefeatScene()
    {
        SceneManager.LoadScene("02_Defeat");
    }

    private void Flip()
    {
        if ((isFacingRight && move.x < 0f) || (!isFacingRight && move.x > 0f))
        {
            isFacingRight = !isFacingRight;

            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void Shoot()
    {
        Bullet bullet = BulletPool.Instance.RequestBullet();
        if (bullet != null)
        {
            bullet.transform.position = firePoint.position;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            Vector2 lookDirection = (mousePos - firePoint.position).normalized;
            bullet.Velocity = lookDirection * bullet.speed;
        }
    }

    private void Fire()
    {
        if (Time.time >= nextRadialTime)
        {
            if (radialWeapon != null)
            {
                radialWeapon.TryPerformRadialShot();
                nextRadialTime = Time.time + radialCooldown;
            }
        }
    }

    private void HandleCooldownUI()
    {
        if (cooldownWheel == null) return;

        if (Time.time < nextRadialTime)
        {
            float remaining = (nextRadialTime - Time.time) / radialCooldown;
            cooldownWheel.fillAmount = remaining;
        }
        else
        {
            cooldownWheel.fillAmount = 0;
        }
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }
}