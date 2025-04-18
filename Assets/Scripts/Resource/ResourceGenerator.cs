using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] Image progressBar;
    [SerializeField] List<ResourceSlot> inputSlots;
    [SerializeField] List<ECoreSlot> eCoreSlots;
    [SerializeField] ResourceSlot outputSlot;

    [SerializeField] List<int> inputAmounts;
    [SerializeField] List<float> generateTimesFactor;
    [SerializeField] int outputAmount;

    [SerializeField] float generateTime;

    float generateTimer;

    private void Start()
    {
        progressBar.fillAmount = 0;
    }

    private void Update()
    {
        if (CheckGenerationConditions())
        {
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

        // 检查是否有至少一个能量核心
        bool hasECore = false;
        foreach (var eCoreSlot in eCoreSlots)
        {
            if (eCoreSlot.HasCore)
            {
                hasECore = true;
                break;
            }
        }

        return hasECore;
    }

    private float CalculateGenerationFactor()
    {
        float totalFactor = generateTimesFactor[eCoreSlots.Count(slot => slot.HasCore)-1];
        
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
