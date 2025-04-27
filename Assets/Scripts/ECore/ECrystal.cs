using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ECrystal : DraggableObj
{
    [SerializeField] private float effectiveTime = 5f;

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

        if (slot is ECoreSlot && effectiveTime > 0)
        {
            StartCoroutine(Effective());
        }
    }

    private IEnumerator Effective()
    {
        draggable.CanDrag = false;
        yield return new WaitForSeconds(effectiveTime);
        this.CurrentSlot.TryRemoveObj(this);
        Destroy(gameObject);
    }
}
