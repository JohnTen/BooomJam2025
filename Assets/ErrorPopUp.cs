using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class ErrorPopUp : MonoBehaviour
{
    public Vector3 originSize;
    [Header("TweenSetting")]
    public bool isStart = true;
    public float fadeTime = 0.2f;
    [Header("ShakeSetting")]
    public float shakeDuration = 0.5f;
    public float shakeStrength = 10f;
    public int shakeVibrato = 20;

    private void OnEnable()
    {
        originSize = transform.localScale;
        ShowPopUp();
    }

    public void ShowPopUp()
    {
        transform.localScale=Vector3.zero;
        gameObject.SetActive(true);
        transform.DOScale(originSize, fadeTime).SetEase(Ease.OutSine).SetUpdate(true).OnComplete(()=>
        {
            transform.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato, 90, true, true);
        });
    }
}

