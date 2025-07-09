using System.Collections.Generic;
using UnityEngine;
using JTUtility.Event;

/// <summary>
/// ResourceGenerator观察者，为外部系统提供统一的接口来获取生成器状态
/// </summary>
public class ResourceGeneratorObserver : MonoBehaviour
{
    [Header("Observed Generators")]
    [SerializeField] private List<ResourceGenerator> observedGenerators;
    
    [Header("Event Callbacks")]
    [SerializeField] private UnityEngine.Events.UnityEvent<ResourceGeneratorData> onGeneratorStateChanged;
    [SerializeField] private UnityEngine.Events.UnityEvent<string> onGeneratorStarted;
    [SerializeField] private UnityEngine.Events.UnityEvent<string> onGeneratorStopped;
    
    private Dictionary<string, ResourceGeneratorData> generatorStates = new Dictionary<string, ResourceGeneratorData>();
    
    private void Start()
    {
        // 注册事件监听
        EventRegister<string>.Register(EventConstant.ResourceGeneratorStarted, OnGeneratorStarted);
        EventRegister<string>.Register(EventConstant.ResourceGeneratorStopped, OnGeneratorStopped);
        
        // 初始化观察的生成器
        InitializeObservedGenerators();
    }
    
    private void OnDestroy()
    {
        // 注销事件监听
        EventRegister<string>.UnRegister(EventConstant.ResourceGeneratorStarted, OnGeneratorStarted);
        EventRegister<string>.UnRegister(EventConstant.ResourceGeneratorStopped, OnGeneratorStopped);
    }
    
    private void InitializeObservedGenerators()
    {
        foreach (var generator in observedGenerators)
        {
            if (generator != null && generator.Data != null)
            {
                generatorStates[generator.Data.generatorId] = generator.Data;
            }
        }
    }
    
    private void OnGeneratorStarted(string generatorId)
    {
        onGeneratorStarted?.Invoke(generatorId);
        UpdateGeneratorState(generatorId);
    }
    
    private void OnGeneratorStopped(string generatorId)
    {
        onGeneratorStopped?.Invoke(generatorId);
        UpdateGeneratorState(generatorId);
    }
    
    private void UpdateGeneratorState(string generatorId)
    {
        var generator = FindGeneratorById(generatorId);
        if (generator != null && generator.Data != null)
        {
            generatorStates[generatorId] = generator.Data;
            onGeneratorStateChanged?.Invoke(generator.Data);
        }
    }
    
    // 公共接口方法
    
    /// <summary>
    /// 获取指定生成器的数据
    /// </summary>
    public ResourceGeneratorData GetGeneratorData(string generatorId)
    {
        if (generatorStates.TryGetValue(generatorId, out var data))
        {
            return data;
        }
        
        var generator = FindGeneratorById(generatorId);
        return generator?.Data;
    }
    
    /// <summary>
    /// 获取所有生成器的状态
    /// </summary>
    public List<ResourceGeneratorData> GetAllGeneratorData()
    {
        var result = new List<ResourceGeneratorData>();
        foreach (var generator in observedGenerators)
        {
            if (generator?.Data != null)
            {
                result.Add(generator.Data);
            }
        }
        return result;
    }
    
    /// <summary>
    /// 获取正在运行的生成器数量
    /// </summary>
    public int GetActiveGeneratorCount()
    {
        int count = 0;
        foreach (var generator in observedGenerators)
        {
            if (generator?.IsGenerating == true)
            {
                count++;
            }
        }
        return count;
    }
    
    /// <summary>
    /// 获取指定生成器的UI组件
    /// </summary>
    public ResourceGeneratorUI GetGeneratorUI(string generatorId)
    {
        var generator = FindGeneratorById(generatorId);
        return generator?.GetComponent<ResourceGeneratorUI>();
    }
    
    /// <summary>
    /// 检查生成器是否满足运行条件
    /// </summary>
    public bool IsGeneratorReady(string generatorId)
    {
        var data = GetGeneratorData(generatorId);
        return data?.CanGenerate ?? false;
    }
    
    /// <summary>
    /// 获取生成器的状态描述
    /// </summary>
    public string GetGeneratorStatus(string generatorId)
    {
        var data = GetGeneratorData(generatorId);
        return data?.StatusDescription ?? "未知状态";
    }
    
    /// <summary>
    /// 添加观察的生成器
    /// </summary>
    public void AddObservedGenerator(ResourceGenerator generator)
    {
        if (generator != null && !observedGenerators.Contains(generator))
        {
            observedGenerators.Add(generator);
            if (generator.Data != null)
            {
                generatorStates[generator.Data.generatorId] = generator.Data;
            }
        }
    }
    
    /// <summary>
    /// 移除观察的生成器
    /// </summary>
    public void RemoveObservedGenerator(ResourceGenerator generator)
    {
        if (generator != null && observedGenerators.Contains(generator))
        {
            observedGenerators.Remove(generator);
            if (generator.Data != null)
            {
                generatorStates.Remove(generator.Data.generatorId);
            }
        }
    }
    
    private ResourceGenerator FindGeneratorById(string generatorId)
    {
        foreach (var generator in observedGenerators)
        {
            if (generator?.Data?.generatorId == generatorId)
            {
                return generator;
            }
        }
        return null;
    }
} 