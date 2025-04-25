using System;
using System.Collections;
using System.Collections.Generic;
using JTUtility;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceSlot : ObjSlot
{
    [SerializeField] private string resourceId;
    [SerializeField] private TMPro.TMP_Text resourceNameText;
    [SerializeField] private bool isInput;
    [SerializeField] private Transform resourceObjParent;

    public ResourceObj ResourceInSlot
    {
        get => ObjInSlot as ResourceObj;
    }

    public string ResourceId
    {
        get => resourceId;
        set
        {
            resourceId = value;
            resourceNameText.text = ResourceDatabase.Instance.GetTemplate(resourceId).name;
        }
    }

    void OnEnable()
    {
        resourceNameText.text = ResourceDatabase.Instance.GetTemplate(resourceId).name;
    }

    public void AddResource(int stack)
    {
        if (ResourceInSlot.IsNotNull())
        {
            ResourceInSlot.Stack += stack;
        }
        else
        {
            var resourceObj = Instantiate(PrefabHub.ResourceObjPrefab, resourceObjParent).GetComponent<ResourceObj>();
            resourceObj.Init(ResourceDatabase.Instance.GetTemplate(resourceId), stack, this);
            resourceObj.SetSlot(this);
        }
    }

    public override void AddObj(Component obj)
    {
        var resourceObj = obj as ResourceObj;
        if (ResourceInSlot.IsNotNull())
        {
            ResourceInSlot.Stack += resourceObj.Stack;
            Destroy(resourceObj.gameObject);
        }
        else
        {
            base.AddObj(obj);
        }
    }

    public override void ClearObj()
    {
        base.ClearObj();
    }

    public override bool TryAddObj(Component obj)
    {
        if (!CanAdd(obj))
        {
            return false;
        }
        
        AddObj(obj);
        return true;
    }

    public override bool CanAdd(Component obj)
    {
        return obj is ResourceObj resourceObj && resourceObj.Template.uid == resourceId && (isInput || resourceObj.CurrentSlot == this);
    }

    public override bool CanRemove(Component obj)
    {
        if (obj != ObjInSlot)
        {
            Debug.LogWarning("Trying to remove an object from a resource slot that is not the same as the object in the slot");
            return false;
        }

        return true;
    }

    public override void OnObjEnter(Component obj)
    {
    }
    
    public override void OnObjExit(Component obj)
    {
    }
}
