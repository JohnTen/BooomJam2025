using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BasePrioritizedPointerHandler : MonoBehaviour, IPrioritizedPointerHandler
{
    [SerializeField] protected PointerPriority priority = PointerPriority.Highlight;
    [SerializeField] protected bool enableDuringDrag = true;
    [SerializeField] protected bool ignoreObstacles = false;
    
    [Header("事件处理设置")]
    [SerializeField] protected bool handleEnterExit = true;
    [SerializeField] protected bool handleClick = true;
    [SerializeField] protected bool handleDrag = true;
    [SerializeField] protected bool consumeClickEvents = false;
    [SerializeField] protected bool consumeEnterExitEvents = false;
    
    public PointerPriority Priority => priority;
    public bool EnableDuringDrag => enableDuringDrag;
    public bool IgnoreObstacles => ignoreObstacles;
    
    protected bool isHovering = false;
    
    protected virtual void OnEnable()
    {
        PrioritizedPointerManager.Instance.RegisterHandler(gameObject, this);
    }
    
    protected virtual void OnDisable()
    {
        if (PrioritizedPointerManager.Instance != null)
        {
            PrioritizedPointerManager.Instance.UnregisterHandler(gameObject);
        }
    }
    
    public virtual bool OnPrioritizedPointerEvent(PointerEventType eventType, PointerEventData eventData)
    {
        // 检查是否应该处理此事件
        if (!ShouldHandleEvent(eventType))
        {
            return false;
        }
        
        switch (eventType)
        {
            case PointerEventType.Enter:
                if (!isHovering)
                {
                    isHovering = true;
                    OnPointerEnter(eventData);
                }
                return consumeEnterExitEvents;
                
            case PointerEventType.Exit:
                if (isHovering)
                {
                    isHovering = false;
                    OnPointerExit(eventData);
                }
                return consumeEnterExitEvents;
                
            case PointerEventType.Click:
                OnPointerClick(eventData);
                return consumeClickEvents;
                
            case PointerEventType.DragStart:
                OnDragStart(eventData);
                return false; // 拖拽事件通常不消费
                
            case PointerEventType.DragEnd:
                OnDragEnd(eventData);
                return false; // 拖拽事件通常不消费
                
            default:
                return false;
        }
    }
    
    public virtual bool ShouldHandleEvent(PointerEventType eventType)
    {
        switch (eventType)
        {
            case PointerEventType.Enter:
            case PointerEventType.Exit:
                return handleEnterExit;
            case PointerEventType.Click:
                return handleClick;
            case PointerEventType.DragStart:
            case PointerEventType.DragEnd:
                return handleDrag;
            default:
                return false;
        }
    }
    
    // 子类重写这些方法
    protected virtual void OnPointerEnter(PointerEventData eventData) { }
    protected virtual void OnPointerExit(PointerEventData eventData) { }
    protected virtual void OnPointerClick(PointerEventData eventData) { }
    protected virtual void OnDragStart(PointerEventData eventData) { }
    protected virtual void OnDragEnd(PointerEventData eventData) { }
} 