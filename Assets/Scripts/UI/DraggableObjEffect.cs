using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using JTUtility;

[RequireComponent(typeof(Draggable))]
public class DraggableObjEffect : MonoBehaviour
{
    [SerializeField] GameObject outline;

    private Draggable draggable;

    private void Awake()
    {
        draggable = GetComponent<Draggable>();
    }

    private void Update()
    {
        if (VirtualCursor.RaycastResults.Count > 0)
        {
            if (VirtualCursor.RaycastResults[0].gameObject == gameObject &&
                draggable.CanDrag &&
                !draggable.IsDragging)
            {
                EnableEffect();
            }
            else
            {
                DisableEffect();
            }
        }
        else
        {
            DisableEffect();
        }
    }

    private void EnableEffect()
    {
        transform.DOScale(Vector3.one * 1.05f, 0.2f).SetEase(Ease.InOutSine).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        if (outline.IsNotNull())
        {
            outline.SetActive(true);
        }
    }

    private void DisableEffect()
    {
        transform.DORewind();
        if (outline.IsNotNull())
        {
            outline.SetActive(false);
        }
    }
}
