using System;
using System.Collections;
using System.Collections.Generic;
using JTUtility;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ResourceSlot : ObjSlot
{
    [SerializeField] private string resourceId;
    [SerializeField] private TMPro.TMP_Text resourceNameText;
    [SerializeField] private bool isInput;
    [SerializeField] private Transform resourceObjParent;
    [SerializeField] private int maxStack = -1;
    [SerializeField] private UnityEvent reachedMaxStack;

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

    public override Transform ObjParent => resourceObjParent;

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
            if (maxStack <= 0)
            {
                ResourceInSlot.Stack += resourceObj.Stack;
                resourceObj.Stack = 0;
                Destroy(resourceObj.gameObject);
            }
            else
            {
                int toAdd = Mathf.Min(maxStack - ResourceInSlot.Stack, resourceObj.Stack);
                ResourceInSlot.Stack += toAdd;
                resourceObj.Stack -= toAdd;

                if (resourceObj.Stack <= 0)
                {
                    Destroy(resourceObj.gameObject);
                    reachedMaxStack.Invoke();
                }
            }
        }
        else
        {
            if (maxStack > 0 && resourceObj.Stack > maxStack)
            {
                AddResource(maxStack);
                resourceObj.Stack -= maxStack;
                reachedMaxStack.Invoke();
                return;
            }

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

        if (maxStack > 0 && obj is ResourceObj resourceObj && resourceObj.Stack > maxStack)
        {
            return false;
        }

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
