
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class CabinActiveFaliure : MonoBehaviour
{
    RectTransform RT;

    public float duration = 0.3f;
    public float strength = 10f;
    public float vibrato = 20f;
    public float delayTime = 1f;

    public UnityEvent unityEvent;

    public UnityEvent delayEvent;


    private void Start()
    {
        RT = GetComponent<RectTransform>();
    }
    public void ActiveFaliure()
    {
        RT.DOShakeAnchorPos(duration, 10, 20, 90, true, true);
        unityEvent.Invoke();

        DOVirtual.DelayedCall(delayTime, () =>
        {
            delayEvent.Invoke();
        });
    }

}
