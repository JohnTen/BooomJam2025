using System.Collections;
using System.Collections.Generic;
using JTUtility.Event;
using UnityEngine;

[RequireComponent(typeof(Draggable))]
[RequireComponent(typeof(DragDropDetector))]
public class DraggableObj : MonoBehaviour
{
    [SerializeField] protected bool initOnEnable = true;

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

        print("OnEnable: " + name + " initOnEnable: " + initOnEnable + " previousSlot: " + previousSlot + " currentSlot: " + currentSlot);
        if (initOnEnable && previousSlot == null && currentSlot == null)
        {
            StartCoroutine(InitializePosition());
        }
    }

    protected virtual void OnDisable()
    {
        draggable.OnDragStart.RemoveListener(OnDragStart);
        draggable.OnDragEnd.RemoveListener(OnDragEnd);
    }

    protected virtual IEnumerator InitializePosition()
    {
        yield return null;
        yield return null;
        yield return null;

        while (true)
        {
            if (draggable.IsDragging)
                yield break;
                
            print("InitializePosition: " + name + " " + slotDetector.DetectComponent());
            if (slotDetector.DetectComponent())
            {
                var slot = slotDetector.TargetComponent as ObjSlot;
                if (slot != null && slot.CanAdd(this))
                {
                    previousSlot = slot;
                    SetSlot(slot);
                    break;
                }
            }
            yield return null;
        }
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

    public virtual void SetSlot(ObjSlot slot, bool force = false)
    {
        if (slot == null)
        {
            Debug.LogError("SetSlot: " + name + " to null slot");
            return;
        }

        if (!slot.isActiveAndEnabled)
        {
            Debug.LogError("SetSlot: " + name + " to disabled slot");
            return;
        }
        
        slot.OnObjExit(this);
        if (force)
        {
            slot.AddObj(this);
        }
        else if (!slot.TryAddObj(this))
        {
            if (slot == previousSlot)
            {
                Debug.LogError("SetSlot: " + name + " to same slot and failed");
                return;
            }

            SetSlot(previousSlot, true);
            return;
        }

        currentSlot = slot;
        transform.rotation = slot.ObjParent.rotation;
        transform.position = slot.ObjParent.position;
        transform.localScale = Vector3.one;
    }

    protected virtual void OnDragStart()
    {
        detectedSlot = currentSlot;
        previousSlot = currentSlot;
        if (currentSlot != null)
        {
            currentSlot.TryRemoveObj(this);
            currentSlot = null;
        }

        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        EventDispatcher<DraggableObj>.Dispatch(EventConstant.OnDragStart, this);
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
            SetSlot(previousSlot, true);
        }
        else
        {
            SetSlot(slot);
        }
        EventDispatcher<DraggableObj>.Dispatch(EventConstant.OnDragEnd, this);
    }
}
