using UnityEngine;
using UnityEngine.Events;

public class WinSystem : MonoBehaviour
{
    public UnityEvent OnWin;

    private void LateUpdate()
    {
        if (transform.childCount <= 0)
            OnWin.Invoke();
    }
}
