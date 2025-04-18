using System.Collections;
using System.Collections.Generic;
using JTUtility;
using UnityEngine;
using UnityEngine.Events;

public class ResourceRequirement : MonoBehaviour
{
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
    [SerializeField] UnityEvent onRequirementMet;
    [SerializeField] UnityEvent onRequirementNotMet;

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

    public void CheckRequirement()
    {
        if (RequirementMet())
        {
            ConsumeResources();
            onRequirementMet.Invoke();
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
                eCoreSlots[i].SetCore(null);
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
            if (!eCoreSlots[i].HasCore)
            {
                return false;
            }
        }

        return true;
    }
}
