using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform followTarget;

    private void FixedUpdate()
    {
        Vector3 currentCameraPos = Camera.main.transform.position;
        if(followTarget != null)
        {
            Vector3 targetPos = new Vector3(followTarget.position.x, followTarget.position.y, -10);
            Camera.main.transform.position = Vector3.Lerp(currentCameraPos, targetPos, 0.1f);
        }
    }
}
