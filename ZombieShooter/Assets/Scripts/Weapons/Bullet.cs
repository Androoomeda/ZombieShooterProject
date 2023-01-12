using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private int damage;

    private Rigidbody2D rigidbody;

    private void OnEnable()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (lifeTime > 0)
            lifeTime -= Time.deltaTime;
        else
            Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable;

        if (collision.CompareTag("Bullet") || collision.isTrigger)
            return;

        if (collision.gameObject.TryGetComponent<IDamageable>(out damageable))
            damageable.ApplyDamage(damage);

        Destroy(this.gameObject);
    }
}
