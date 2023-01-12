using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform pointDamage;
    [SerializeField] private float movementSpeed;
    [SerializeField] private int damage;

    [Header("AUDIO")]
    [SerializeField] private AudioSource walkSound;

    private bool isAttack;
    private GameObject target;
    private PlayerDamageable playerDamageable;
    private Rigidbody2D rigidbody;
    private Animator anim;

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        playerDamageable = target.GetComponent<PlayerDamageable>();
    }

    private void Update()
    {
        if (target != null)
        {
            CheckDistanceToTarget();

            RotateToTarget();
        }
    }

    private void RotateToTarget()
    {
        Vector2 direction = target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.2f);
    }

    private void CheckDistanceToTarget()
    {
        float distanceToTarget = Vector2.Distance(transform.position, target.transform.position);

        TryMove(distanceToTarget);

        if (!isAttack && distanceToTarget <= 1.5)
        {
            isAttack = true;
            anim.SetTrigger("Attack");
        }
    }

    private void TryMove(float distance)
    {
        if (!isAttack && distance > 1.5)
        {
            anim.SetBool("isWalking", true);
            Vector2 direction = target.transform.position - transform.position;
            rigidbody.velocity = direction.normalized * movementSpeed;
        }
        else
            anim.SetBool("isWalking", false);
    }


    // Animation events

    public void Damage()
    {
        float distanceToTarget = Vector2.Distance(pointDamage.position, target.transform.position);

        if (distanceToTarget <= 1)
            playerDamageable.ApplyDamage(damage);
    }

    public void EndAttack() => isAttack = false;

    public void PlayWalkSound()
    {
        walkSound.PlayOneShot(walkSound.clip);
    }
}
