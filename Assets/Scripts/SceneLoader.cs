using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject quitButton;

    void Start()
    {
        #if UNITY_WEBGL
        if (quitButton != null)
        {
            quitButton.SetActive(false);
        }
        #endif
    }

    public void LoadGameScene(string sceneName)
    {
        StartCoroutine(LoadSceneAfterDelay(sceneName, 0.5f));
    }

    System.Collections.IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
