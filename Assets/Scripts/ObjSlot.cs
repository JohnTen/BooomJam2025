using JTUtility;
using UnityEngine;

public abstract class ObjSlot : MonoBehaviour
{
    [SerializeField] protected Component obj;

    [SerializeField] protected Indicator indicator;

    public virtual bool HasObj => obj.IsNotNull();

    public virtual bool HasActiveObj => obj.IsNotNull() && obj.gameObject.activeSelf;

    public virtual Component ObjInSlot => obj;

    public virtual Transform ObjParent => transform;

    public virtual Indicator Indicator => indicator;

    public virtual void AddObj(Component obj)
    {
        this.obj = obj;
    }

    public virtual void ClearObj()
    {
        obj = null;
    }

    public virtual bool TryAddObj(Component obj)
    {
        if (!CanAdd(obj))
        {
            return false;
        }

        AddObj(obj);
        return true;
    }

    public virtual bool TryRemoveObj(Component obj)
    {
        if (!CanRemove(obj))
        {
            return false;
        }

        ClearObj();
        return true;
    }
    
    public abstract bool CanAdd(Component obj);
    public abstract bool CanRemove(Component obj);
    public virtual void OnObjEnter(Component obj)
    {
    }

    public virtual void OnObjExit(Component obj)
    {
    }
}
