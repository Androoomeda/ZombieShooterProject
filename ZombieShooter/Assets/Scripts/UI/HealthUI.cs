using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Animator[] heartsAnim;

    private int currentHearthIndex = 2;

    public void FadeHearts(int damage)
    {
        for (int i = 0; i < damage; i++)
        {
            if (currentHearthIndex < 0)
            {
                currentHearthIndex = 0;
                return;
            }

            heartsAnim[currentHearthIndex].SetTrigger("Fade");
            currentHearthIndex--;
        }
    }

    public void ShowHearts(int heartsCount)
    {
        for (int i = 0; i < heartsCount; i++)
        {
            currentHearthIndex++;

            if (currentHearthIndex > 2)
            {
                currentHearthIndex = 2;
                return;
            }

            heartsAnim[currentHearthIndex].SetTrigger("Show");
        }
    }
}
