using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 优先级指针系统使用示例
/// 展示如何创建不同类型的handler和配置事件处理
/// </summary>
public class ExampleUsage : MonoBehaviour
{
    [Header("示例配置")]
    [SerializeField] private bool enableExamples = true;
    [SerializeField] private GameObject exampleButton;
    [SerializeField] private GameObject examplePanel;
    [SerializeField] private GameObject exampleTooltip;
    
    private void Start()
    {
        if (!enableExamples) return;
        
        // 示例1：创建高优先级按钮handler（消费点击事件）
        CreateButtonHandler();
        
        // 示例2：创建面板handler（处理悬停但不消费事件）
        CreatePanelHandler();
        
        // 示例3：创建工具提示handler（最高优先级，忽略遮挡）
        CreateTooltipHandler();
    }
    
    private void CreateButtonHandler()
    {
        if (exampleButton == null) return;
        
        var buttonHandler = exampleButton.AddComponent<ExampleButtonHandler>();
        buttonHandler.Initialize(
            priority: PointerPriority.Highlight,
            consumeClickEvents: true,  // 消费点击事件
            handleEnterExit: true,
            handleClick: true,
            handleDrag: false
        );
    }
    
    private void CreatePanelHandler()
    {
        if (examplePanel == null) return;
        
        var panelHandler = examplePanel.AddComponent<ExamplePanelHandler>();
        panelHandler.Initialize(
            priority: PointerPriority.Expandable,
            consumeClickEvents: false,  // 不消费点击事件
            handleEnterExit: true,
            handleClick: true,
            handleDrag: true
        );
    }
    
    private void CreateTooltipHandler()
    {
        if (exampleTooltip == null) return;
        
        var tooltipHandler = exampleTooltip.AddComponent<ExampleTooltipHandler>();
        tooltipHandler.Initialize(
            priority: PointerPriority.Tooltip,
            consumeClickEvents: false,
            handleEnterExit: true,
            handleClick: false,
            handleDrag: false,
            ignoreObstacles: true  // 忽略遮挡
        );
    }
}

/// <summary>
/// 示例按钮Handler - 高优先级，消费点击事件
/// </summary>
public class ExampleButtonHandler : BasePrioritizedPointerHandler
{
    private Color originalColor;
    private Renderer renderer;
    
    public void Initialize(PointerPriority priority, bool consumeClickEvents, 
        bool handleEnterExit, bool handleClick, bool handleDrag)
    {
        this.priority = priority;
        this.consumeClickEvents = consumeClickEvents;
        this.handleEnterExit = handleEnterExit;
        this.handleClick = handleClick;
        this.handleDrag = handleDrag;
        
        renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            originalColor = renderer.material.color;
        }
    }
    
    protected override void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"按钮进入: {gameObject.name}");
        if (renderer != null)
        {
            renderer.material.color = Color.yellow;
        }
    }
    
    protected override void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"按钮离开: {gameObject.name}");
        if (renderer != null)
        {
            renderer.material.color = originalColor;
        }
    }
    
    protected override void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"按钮点击: {gameObject.name}");
        // 这里可以添加按钮点击逻辑
    }
}

/// <summary>
/// 示例面板Handler - 中等优先级，不消费事件
/// </summary>
public class ExamplePanelHandler : BasePrioritizedPointerHandler
{
    private Vector3 originalScale;
    
    public void Initialize(PointerPriority priority, bool consumeClickEvents,
        bool handleEnterExit, bool handleClick, bool handleDrag)
    {
        this.priority = priority;
        this.consumeClickEvents = consumeClickEvents;
        this.handleEnterExit = handleEnterExit;
        this.handleClick = handleClick;
        this.handleDrag = handleDrag;
        
        originalScale = transform.localScale;
    }
    
    protected override void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"面板进入: {gameObject.name}");
        transform.localScale = originalScale * 1.1f;
    }
    
    protected override void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"面板离开: {gameObject.name}");
        transform.localScale = originalScale;
    }
    
    protected override void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"面板点击: {gameObject.name}");
        // 面板点击逻辑
    }
    
    protected override void OnDragStart(PointerEventData eventData)
    {
        Debug.Log($"面板开始拖拽: {gameObject.name}");
    }
    
    protected override void OnDragEnd(PointerEventData eventData)
    {
        Debug.Log($"面板结束拖拽: {gameObject.name}");
    }
}

/// <summary>
/// 示例工具提示Handler - 最高优先级，忽略遮挡
/// </summary>
public class ExampleTooltipHandler : BasePrioritizedPointerHandler
{
    private GameObject tooltipUI;
    
    public void Initialize(PointerPriority priority, bool consumeClickEvents,
        bool handleEnterExit, bool handleClick, bool handleDrag, bool ignoreObstacles)
    {
        this.priority = priority;
        this.consumeClickEvents = consumeClickEvents;
        this.handleEnterExit = handleEnterExit;
        this.handleClick = handleClick;
        this.handleDrag = handleDrag;
        this.ignoreObstacles = ignoreObstacles;
    }
    
    protected override void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"工具提示显示: {gameObject.name}");
        ShowTooltip();
    }
    
    protected override void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"工具提示隐藏: {gameObject.name}");
        HideTooltip();
    }
    
    private void ShowTooltip()
    {
        // 显示工具提示UI
        if (tooltipUI == null)
        {
            // 创建工具提示UI
            tooltipUI = new GameObject("Tooltip");
            tooltipUI.transform.SetParent(transform);
            // 这里可以添加UI组件
        }
        tooltipUI.SetActive(true);
    }
    
    private void HideTooltip()
    {
        if (tooltipUI != null)
        {
            tooltipUI.SetActive(false);
        }
    }
} 