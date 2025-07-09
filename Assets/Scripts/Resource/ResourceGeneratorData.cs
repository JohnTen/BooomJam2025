using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ResourceGeneratorData
{
    [Header("Basic Info")]
    public string generatorId;
    
    [Header("Generation Settings")]
    public List<string> inputResourceIds;
    public List<int> inputAmounts;
    public List<float> generateTimesFactor;
    public float characterFactor;
    public string outputResourceId;
    public int outputAmount;
    public float generateTime;
    
    [Header("Runtime State")]
    public bool isGenerating;
    public float generateTimer;
    public float currentProgress => generateTime > 0 ? generateTimer / generateTime : 0f;
    public float remainingTime => generateTime - generateTimer;
    
    [Header("Slot States")]
    public List<ResourceSlot> inputSlots;
    public List<ECoreSlot> eCoreSlots;
    public CharacterSlot accCharacterSlot;
    public ResourceSlot outputSlot;
    
    // 计算属性
    public bool CanGenerate => CheckGenerationConditions();
    public float CurrentGenerationFactor => CalculateGenerationFactor();
    public string StatusDescription => GetStatusDescription();
    
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
        if (generateTimesFactor == null || generateTimesFactor.Count == 0)
            return 1f;
            
        float totalFactor = generateTimesFactor[Mathf.Max(0, eCoreSlots.Count(slot => slot.HasActiveObj)-1)];
        if (accCharacterSlot != null && accCharacterSlot.HasObj)
        {
            totalFactor *= characterFactor;
        }
        
        return totalFactor;
    }
    
    private string GetStatusDescription()
    {
        if (!CanGenerate)
        {
            return "等待条件满足";
        }
        
        if (isGenerating)
        {
            return $"生产中... ({currentProgress:P0})";
        }
        
        return "就绪";
    }
    
    // 更新运行时状态
    public void UpdateRuntimeState(bool isGenerating, float generateTimer)
    {
        this.isGenerating = isGenerating;
        this.generateTimer = generateTimer;
    }
} 