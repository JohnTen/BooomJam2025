using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 适配器类，用于将旧的ResourceGenerator接口适配到新的架构
/// 保持向后兼容性，同时支持新的数据驱动架构
/// </summary>
public class ResourceGeneratorAdapter : MonoBehaviour
{
    [Header("Legacy Fields - For Backward Compatibility")]
    [SerializeField] private Image progressBar;
    [SerializeField] private string generatorId;
    [SerializeField] private List<ResourceSlot> inputSlots;
    [SerializeField] private List<ECoreSlot> eCoreSlots;
    [SerializeField] private CharacterSlot accCharacterSlot;
    [SerializeField] private ResourceSlot outputSlot;
    [SerializeField] private List<string> inputResourceids;
    [SerializeField] private string outputResourceid;
    [SerializeField] private List<int> inputAmounts;
    [SerializeField] private List<float> generateTimesFactor;
    [SerializeField] private float characterFactor;
    [SerializeField] private int outputAmount;
    [SerializeField] private float generateTime;
    
    [Header("New Architecture")]
    [SerializeField] private ResourceGeneratorData data;
    [SerializeField] private ResourceGeneratorUI uiComponent;
    
    private ResourceGenerator coreGenerator;
    
    private void Awake()
    {
        // 创建核心生成器
        coreGenerator = gameObject.GetComponent<ResourceGenerator>();
        if (coreGenerator == null)
        {
            coreGenerator = gameObject.AddComponent<ResourceGenerator>();
        }
        
        // 如果data为空，从旧字段创建
        if (data == null)
        {
            data = new ResourceGeneratorData();
            MigrateFromLegacyFields();
        }
        
        // 设置核心生成器的数据
        SetPrivateField(coreGenerator, "data", data);
        
        // 如果UI组件为空，创建默认的
        if (uiComponent == null)
        {
            uiComponent = gameObject.AddComponent<ResourceGeneratorUI>();
            if (progressBar != null)
            {
                uiComponent.SetProgressBar(progressBar);
            }
        }
        
        SetPrivateField(coreGenerator, "uiComponent", uiComponent);
    }
    
    private void MigrateFromLegacyFields()
    {
        data.generatorId = generatorId;
        data.outputResourceId = outputResourceid;
        data.inputResourceIds = new List<string>(inputResourceids);
        data.inputAmounts = new List<int>(inputAmounts);
        data.generateTimesFactor = new List<float>(generateTimesFactor);
        data.characterFactor = characterFactor;
        data.outputAmount = outputAmount;
        data.generateTime = generateTime;
        data.inputSlots = new List<ResourceSlot>(inputSlots);
        data.eCoreSlots = new List<ECoreSlot>(eCoreSlots);
        data.accCharacterSlot = accCharacterSlot;
        data.outputSlot = outputSlot;
    }
    
    // 反射设置私有字段
    private void SetPrivateField(object obj, string fieldName, object value)
    {
        var field = obj.GetType().GetField(fieldName, 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null)
        {
            field.SetValue(obj, value);
        }
    }
    
    // 向后兼容的公共接口
    public bool IsGenerating => coreGenerator.IsGenerating;
    public float CurrentProgress => coreGenerator.CurrentProgress;
    public float RemainingTime => coreGenerator.RemainingTime;
    public string StatusDescription => coreGenerator.StatusDescription;
    
    // 提供对数据的访问
    public ResourceGeneratorData Data => data;
    public ResourceGeneratorUI UI => uiComponent;
} 