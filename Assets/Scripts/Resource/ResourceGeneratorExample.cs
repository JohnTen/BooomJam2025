using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ResourceGenerator使用示例，展示如何使用新的架构
/// </summary>
public class ResourceGeneratorExample : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image progressBar;
    
    [Header("Generator References")]
    [SerializeField] private ResourceGenerator generator;
    [SerializeField] private ResourceGeneratorObserver observer;
    
    private void Start()
    {
        // 示例1：直接访问生成器数据
        if (generator != null)
        {
            Debug.Log($"生成器状态: {generator.StatusDescription}");
            Debug.Log($"当前进度: {generator.CurrentProgress:P0}");
            Debug.Log($"剩余时间: {generator.RemainingTime:F1}秒");
        }
        
        // 示例2：通过观察者获取多个生成器信息
        if (observer != null)
        {
            var allGenerators = observer.GetAllGeneratorData();
            Debug.Log($"总共有 {allGenerators.Count} 个生成器");
            
            int activeCount = observer.GetActiveGeneratorCount();
            Debug.Log($"正在运行的生成器: {activeCount} 个");
        }
        
        // 示例3：设置UI显示
        SetupUI();
    }
    
    private void Update()
    {
        // 实时更新UI显示
        UpdateUI();
    }
    
    private void SetupUI()
    {
        if (generator == null) return;
        
        // 获取UI组件并设置引用
        var ui = generator.GetComponent<ResourceGeneratorUI>();
        if (ui != null)
        {
            if (progressBar != null) ui.SetProgressBar(progressBar);
            if (timeText != null) ui.SetTimeText(timeText);
            if (statusText != null) ui.SetStatusText(statusText);
            if (descriptionText != null) ui.SetDescriptionText(descriptionText);
        }
    }
    
    private void UpdateUI()
    {
        if (generator == null) return;
        
        // 手动更新UI（如果UI组件不可用）
        if (statusText != null)
        {
            statusText.text = generator.StatusDescription;
        }
        
        if (timeText != null)
        {
            if (generator.IsGenerating)
            {
                timeText.text = $"剩余时间: {generator.RemainingTime:F1}秒";
            }
            else
            {
                timeText.text = "等待条件满足";
            }
        }
        
        if (progressBar != null)
        {
            progressBar.fillAmount = generator.CurrentProgress;
        }
    }
    
    // 示例方法：获取生成器的详细信息
    public void LogGeneratorDetails()
    {
        if (generator?.Data == null) return;
        
        var data = generator.Data;
        Debug.Log($"=== 生成器详细信息 ===");
        Debug.Log($"ID: {data.generatorId}");
        Debug.Log($"输出资源: {data.outputResourceId}");
        Debug.Log($"生成时间: {data.generateTime:F1}秒");
        Debug.Log($"输出数量: {data.outputAmount}");
        Debug.Log($"当前因子: {data.CurrentGenerationFactor:F2}");
        Debug.Log($"是否可生成: {data.CanGenerate}");
        Debug.Log($"状态描述: {data.StatusDescription}");
        
        // 输入资源信息
        for (int i = 0; i < data.inputResourceIds.Count; i++)
        {
            var resourceId = data.inputResourceIds[i];
            var amount = data.inputAmounts[i];
            var template = ResourceDatabase.Instance.GetTemplate(resourceId);
            Debug.Log($"输入资源 {i + 1}: {template.name} x{amount}");
        }
    }
    
    // 示例方法：通过观察者获取所有生成器状态
    public void LogAllGeneratorsStatus()
    {
        if (observer == null) return;
        
        var allData = observer.GetAllGeneratorData();
        Debug.Log($"=== 所有生成器状态 ===");
        
        foreach (var data in allData)
        {
            Debug.Log($"生成器 {data.generatorId}: {data.StatusDescription}");
        }
        
        Debug.Log($"活跃生成器数量: {observer.GetActiveGeneratorCount()}");
    }
} 