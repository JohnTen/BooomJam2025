using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace JTUtility
{
    public class SwitchLevel : MonoBehaviour
    {
        [System.Serializable] private class AsyncOperationEvent : UnityEvent<AsyncOperation> { }

        [SerializeField] private AsyncOperationEvent onLoadingScene = new AsyncOperationEvent();

        public void LoadScene(string levelName)
        {
            SceneManager.LoadScene(levelName);
        }

        public void LoadSceneAsync(string levelName)
        {
            onLoadingScene.Invoke(SceneManager.LoadSceneAsync(levelName));
        }

        public void LoadSceneAdditive(string levelName)
        {
            SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
        }

        public void LoadSceneAsyncAdditive(string levelName)
        {
            onLoadingScene.Invoke(SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive));
        }

        public void UnloadSceneAsync(string levelName)
        {
            SceneManager.UnloadSceneAsync(levelName);
        }

        public void ReloadCurrentScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}