using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class LaunchingProgress : MonoBehaviour
{
    public Slider slider;
    public float speed = 25f;
    public float percent = 0.5f;
    public TextMeshProUGUI text;
    public MultiLanguageText MLT;

    float targetValue = 0f;
    bool once = true;

    public UnityEvent midEvent;
    public UnityEvent endEvent;

    // Update is called once per frame
    void Update()
    {
        slider.value += targetValue + Time.deltaTime * speed;
        if (slider.value > slider.maxValue * percent && once)
        {
            midEvent.Invoke();
        }

        if (slider.value == slider.maxValue && once)
        {
            //text.text = "³É¹¦Æð·É!";
            MLT.textID = "ui57";
            once = false;
            endEvent.Invoke();
        }
    }
}
