using UnityEngine.EventSystems;

public interface IPrioritizedPointerHandler
{
    PointerPriority Priority { get; }
    bool EnableDuringDrag { get; }
    bool IgnoreObstacles { get; }
    
    /// <summary>
    /// 处理优先级指针事件
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="eventData">事件数据</param>
    /// <returns>是否消费此事件（阻止低优先级handler处理）</returns>
    bool OnPrioritizedPointerEvent(PointerEventType eventType, PointerEventData eventData);
    
    /// <summary>
    /// 检查是否应该处理此事件类型
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <returns>是否处理</returns>
    bool ShouldHandleEvent(PointerEventType eventType);
} 