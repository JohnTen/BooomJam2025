using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameEffect : MonoBehaviour
{
    [SerializeField] private Image frame;
    [SerializeField] private float speed;
    [SerializeField] private Vector2 minMaxVal;
    [SerializeField] private Color color1;
    [SerializeField] private Color color2;

    void OnEnable()
    {
        StartCoroutine(BlinkCoroutine());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator BlinkCoroutine()
    {
        while (true)
        {
            frame.pixelsPerUnitMultiplier = Mathf.Lerp(minMaxVal.x, minMaxVal.y, Mathf.Abs(Mathf.Sin(Time.unscaledTime * speed)));
            frame.color = Color.Lerp(color1, color2, Mathf.Abs(Mathf.Sin(Time.unscaledTime * speed)));
            yield return null;
        }
    }
}
