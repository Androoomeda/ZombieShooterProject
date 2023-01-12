using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [SerializeField] private Animator screenFader;

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadNextLevel()
    {
        StartCoroutine(StartLoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(StartLoadScene(sceneIndex));
    }

    public void Restart()
    {
        StartCoroutine(StartLoadScene(SceneManager.GetActiveScene().buildIndex));
    }

    IEnumerator StartLoadScene(int sceneIndex)
    {
        screenFader.SetTrigger("Fade");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneIndex);
    }
}
