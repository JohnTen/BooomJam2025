using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Draggable))]
public class DragDropDetector : MonoBehaviour
{
    [SerializeField] private Component targetComponent;
    public Component TargetComponent
    {
        get
        {
            return targetComponent;
        }
    }

    public Type TargetComponentType;

    private Draggable draggable;

    void OnEnable()
    {
        if (draggable == null)
        {
            draggable = GetComponent<Draggable>();
        }
    }

    void Update()
    {
        if (draggable.IsDragging)
        {
            DetectComponent();
        }
    }

    public bool DetectComponent()
    {
        var pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Camera.main.WorldToScreenPoint(transform.position);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        targetComponent = null;
        foreach (var result in results)
        {
            if (result.gameObject.TryGetComponent(TargetComponentType, out targetComponent))
            {
                break;
            }
        }

        return targetComponent != null;
    }
}
