using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class MedicineKit : MonoBehaviour
{
    [SerializeField] private int healCount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerDamageable player;

        if (collision.gameObject.TryGetComponent<PlayerDamageable>(out player))
        {
            if(player.TryHeal(healCount))
                Destroy(this.gameObject);
        }
    }
}
