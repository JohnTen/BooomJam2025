using System.Collections;
using System.Collections.Generic;
using JTiming;
using JTUtility.Event;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncLoadScene : MonoBehaviour
{
    public static AsyncLoadScene Instance;

    private AsyncOperation asyncOperation;

    public string sceneName;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadSceneAsync(sceneName);
    }

    public void LoadSceneAsync(string sceneName)
    {
        this.sceneName = sceneName;
        asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = false;
    }

    public void ActiveScene()
    {
        if (asyncOperation == null)
        {
            Debug.LogError("AsyncOperation is null");
            return;
        }

        EventDispatcher<string>.Dispatch(EventConstant.AsyncSceneActivating, sceneName);
        asyncOperation.allowSceneActivation = true;

        Timing.CallDelayed(0.0001f, () =>
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name).allowSceneActivation = true;
            EventDispatcher<string>.Dispatch(EventConstant.AsyncSceneActivated, sceneName);
        });
    }
}
