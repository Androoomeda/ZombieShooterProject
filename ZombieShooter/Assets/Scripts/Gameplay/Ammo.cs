using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Ammo : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player;

        if (collision.gameObject.TryGetComponent<PlayerController>(out player))
        {
            player.TakeAmmo(Random.Range(10, 20));
            Destroy(this.gameObject);
        }
    }
}
