using System.Collections;
using System.Collections.Generic;
using JTUtility;
using UnityEngine;
using UnityEngine.UI;

public class ECrystal : DraggableObj
{
    [SerializeField] protected Image visualImage;
    [SerializeField] private float effectiveTime = 5f;

    protected Material material;

    protected virtual void Start()
    {
        material = new Material(visualImage.material);
        visualImage.material = material;
    }

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
        float time = effectiveTime;
        while (time > 0)
        {
            time -= Time.deltaTime;
            material.SetFloat("_energy", time / effectiveTime);
            yield return null;
        }
        this.CurrentSlot.TryRemoveObj(this);
        Destroy(gameObject);
    }
}
