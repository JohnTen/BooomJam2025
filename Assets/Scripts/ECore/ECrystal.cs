using System.Collections;
using System.Collections.Generic;
using JTUtility;
using UnityEngine;

public class ECrystal : DraggableObj
{
    [SerializeField] private float effectiveTime = 5f;

    protected override void OnDragStart()
    {
        base.OnDragStart();
    }

    protected override void OnDragEnd()
    {
        base.OnDragEnd();
    }

    public override void SetSlot(ObjSlot slot, bool force = false)
    {
        if(slot.IsNotNull())
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
