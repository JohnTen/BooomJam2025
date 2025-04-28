using System.Collections;
using System.Collections.Generic;
using JTUtility.Event;
using UnityEngine;

public class DestroyOnSceneChange : MonoBehaviour
{
    void OnEnable()
    {
        EventRegister<string>.Register(EventConstant.AsyncSceneActivating, OnSceneActivating);
    }

    void OnDisable()
    {
        EventRegister<string>.UnRegister(EventConstant.AsyncSceneActivating, OnSceneActivating);
    }

    void OnSceneActivating(string sceneName)
    {
        Destroy(gameObject);
    }
}
