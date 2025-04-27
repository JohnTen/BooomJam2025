using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ECrystal : DraggableObj
{
    protected override void OnDragStart()
    {
        base.OnDragStart();
        VirtualCursor.Instance.CursorSpeedMultiplier = GameManager.Instance.GameProperty.CoreDragSpeed;
    }

    protected override void OnDragEnd()
    {
        base.OnDragEnd();
        VirtualCursor.Instance.CursorSpeedMultiplier = 1f;
    }

    public override void SetSlot(ObjSlot slot, bool force = false)
    {
        transform.SetParent(slot.transform);
        base.SetSlot(slot, force);
    }
}
