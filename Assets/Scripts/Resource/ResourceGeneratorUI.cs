using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JTUtility.Event;
using JTUtility;

public class ResourceGeneratorUI : MonoBehaviour
{
    [Header("Progress Display")]
    [SerializeField] private Image progressBar;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI statusText;
    
    [Header("Slot Highlights")]
    [SerializeField] private GameObject[] inputSlotHighlights;
    [SerializeField] private GameObject[] eCoreSlotHighlights;
    [SerializeField] private GameObject outputSlotHighlight;
    
    [Header("Description")]
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private string descriptionFormat = "输入: {0}\n输出: {1}\n时间: {2:F1}秒";
    
    private ResourceGeneratorData data;
    private bool isInitialized = false;
    
    public void Initialize(ResourceGeneratorData generatorData)
    {
        data = generatorData;
        isInitialized = true;
        UpdateDescription();
    }
    
    private void Update()
    {
        if (!isInitialized || data == null) return;
        
        UpdateProgressDisplay();
        UpdateStatusText();
        UpdateSlotHighlights();
    }
    
    private void UpdateProgressDisplay()
    {
        if (progressBar != null)
        {
            progressBar.fillAmount = data.currentProgress;
        }
        
        if (timeText != null)
        {
            if (data.isGenerating)
            {
                timeText.text = $"剩余时间: {data.remainingTime:F1}秒";
            }
            else
            {
                timeText.text = $"生成时间: {data.generateTime:F1}秒";
            }
        }
    }
    
    private void UpdateStatusText()
    {
        if (statusText != null)
        {
            statusText.text = data.StatusDescription;
        }
    }
    
    private void UpdateSlotHighlights()
    {
        // 更新输入槽位高亮
        if (!inputSlotHighlights.IsNullOrEmpty())
        {
            for (int i = 0; i < inputSlotHighlights.Length && i < data.inputSlots.Count; i++)
            {
                if (inputSlotHighlights[i] != null)
                {
                    bool shouldHighlight = data.inputSlots[i].ResourceInSlot != null && 
                                        data.inputSlots[i].ResourceInSlot.Stack >= data.inputAmounts[i];
                    inputSlotHighlights[i].SetActive(shouldHighlight);
                }
            }
        }
        
        // 更新能量核心槽位高亮
        if (!eCoreSlotHighlights.IsNullOrEmpty())
        {
            for (int i = 0; i < eCoreSlotHighlights.Length && i < data.eCoreSlots.Count; i++)
            {
                if (eCoreSlotHighlights[i] != null)
                {
                    bool shouldHighlight = data.eCoreSlots[i].HasActiveObj;
                    eCoreSlotHighlights[i].SetActive(shouldHighlight);
                }
            }
        }
        
        // 更新输出槽位高亮
        if (outputSlotHighlight != null)
        {
            bool shouldHighlight = data.outputSlot.ResourceInSlot != null;
            outputSlotHighlight.SetActive(shouldHighlight);
        }
    }
    
    private void UpdateDescription()
    {
        if (descriptionText == null || data == null) return;
        
        string inputDesc = "";
        for (int i = 0; i < data.inputResourceIds.Count; i++)
        {
            if (i > 0) inputDesc += ", ";
            var template = ResourceDatabase.Instance.GetTemplate(data.inputResourceIds[i]);
            inputDesc += $"{template.name} x{data.inputAmounts[i]}";
        }
        
        string outputDesc = "";
        var outputTemplate = ResourceDatabase.Instance.GetTemplate(data.outputResourceId);
        outputDesc = $"{outputTemplate.name} x{data.outputAmount}";
        
        descriptionText.text = string.Format(descriptionFormat, inputDesc, outputDesc, data.generateTime);
    }
    
    // 公共接口供外部调用
    public void SetProgressBar(Image newProgressBar)
    {
        progressBar = newProgressBar;
    }
    
    public void SetTimeText(TextMeshProUGUI newTimeText)
    {
        timeText = newTimeText;
    }
    
    public void SetStatusText(TextMeshProUGUI newStatusText)
    {
        statusText = newStatusText;
    }
    
    public void SetDescriptionText(TextMeshProUGUI newDescriptionText)
    {
        descriptionText = newDescriptionText;
        UpdateDescription();
    }
} 