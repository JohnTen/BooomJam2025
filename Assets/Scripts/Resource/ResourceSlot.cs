using System;
using System.Collections;
using System.Collections.Generic;
using JTUtility;
using UnityEngine;

public class ResourceSlot : MonoBehaviour
{
    [SerializeField] private string resourceId;
    [SerializeField] private TMPro.TMP_Text resourceNameText;
    [SerializeField] private bool isInput;
    [SerializeField] private Transform resourceObjParent;

    private ResourceObj resourceObj;
    public ResourceObj ResourceInSlot
    {
        get => resourceObj;
    }

    public string ResourceId
    {
        get => resourceId;
        set
        {
            resourceId = value;
            resourceNameText.text = resourceId;
        }
    }

    void OnEnable()
    {
        resourceNameText.text = resourceId;
    }

    public void AddResource(int stack)
    {
        if (resourceObj.IsNotNull())
        {
            resourceObj.Stack += stack;
        }
        else
        {
            resourceObj = Instantiate(PrefabHub.ResourceObjPrefab, resourceObjParent).GetComponent<ResourceObj>();
            resourceObj.Init(ResourceDatabase.Instance.GetTemplate(resourceId), stack, this);
        }
    }

    public bool TryAddResource(ResourceObj inputResourceObj, bool force = false)
    {
        if (inputResourceObj.Template.uid != resourceId)
        {
            Debug.LogWarning("Trying to add resource to a slot with a different resource id");
            return false;
        }

        if (!isInput && !force)
        {
            Debug.LogWarning("Trying to add resource to a non-input slot");
            return false;
        }
        
        if (resourceObj.IsNotNull())
        {
            resourceObj.Stack += inputResourceObj.Stack;
            Destroy(inputResourceObj.gameObject);
            return true;
        }
        else
        {
            resourceObj = inputResourceObj;
            resourceObj.Slot = this;
            resourceObj.transform.SetParent(resourceObjParent);
            resourceObj.transform.localPosition = Vector3.zero;
            return true;
        }
    }

    public ResourceObj TakeResource()
    {
        ResourceObj result;
        if (resourceObj.IsNotNull())
        {
            result = resourceObj;
            resourceObj = null;
        }
        else
        {
            result = null;
        }

        return result;
    }
}
