using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualCursorVisual : MonoBehaviour
{
    [SerializeField] private Image outerRing;
    [SerializeField] private Image innerCore;
    
    [Header("Moving")]
    [SerializeField] private float movingShrinkSpeed = 1f;
    [SerializeField] private float movingMaxShrink = 0.8f;

    [Header("Selected")]
    [SerializeField] private float selectedExpandSpeed = 1f;
    [SerializeField] private float selectedMaxExpand = 1.2f;
    [SerializeField] private Color selectedColor;

    Vector2 lastPosition;
    Draggable draggable;

    Vector2 coreSizeDelta;
    Vector2 ringSizeDelta;
    Color coreColor;

    void Start()
    {
        lastPosition = VirtualCursor.ScreenPosition;
        coreSizeDelta = innerCore.rectTransform.sizeDelta;
        ringSizeDelta = outerRing.rectTransform.sizeDelta;
        coreColor = innerCore.color;
    }

    void Update()
    {
        draggable = VirtualCursor.Instance.FirstResult.gameObject?.GetComponent<Draggable>();

        var targetCoreSizeDelta = coreSizeDelta;
        var targetRingSizeDelta = ringSizeDelta;
        var targetColor = coreColor;
        var coreShrinkFactor = Time.unscaledDeltaTime * movingShrinkSpeed;
        var ringShrinkFactor = Time.unscaledDeltaTime * selectedExpandSpeed;
        
        if (draggable != null && draggable.IsDragging)
        {
            targetRingSizeDelta = ringSizeDelta * selectedMaxExpand;
            targetColor = selectedColor;
        }
        else if (lastPosition != VirtualCursor.ScreenPosition)
        {
            targetCoreSizeDelta = coreSizeDelta * movingMaxShrink;
        }

        innerCore.rectTransform.sizeDelta = Vector2.Lerp(innerCore.rectTransform.sizeDelta, targetCoreSizeDelta, coreShrinkFactor);
        outerRing.rectTransform.sizeDelta = Vector2.Lerp(outerRing.rectTransform.sizeDelta, targetRingSizeDelta, ringShrinkFactor);
        innerCore.color = Color.Lerp(innerCore.color, targetColor, ringShrinkFactor);

        lastPosition = VirtualCursor.ScreenPosition;
    }


}
