using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using JTUtility;
using UnityEngine.Events;
using Unity.VisualScripting;

public class ECoreSlot : ObjSlot
{
    [SerializeField] UnityEvent<bool> OnCoreChanges;
    [SerializeField] private bool noWarmUp = false;
    [SerializeField] private bool ecoreOnly = false;

    public ECrystal ECoreInSlot => ObjInSlot as ECrystal;

    public override bool HasActiveObj
    {
        get
        {
            if (!HasObj)
            {
                return false;
            }

            if (ECoreInSlot is ECore eCore)
            {
                return !eCore.IsWarmUp && !eCore.IsBreakdown;
            }

            return true;
        }   
    }

    public bool NoWarmUp => noWarmUp;

    public override bool TryAddObj(Component obj)
    {
        if (!CanAdd(obj))
        {
            return false;
        }

        AddObj(obj);
        return true;
    }

    public override void AddObj(Component obj)
    {
        base.AddObj(obj);
        if (HasObj)
        {
            OnCoreChanges.Invoke(true);
        }
    }

    public override void ClearObj()
    {
        if (HasObj)
        {
            OnCoreChanges.Invoke(false);
        }
        base.ClearObj();
    }

    public override bool CanAdd(Component obj)
    {
        if (ecoreOnly)
        {
            return obj is ECore && !HasObj;
        }
        else
        {
            return obj is ECrystal && !HasObj;
        }
    }

    public override bool CanRemove(Component obj)
    {
        return obj is ECrystal && obj == ObjInSlot;
    }
}
