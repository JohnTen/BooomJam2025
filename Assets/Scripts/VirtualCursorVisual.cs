using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualCursorVisual : MonoBehaviour
{
    [SerializeField] private Image cursor;

    [SerializeField] private Sprite normalCursor;
    [SerializeField] private Sprite selectedCursor;
    [SerializeField] private Sprite movingCursor;

    Vector2 lastPosition;
    Draggable draggable;

    void Start()
    {
        lastPosition = VirtualCursor.ScreenPosition;
    }

    void Update()
    {
        var results = new List<RaycastResult>();
        var pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = VirtualCursor.ScreenPosition;
        EventSystem.current.RaycastAll(pointerEventData, results);
        foreach (var result in results)
        {
            if (result.gameObject.TryGetComponent(out draggable))
            {
                break;
            }
        }
        
        if (draggable != null && draggable.IsDragging)
        {
            cursor.sprite = selectedCursor;
        }
        else if (lastPosition != VirtualCursor.ScreenPosition)
        {
            cursor.sprite = movingCursor;
        }
        else
        {
            cursor.sprite = normalCursor;
        }

        lastPosition = VirtualCursor.ScreenPosition;
    }


}
