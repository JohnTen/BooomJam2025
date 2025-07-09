using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipHandler : BasePrioritizedPointerHandler
{
    [SerializeField] private string tooltipText;
    [SerializeField] private GameObject tooltipPrefab;
    [SerializeField] private Vector2 tooltipOffset = new Vector2(10, 10);
    
    private GameObject currentTooltip;
    
    protected void Awake()
    {
        priority = PointerPriority.Tooltip;
        enableDuringDrag = false; // 拖拽时不显示提示
        ignoreObstacles = false;  // 被遮挡时不显示
    }
    
    protected override void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltipPrefab != null && currentTooltip == null)
        {
            currentTooltip = Instantiate(tooltipPrefab);
            
            // 使用PointerEventData的屏幕坐标
            Vector2 screenPosition = eventData.position + tooltipOffset;
            
            // 设置提示位置
            var canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                var canvasRect = canvas.GetComponent<RectTransform>();
                Vector2 localPosition;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRect, screenPosition, canvas.worldCamera, out localPosition);
                
                currentTooltip.GetComponent<RectTransform>().anchoredPosition = localPosition;
            }
            
            // 设置提示内容
            var tooltipTextComponent = currentTooltip.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (tooltipTextComponent != null)
            {
                tooltipTextComponent.text = this.tooltipText;
            }
        }
    }
    
    protected override void OnPointerExit(PointerEventData eventData)
    {
        if (currentTooltip != null)
        {
            Destroy(currentTooltip);
            currentTooltip = null;
        }
    }
} 