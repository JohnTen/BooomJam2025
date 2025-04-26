using System.Collections;
using System.Collections.Generic;
using JTUtility;
using JTUtility.Event;
using UnityEngine;
using UnityEngine.Events;

public class ResourceRequirement : MonoBehaviour
{
    [SerializeField] private string requirementID;
    [SerializeField] private bool autoCheck = true;
    [Header("Slot references")]
    [SerializeField] List<ResourceSlot> inputSlots;
    [SerializeField] List<ECoreSlot> eCoreSlots;

    [Header("Convenient settings")]
    [SerializeField] List<string> inputResourceids;
    [SerializeField] bool consumeInputResources;
    [SerializeField] bool consumeECore;

    [Header("Conditions")]
    [SerializeField] List<int> inputAmounts;

    [Header("Events")]
    public UnityEvent onRequirementMet;
    public UnityEvent onRequirementNotMet;

    private void Start()
    {
        if (!inputResourceids.IsNullOrEmpty())
        {
            for (int i = 0; i < inputResourceids.Count && i < inputSlots.Count; i++)
            {
                inputSlots[i].ResourceId = inputResourceids[i];
            }
        }
    }

    private void Update()
    {
        if (autoCheck)
        {
            CheckRequirement();
        }
    }

    public void CheckRequirement()
    {
        if (RequirementMet())
        {
            ConsumeResources();
            onRequirementMet.Invoke();
            EventDispatcher<string>.Dispatch(EventConstant.ResourceRequirementMet, requirementID);
        }
        else
        {
            onRequirementNotMet.Invoke();
        }
    }

    private void ConsumeResources()
    {
        if (consumeInputResources)
        {
            for (int i = 0; i < inputSlots.Count; i++)
            {
                inputSlots[i].ResourceInSlot.Stack -= inputAmounts[i];
            }
        }

        if (consumeECore)
        {
            for (int i = 0; i < eCoreSlots.Count; i++)
            {
                var core = eCoreSlots[i].ECoreInSlot;
                eCoreSlots[i].TryRemoveObj(core);
                Destroy(core.gameObject);
            }
        }
    }   
    
    private bool RequirementMet()
    {
        for (int i = 0; i < inputSlots.Count; i++)
        {
            if (inputSlots[i].ResourceInSlot == null || inputSlots[i].ResourceInSlot.Stack < inputAmounts[i])
            {
                return false;
            }
        }

        for (int i = 0; i < eCoreSlots.Count; i++)
        {
            if (!eCoreSlots[i].HasObj)
            {
                return false;
            }
        }

        return true;
    }
}
