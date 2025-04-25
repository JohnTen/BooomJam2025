using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : ObjSlot
{
    public override void AddObj(Component obj)
    {
        base.AddObj(obj);
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;
    }

    public override bool TryAddObj(Component obj)
    {
        if (!CanAdd(obj))
        {
            return false;
        }

        if (obj is ResourceObj resourceObj)
        {
            if (ObjInSlot != null)
            {
                var resourceInSlot = ObjInSlot as ResourceObj;
                resourceInSlot.Stack += resourceObj.Stack;
                Destroy(obj.gameObject);
            }
            else
            {
                AddObj(obj);
            }
            return true;
        }

        if (obj is ECore)
        {
            AddObj(obj);
            return true;
        }

        return false;
    }

    public override bool TryRemoveObj(Component obj)
    {
        if (!CanRemove(obj))
        {
            return false;
        }

        ClearObj();
        return true;
    }

    public override bool CanAdd(Component component)
    {
        if (ObjInSlot != null)
        {
            var resourceInSlot = ObjInSlot as ResourceObj;
            var resourceToAdd = component as ResourceObj;
            if (resourceInSlot == null || resourceToAdd == null)
            {
                return false;
            }

            if (resourceInSlot.Template.uid != resourceToAdd.Template.uid)
            {
                return false;
            }

            return true;
        }

        if (component is ResourceObj || component is ECore)
        {
            return true;
        }

        return false;
    }

    public override bool CanRemove(Component obj)
    {
        return obj == ObjInSlot;
    }

    public override void OnObjEnter(Component obj)
    {

    }

    public override void OnObjExit(Component obj)
    {

    }
}
