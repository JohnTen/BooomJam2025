using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

// 简洁的PointerEventData访问器
public static class SimplePointerEventDataAccessor
{
    private static VirtualInputModule virtualInputModule;
    
    static SimplePointerEventDataAccessor()
    {
        virtualInputModule = Object.FindObjectOfType<VirtualInputModule>();
    }
    
    // 获取当前PointerEventData
    public static PointerEventData GetCurrentPointerEventData(PointerEventData.InputButton button = PointerEventData.InputButton.Left)
    {
        // 尝试从VirtualInputModule获取
        var eventData = GetFromVirtualInputModule(button);
        if (eventData != null)
        {
            return eventData;
        }
        
        // 备用方案：创建新的
        return CreateNewPointerEventData(button);
    }
    
    // 从VirtualInputModule获取PointerEventData
    private static PointerEventData GetFromVirtualInputModule(PointerEventData.InputButton button)
    {
        if (virtualInputModule == null) return null;
        
        try
        {
            return virtualInputModule.GetCurrentPointerEventData(button);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"Failed to get PointerEventData from VirtualInputModule: {e.Message}");
        }
        
        return null;
    }
    
    // 创建新的PointerEventData
    private static PointerEventData CreateNewPointerEventData(PointerEventData.InputButton button)
    {
        var eventData = new PointerEventData(EventSystem.current);
        eventData.position = VirtualCursor.ScreenPosition;
        eventData.button = button;
        eventData.clickTime = Time.time;
        eventData.clickCount = 1;
        
        // 执行射线检测
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        eventData.pointerCurrentRaycast = results.Count > 0 ? results[0] : new RaycastResult();
        
        // 设置拖拽状态
        var draggingObject = Object.FindObjectOfType<Draggable>();
        eventData.dragging = draggingObject != null && draggingObject.IsDragging;
        
        return eventData;
    }
    
    // 检查VirtualInputModule是否可用
    public static bool IsVirtualInputModuleAvailable()
    {
        return virtualInputModule != null;
    }
} 