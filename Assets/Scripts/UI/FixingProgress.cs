using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FixingProgress : MonoBehaviour
{
    public Slider slider;
    public float speed=50f;
    float targetValue = 0f;
    bool once = true;

    public UnityEvent unityEvent;

    void Update()
    {
        slider.value += targetValue+Time.deltaTime*speed;
        if (slider.value == slider.maxValue&&once)
        {
            once = false;
            unityEvent.Invoke();
        }
    }

    
}
