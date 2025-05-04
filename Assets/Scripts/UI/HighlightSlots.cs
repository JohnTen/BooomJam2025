using System.Collections;
using System.Collections.Generic;
using JTUtility;
using JTUtility.Event;
using UnityEngine;

public class HighlightSlots : MonoBehaviour
{
    void OnEnable()
    {
        EventRegister<DraggableObj>.Register(EventConstant.OnDragStart, OnDragStart);
        EventRegister<DraggableObj>.Register(EventConstant.OnDragEnd, OnDragEnd);
    }

    void OnDisable()
    {
        EventRegister<DraggableObj>.UnRegister(EventConstant.OnDragStart, OnDragStart);
        EventRegister<DraggableObj>.UnRegister(EventConstant.OnDragEnd, OnDragEnd);
    }

    void OnDragStart(DraggableObj draggableObj)
    {
        var slots = FindObjectsByType<ObjSlot>(FindObjectsSortMode.None);
        foreach (var slot in slots)
        {
            if (slot.IsNull() || slot.Indicator.IsNull() || !slot.isActiveAndEnabled)
            {
                continue;
            }

            print("Slot: " + slot.transform.parent.name + "\\" + slot.name + " CanAdd: " + slot.CanAdd(draggableObj));
            if (slot.CanAdd(draggableObj))
            {
                slot.Indicator.SetActive(true);
            }
        }
    }

    void OnDragEnd(DraggableObj draggableObj)
    {
        var slots = FindObjectsByType<ObjSlot>(FindObjectsSortMode.None);
        foreach (var slot in slots)
        {
            slot.Indicator?.SetActive(false);
        }
    }
}
