using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FixingProgress : MonoBehaviour
{
    public Slider slider;
    public float speed=50f;

    public bool isMax = true; 
    public float percent = 0.6f;

    float targetValue = 0f;
    bool once = true;

    public UnityEvent unityEvent;

    void Update()
    {
        slider.value += targetValue+Time.deltaTime*speed;
        if (!isMax) {
            if (slider.value > slider.maxValue*percent && once)
            {
                once = false;
                unityEvent.Invoke();
            }
        }
        else
        {
            if (slider.value == slider.maxValue && once)
            {
                once = false;
                unityEvent.Invoke();
            }
        }

    }

    public void ResetBar()
    { 
        once=true;
    }

    
}
