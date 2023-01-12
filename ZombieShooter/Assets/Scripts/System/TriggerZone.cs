using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour
{
    [SerializeField] private Enemy[] enemies;

    public UnityEvent OnTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnTrigger.Invoke();

            foreach (var enemy in enemies)
                if(enemy != null)
                    enemy.enabled = true;
        }
    }
}
