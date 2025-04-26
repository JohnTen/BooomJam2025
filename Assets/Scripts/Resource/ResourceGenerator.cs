using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JTUtility;
using JTUtility.Event;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] Image progressBar;

    [SerializeField] string generatorId;
    
    [Header("Slot references")]
    [SerializeField] List<ResourceSlot> inputSlots;
    [SerializeField] List<ECoreSlot> eCoreSlots;
    [SerializeField] CharacterSlot characterSlot;
    [SerializeField] ResourceSlot outputSlot;

    [Header("Convenient settings")]
    [SerializeField] List<string> inputResourceids;
    [SerializeField] string outputResourceid;

    [Header("Generation conditions")]
    [SerializeField] List<int> inputAmounts;
    [SerializeField] List<float> generateTimesFactor;
    [SerializeField] float characterFactor;
    [SerializeField] int outputAmount;

    [SerializeField] float generateTime;

    float generateTimer;

    bool isGenerating = false;
    public bool IsGenerating => isGenerating;

    private void Start()
    {
        progressBar.fillAmount = 0;

        if (!inputResourceids.IsNullOrEmpty())
        {
            for (int i = 0; i < inputResourceids.Count && i < inputSlots.Count; i++)
            {
                inputSlots[i].ResourceId = inputResourceids[i];
            }
        }

        if (!string.IsNullOrEmpty(outputResourceid))
        {
            outputSlot.ResourceId = outputResourceid;
        }
    }

    private void Update()
    {
        if (CheckGenerationConditions())
        {
            if (!isGenerating)
            {
                isGenerating = true;
                EventDispatcher<string>.Dispatch(EventConstant.ResourceGeneratorStarted, generatorId);
            }

            float factor = CalculateGenerationFactor();
            generateTimer += Time.deltaTime * factor;
            progressBar.fillAmount = generateTimer / generateTime;

            if (generateTimer >= generateTime)
            {
                GenerateResource();
                generateTimer = 0;
                progressBar.fillAmount = 0;
            }
        }
        else
        {
            if (isGenerating)
            {
                isGenerating = false;
                EventDispatcher<string>.Dispatch(EventConstant.ResourceGeneratorStopped, generatorId);
            }
            generateTimer = 0;
            progressBar.fillAmount = 0;
        }
    }

    private bool CheckGenerationConditions()
    {
        // 检查输入槽位条件
        for (int i = 0; i < inputSlots.Count; i++)
        {
            var resourceObj = inputSlots[i].ResourceInSlot;
            if (resourceObj == null || resourceObj.Stack < inputAmounts[i])
            {
                return false;
            }
        }

        // 检查是否有核心插槽或至少插入一个能量核心
        bool hasECore = eCoreSlots.Count <= 0;
        foreach (var eCoreSlot in eCoreSlots)
        {
            if (eCoreSlot.HasActiveObj)
            {
                hasECore = true;
                break;
            }
        }

        return hasECore;
    }

    private float CalculateGenerationFactor()
    {
        float totalFactor = generateTimesFactor[eCoreSlots.Count(slot => slot.HasActiveObj)-1];
        if (characterSlot != null &&characterSlot.HasObj)
        {
            totalFactor *= characterFactor;
        }
        
        return totalFactor;
    }

    private void GenerateResource()
    {
        // 扣除输入资源
        for (int i = 0; i < inputSlots.Count; i++)
        {
            var resourceObj = inputSlots[i].ResourceInSlot;
            resourceObj.Stack -= inputAmounts[i];
        }

        // 添加输出资源
        var outputResourceObj = outputSlot.ResourceInSlot;
        if (outputResourceObj == null)
        {
            // 如果输出槽为空，创建新的资源
            outputSlot.AddResource(outputAmount);
        }
        else
        {
            // 如果输出槽已有资源，增加数量
            outputResourceObj.Stack += outputAmount;
        }
    }
}
