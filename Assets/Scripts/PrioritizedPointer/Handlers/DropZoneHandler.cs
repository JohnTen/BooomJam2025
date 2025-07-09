using UnityEngine;
using UnityEngine.EventSystems;

public class DropZoneHandler : BasePrioritizedPointerHandler
{
    [SerializeField] private Color validDropColor = Color.green;
    [SerializeField] private Color invalidDropColor = Color.red;
    [SerializeField] private Color normalColor = Color.white;
    
    private Color originalColor;
    private bool canAcceptDrop = false;
    
    protected void Awake()
    {
        priority = PointerPriority.DropZone;
        enableDuringDrag = true;
        ignoreObstacles = true;
    }
    
    protected override void OnPointerEnter(PointerEventData eventData)
    {
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            originalColor = renderer.material.color;
            UpdateDropZoneColor();
        }
    }
    
    protected override void OnPointerExit(PointerEventData eventData)
    {
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = originalColor;
        }
    }
    
    protected override void OnDragStart(PointerEventData eventData)
    {
        // 拖拽开始时检查是否可以接受放置
        canAcceptDrop = CheckIfCanAcceptDrop(eventData);
        UpdateDropZoneColor();
    }
    
    protected override void OnDragEnd(PointerEventData eventData)
    {
        // 拖拽结束时处理放置逻辑
        if (canAcceptDrop && isHovering)
        {
            HandleDrop(eventData);
        }
        
        canAcceptDrop = false;
        UpdateDropZoneColor();
    }
    
    private void UpdateDropZoneColor()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            if (canAcceptDrop)
            {
                renderer.material.color = validDropColor;
            }
            else
            {
                renderer.material.color = normalColor;
            }
        }
    }
    
    private bool CheckIfCanAcceptDrop(PointerEventData eventData)
    {
        // 实现放置验证逻辑
        var draggingObject = FindObjectOfType<Draggable>();
        if (draggingObject != null && draggingObject.IsDragging)
        {
            // 检查拖拽对象的类型是否匹配此放置区域
            return true; // 简化示例
        }
        return false;
    }
    
    private void HandleDrop(PointerEventData eventData)
    {
        // 处理放置逻辑
        Debug.Log($"Item dropped on {gameObject.name} at position {eventData.position}");
    }
} 