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
    [SerializeField] List<CharacterSlot> characterSlots;

    [Header("Convenient settings")]
    [SerializeField] List<string> inputResourceids;
    [SerializeField] bool consumeInputResources;
    [SerializeField] bool consumeECore;
    [SerializeField] bool consumeCharacter;

    [Header("Conditions")]
    [SerializeField] List<int> inputAmounts;

    [Header("Events")]
    public UnityEvent onRequirementMet;
    public UnityEvent onRequirementNotMet;
    public UnityEvent onRequirementFinished;

    bool requirementHasMet = false;

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
            if (!requirementHasMet)
            {
                onRequirementMet.Invoke();
                EventDispatcher<string>.Dispatch(EventConstant.ResourceRequirementMet, requirementID);
                requirementHasMet = true;
            }
        }
        else
        {
            if (requirementHasMet)
            {
                onRequirementNotMet.Invoke();
                requirementHasMet = false;
            }
        }
    }

    public void FinishRequirement()
    {
        if (RequirementMet())
        {
            ConsumeResources();
            onRequirementFinished.Invoke();
            EventDispatcher<string>.Dispatch(EventConstant.ResourceRequirementFinished, requirementID);
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

        if (consumeCharacter)
        {
            for (int i = 0; i < characterSlots.Count; i++)
            {
                if (characterSlots[i].HasObj)
                {
                    characterSlots[i].TryRemoveObj(characterSlots[i].ObjInSlot);
                    Destroy(characterSlots[i].ObjInSlot.gameObject);
                }
            }
        }
        else
        {
            for (int i = 0; i < characterSlots.Count; i++)
            {
                var character = characterSlots[i].ObjInSlot as Character;
                if (character.CharacterType == CharacterType.Explorer)
                {
                    character.SetSlot(GameManager.Instance.defaultExplorerSlot);
                }
                else if (character.CharacterType == CharacterType.Worker)
                {
                    character.SetSlot(GameManager.Instance.defaultWorkerSlot);
                }
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

        for (int i = 0; i < characterSlots.Count; i++)
        {
            if (!characterSlots[i].HasObj)
            {
                return false;
            }
        }

        return true;
    }
}
