using System.Collections;
using UnityEngine;

public class ShotgunShot : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(CheckChildCount());
    }

    IEnumerator CheckChildCount()
    {
        if (transform.childCount == 0)
            Destroy(gameObject);

        yield return new WaitForSeconds(1f);
        StartCoroutine(CheckChildCount());
    }
}
