using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Draggable))]
[RequireComponent(typeof(DragDropDetector))]
public class DraggableObj : MonoBehaviour
{
    protected Draggable draggable;
    protected DragDropDetector slotDetector;

    protected ObjSlot currentSlot;
    protected ObjSlot detectedSlot;
    protected ObjSlot previousSlot;

    public ObjSlot CurrentSlot => currentSlot;

    protected virtual void OnEnable()
    {
        if (draggable == null)
        {
            draggable = GetComponent<Draggable>();
        }

        if (slotDetector == null)
        {
            slotDetector = GetComponent<DragDropDetector>();
        }
        slotDetector.TargetComponentType = typeof(ObjSlot);

        draggable.OnDragStart.AddListener(OnDragStart);
        draggable.OnDragEnd.AddListener(OnDragEnd);
    }

    protected virtual void OnDisable()
    {
        draggable.OnDragStart.RemoveListener(OnDragStart);
        draggable.OnDragEnd.RemoveListener(OnDragEnd);
    }

    protected virtual void Update()
    {
        if (draggable.IsDragging)
        {
            // 处理slot的状态变化
            if (slotDetector.TargetComponent != null)
            {
                var slot = slotDetector.TargetComponent as ObjSlot;
                if (slot != null && slot.CanAdd(this) && detectedSlot != slot)
                {
                    slot.OnObjEnter(this);
                    if (detectedSlot != null)
                    {
                        detectedSlot.OnObjExit(this);
                    }
                }
                detectedSlot = slot;
            }
            else
            {
                if (detectedSlot != null)
                {
                    detectedSlot.OnObjExit(this);
                    detectedSlot = null;
                }
            }
        }
    }

    public virtual void SetSlot(ObjSlot slot)
    {
        print("SetSlot: " + slot.name);
        currentSlot = slot;
        currentSlot.TryAddObj(this);
        currentSlot.OnObjExit(this);

        transform.rotation = slot.transform.rotation;
        transform.position = slot.transform.position;
    }

    protected virtual void OnDragStart()
    {
        print("OnDragStart: " + currentSlot.name);
        detectedSlot = currentSlot;
        previousSlot = currentSlot;
        if (currentSlot != null)
        {
            currentSlot.TryRemoveObj(this);
            currentSlot = null;
        }

        transform.rotation = Quaternion.identity;
    }

    protected virtual void OnDragEnd()
    {
        print("OnDragEnd: " + name);
        if (detectedSlot != null)
        {
            detectedSlot.OnObjExit(this);
        }

        var slot = slotDetector.TargetComponent as ObjSlot;
        if (slot == null || !slot.CanAdd(this))
        {
            SetSlot(previousSlot);
        }
        else
        {
            SetSlot(slot);
        }
    }
}
