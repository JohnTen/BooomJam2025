using System.Collections.Generic;
using System.Linq;
using JTUtility;
using UnityEngine;
using UnityEngine.EventSystems;

public class PrioritizedPointerManager : MonoSingleton<PrioritizedPointerManager>
{
    [Header("射线检测设置")]
    [SerializeField] private LayerMask raycastLayerMask = -1;
    [SerializeField] private int maxRaycastResults = 10;
    [SerializeField] private int maxParentTraversalDepth = 10;
    
    [Header("性能设置")]
    [SerializeField] private bool enableEventConsumption = true;
    [SerializeField] private bool enableDebugLogging = false;
    
    private Dictionary<GameObject, IPrioritizedPointerHandler> registeredHandlers = new Dictionary<GameObject, IPrioritizedPointerHandler>();
    private List<GameObject> currentHoveredObjects = new List<GameObject>();
    private List<GameObject> previousHoveredObjects = new List<GameObject>();
    
    // 使用统一的RaycastManager，不再需要自己的射线检测缓存
    private List<GameObject> validObjects = new List<GameObject>();
    private List<IPrioritizedPointerHandler> sortedHandlers = new List<IPrioritizedPointerHandler>();
    
    // 性能优化缓存
    private HashSet<GameObject> processedObjects = new HashSet<GameObject>();
    private Dictionary<GameObject, List<IPrioritizedPointerHandler>> handlerCache = new Dictionary<GameObject, List<IPrioritizedPointerHandler>>();
    
    private bool isAnyDragging = false;
    private PointerEventData lastPointerEventData;
    
    // 帧缓存，避免重复计算
    private int lastProcessedFrame = -1;
    private Vector2 lastMousePosition;
    
    private void Update()
    {
        var currentPointerEventData = SimplePointerEventDataAccessor.GetCurrentPointerEventData();
        if (currentPointerEventData != null)
        {
            UpdatePointerState(currentPointerEventData);
        }
    }
    
    public void RegisterHandler(GameObject obj, IPrioritizedPointerHandler handler)
    {
        registeredHandlers[obj] = handler;
        // 清除缓存，因为注册了新的handler
        handlerCache.Clear();
    }
    
    public void UnregisterHandler(GameObject obj)
    {
        registeredHandlers.Remove(obj);
        // 清除缓存
        handlerCache.Clear();
    }
    
    private void UpdatePointerState(PointerEventData eventData)
    {
        // 检查是否需要重新计算（鼠标位置变化或新帧）
        if (ShouldSkipProcessing(eventData))
        {
            return;
        }
        
        // 更新拖拽状态
        UpdateDragState(eventData);
        
        // 更新悬停状态
        UpdateHoverState(eventData);
        
        lastPointerEventData = eventData;
        lastMousePosition = eventData.position;
        lastProcessedFrame = Time.frameCount;
    }
    
    private bool ShouldSkipProcessing(PointerEventData eventData)
    {
        // 如果鼠标位置没有变化且在同一帧内，跳过处理
        if (lastProcessedFrame == Time.frameCount && 
            Vector2.Distance(lastMousePosition, eventData.position) < 0.1f)
        {
            return true;
        }
        return false;
    }
    
    private void UpdateDragState(PointerEventData eventData)
    {
        bool wasDragging = isAnyDragging;
        isAnyDragging = eventData.dragging;
        
        // 检测拖拽状态变化
        if (!wasDragging && isAnyDragging)
        {
            NotifyHandlers(PointerEventType.DragStart, eventData);
        }
        else if (wasDragging && !isAnyDragging)
        {
            NotifyHandlers(PointerEventType.DragEnd, eventData);
        }
    }
    
    private void UpdateHoverState(PointerEventData eventData)
    {
        // 保存上一帧的悬停对象
        previousHoveredObjects.Clear();
        previousHoveredObjects.AddRange(currentHoveredObjects);
        currentHoveredObjects.Clear();
        
        // 尝试使用RaycastManager，如果不可用则使用备用方案
        List<RaycastResult> raycastResults;
        
        // 检查RaycastManager是否可用
        if (RaycastManager.HasInstance)
        {
            raycastResults = RaycastManager.RaycastResults;
        }
        else
        {
            raycastResults = PerformDirectRaycast(eventData);
        }
        
        // 收集所有有效的handler
        CollectValidHandlers(raycastResults);
        
        // 按优先级排序
        SortHandlersByPriority();
        
        // 更新当前悬停对象列表
        UpdateCurrentHoveredObjects();
        
        // 触发事件
        TriggerEvents(eventData);
        
        if (enableDebugLogging)
        {
            DebugLogRaycastResults(raycastResults);
        }
    }
    
    /// <summary>
    /// 执行直接射线检测（备用方案）
    /// </summary>
    private List<RaycastResult> PerformDirectRaycast(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        
        // 限制结果数量以提高性能
        if (results.Count > maxRaycastResults)
        {
            results.RemoveRange(maxRaycastResults, results.Count - maxRaycastResults);
        }
        
        return results;
    }
    
    private void CollectValidHandlers(List<RaycastResult> raycastResults)
    {
        validObjects.Clear();
        sortedHandlers.Clear();
        processedObjects.Clear();
        
        // 收集射线检测结果中的有效对象
        foreach (var result in raycastResults)
        {
            if (result.gameObject != null)
            {
                CollectHandlersFromObject(result.gameObject);
            }
        }
        
        // 处理忽略遮挡的handler
        foreach (var kvp in registeredHandlers)
        {
            if (kvp.Value.IgnoreObstacles && ShouldProcessHandler(kvp.Value))
            {
                if (!validObjects.Contains(kvp.Key))
                {
                    validObjects.Add(kvp.Key);
                    sortedHandlers.Add(kvp.Value);
                }
            }
        }
    }
    
    private void CollectHandlersFromObject(GameObject obj)
    {
        // 检查缓存
        if (handlerCache.ContainsKey(obj))
        {
            foreach (var handler in handlerCache[obj])
            {
                if (ShouldProcessHandler(handler) && !validObjects.Contains(obj))
                {
                    validObjects.Add(obj);
                    sortedHandlers.Add(handler);
                }
            }
            return;
        }
        
        // 缓存未命中，执行完整遍历
        var handlers = new List<IPrioritizedPointerHandler>();
        int traversalDepth = 0;
        Transform current = obj.transform;
        
        // 向上追溯直到根节点或达到最大深度
        while (current != null && traversalDepth < maxParentTraversalDepth)
        {
            if (registeredHandlers.ContainsKey(current.gameObject))
            {
                var handler = registeredHandlers[current.gameObject];
                handlers.Add(handler);
                
                if (ShouldProcessHandler(handler) && !validObjects.Contains(current.gameObject))
                {
                    validObjects.Add(current.gameObject);
                    sortedHandlers.Add(handler);
                }
            }
            
            current = current.parent;
            traversalDepth++;
        }
        
        // 缓存结果
        handlerCache[obj] = handlers;
    }
    
    private void SortHandlersByPriority()
    {
        // 按优先级排序（数值越小优先级越高）
        sortedHandlers.Sort((a, b) => a.Priority.CompareTo(b.Priority));
    }
    
    private void UpdateCurrentHoveredObjects()
    {
        currentHoveredObjects.Clear();
        foreach (var handler in sortedHandlers)
        {
            // 找到对应的GameObject
            foreach (var kvp in registeredHandlers)
            {
                if (kvp.Value == handler)
                {
                    currentHoveredObjects.Add(kvp.Key);
                    break;
                }
            }
        }
    }
    
    private bool ShouldProcessHandler(IPrioritizedPointerHandler handler)
    {
        if (isAnyDragging)
        {
            return handler.EnableDuringDrag;
        }
        return true;
    }
    
    private void TriggerEvents(PointerEventData eventData)
    {
        // 处理新进入的对象
        foreach (var obj in currentHoveredObjects)
        {
            if (!previousHoveredObjects.Contains(obj))
            {
                var handler = registeredHandlers[obj];
                if (handler.ShouldHandleEvent(PointerEventType.Enter))
                {
                    bool consumed = handler.OnPrioritizedPointerEvent(PointerEventType.Enter, eventData);
                    if (enableEventConsumption && consumed)
                    {
                        break; // 事件被消费，停止处理
                    }
                }
            }
        }
        
        // 处理离开的对象
        foreach (var obj in previousHoveredObjects)
        {
            if (!currentHoveredObjects.Contains(obj))
            {
                var handler = registeredHandlers[obj];
                if (handler.ShouldHandleEvent(PointerEventType.Exit))
                {
                    bool consumed = handler.OnPrioritizedPointerEvent(PointerEventType.Exit, eventData);
                    if (enableEventConsumption && consumed)
                    {
                        break; // 事件被消费，停止处理
                    }
                }
            }
        }
    }
    
    private void NotifyHandlers(PointerEventType eventType, PointerEventData eventData)
    {
        // 按优先级顺序通知所有handler
        foreach (var handler in sortedHandlers)
        {
            if (handler.ShouldHandleEvent(eventType))
            {
                bool consumed = handler.OnPrioritizedPointerEvent(eventType, eventData);
                
                // 如果启用了事件消费机制且事件被消费，停止传播
                if (enableEventConsumption && consumed)
                {
                    break;
                }
            }
        }
    }
    
    // 公共方法，供外部调用
    public void NotifyClick(PointerEventData eventData)
    {
        NotifyHandlers(PointerEventType.Click, eventData);
    }
    
    // 清理缓存的方法
    public void ClearCache()
    {
        handlerCache.Clear();
        processedObjects.Clear();
    }
    
    // 调试方法
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private void DebugLogRaycastResults(List<RaycastResult> raycastResults)
    {
        Debug.Log($"射线检测结果数量: {raycastResults.Count}");
        for (int i = 0; i < raycastResults.Count; i++)
        {
            var result = raycastResults[i];
            Debug.Log($"结果 {i}: {result.gameObject.name} (深度: {result.depth})");
        }
        
        Debug.Log($"有效Handler数量: {sortedHandlers.Count}");
        for (int i = 0; i < sortedHandlers.Count; i++)
        {
            var handler = sortedHandlers[i];
            Debug.Log($"Handler {i}: {handler.GetType().Name} (优先级: {handler.Priority})");
        }
    }
    
    private void OnDestroy()
    {
        // 清理缓存
        ClearCache();
    }
} 