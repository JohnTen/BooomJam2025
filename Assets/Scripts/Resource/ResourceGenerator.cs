using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JTUtility;
using JTUtility.Event;
using UnityEngine;
using UnityEngine.UI;

public class ResourceGenerator : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private ResourceGeneratorData data;
    
    [Header("UI Components")]
    [SerializeField] private ResourceGeneratorUI uiComponent;
    
    // 运行时状态
    private float generateTimer;
    private bool isGenerating = false;
    
    // 公共接口
    public ResourceGeneratorData Data => data;
    public bool IsGenerating => isGenerating;
    public float CurrentProgress => data.currentProgress;
    public float RemainingTime => data.remainingTime;
    public string StatusDescription => data.StatusDescription;

    private void Start()
    {
        // 初始化数据
        InitializeData();
        
        // 初始化UI
        if (uiComponent != null)
        {
            uiComponent.Initialize(data);
        }
    }
    
    private void InitializeData()
    {
        // 设置输入槽位的资源ID
        if (data.inputResourceIds != null && data.inputSlots != null)
        {
            for (int i = 0; i < data.inputResourceIds.Count && i < data.inputSlots.Count; i++)
            {
                data.inputSlots[i].ResourceId = data.inputResourceIds[i];
            }
        }

        // 设置输出槽位的资源ID
        if (!string.IsNullOrEmpty(data.outputResourceId))
        {
            data.outputSlot.ResourceId = data.outputResourceId;
        }
    }

    private void Update()
    {
        // 更新数据状态
        data.UpdateRuntimeState(isGenerating, generateTimer);
        
        if (data.CanGenerate)
        {
            if (!isGenerating)
            {
                isGenerating = true;
                EventDispatcher<string>.Dispatch(EventConstant.ResourceGeneratorStarted, data.generatorId);
            }

            float factor = data.CurrentGenerationFactor;
            generateTimer += Time.deltaTime * factor;

            if (generateTimer >= data.generateTime)
            {
                GenerateResource();
                generateTimer = 0;
            }
        }
        else
        {
            if (isGenerating)
            {
                isGenerating = false;
                EventDispatcher<string>.Dispatch(EventConstant.ResourceGeneratorStopped, data.generatorId);
            }
            generateTimer = 0;
        }
    }

    private void GenerateResource()
    {
        // 扣除输入资源
        for (int i = 0; i < data.inputSlots.Count; i++)
        {
            var resourceObj = data.inputSlots[i].ResourceInSlot;
            resourceObj.Stack -= data.inputAmounts[i];
        }

        // 添加输出资源
        var outputResourceObj = data.outputSlot.ResourceInSlot;
        if (outputResourceObj == null)
        {
            // 如果输出槽为空，创建新的资源
            data.outputSlot.AddResource(data.outputAmount);
        }
        else
        {
            // 如果输出槽已有资源，增加数量
            outputResourceObj.Stack += data.outputAmount;
        }
    }
}

