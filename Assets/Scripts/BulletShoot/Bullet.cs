using UnityEngine;

public class Bullet : MonoBehaviour
{
    private const float MAX_LIFE_TIME = 3f;
    private float _lifeTime = 0f;
    public float speed = 15f;
    public Vector2 Velocity;

    private void Update()
    {
        transform.position += (Vector3)Velocity * Time.deltaTime;

        _lifeTime += Time.deltaTime;

        if (_lifeTime > MAX_LIFE_TIME)
        {
            Disable();
        }
    }

    private void OnEnable()
    {
        _lifeTime = 0f;

        if (GetComponent<Rigidbody2D>() != null)
        {
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}