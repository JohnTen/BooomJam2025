using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchListener : MonoBehaviour
{
    public bool engineReady = false;    
    public bool coplitReady = false;
    public GameObject launch;
    bool once = true;

    // Update is called once per frame
    void Update()
    {
        if(engineReady&&coplitReady&&once)
        {
            launch.SetActive(true);
            once = false;
            this.enabled = false;
        }
    }

    public void SetEngineReady()
    {
        engineReady = true;
    }
    public void SetCoplitReady()
    {
        coplitReady = true;
    }
}
