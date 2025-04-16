using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ECore : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isDragging = false;
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector3 originalPosition;
    private ECoreSlot currentSlot;
    private ECoreSlot previousSlot;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        
        // 如果没有CanvasGroup组件，添加一个
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    private void Start()
    {
        StartCoroutine(InitializePosition());
    }

    private IEnumerator InitializePosition()
    {
        // 等待一帧确保UI系统准备就绪
        yield return null;

        // 使用OverlapPoint检测而不是Raycast
        var rectTransform = GetComponent<RectTransform>();
        var position = rectTransform.position;
        var slots = FindObjectsOfType<ECoreSlot>();

        foreach (var slot in slots)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(
                slot.GetComponent<RectTransform>(),
                Camera.main.WorldToScreenPoint(position),
                Camera.main))
            {
                previousSlot = slot;
                previousSlot.SetHasCore(true);
                rectTransform.position = previousSlot.transform.position;
                break;
            }
        }

        originalPosition = rectTransform.position;
    }
    
    private void Update()
    {
        if (isDragging)
        {
            // 更新位置跟随鼠标
            Vector3 localPoint;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                canvas.GetComponent<RectTransform>(),
                Input.mousePosition,
                canvas.worldCamera,
                out localPoint);
                
            rectTransform.position = localPoint;
            
            // 检测是否悬停在ECoreSlot上
            ECoreSlot newSlot = CheckHoveredSlot();
            
            // 处理slot的状态变化
            if (newSlot != currentSlot)
            {
                if (currentSlot != null)
                {
                    currentSlot.StopBlink();
                }
                if (newSlot != null)
                {
                    newSlot.StartBlink();
                }
                currentSlot = newSlot;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        originalPosition = rectTransform.position;
        
        // 记录之前的slot
        previousSlot = GetOverlappingSlot();
        
        // 确保在最上层显示
        transform.SetAsLastSibling();
        
        // 修改透明度，表示正在拖拽
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        
        // 恢复透明度
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        
        // 检查是否放置在有效的插槽上
        if (currentSlot != null && !currentSlot.HasCore)
        {
            // 停止闪烁并设置状态
            currentSlot.StopBlink();
            currentSlot.SetHasCore(true);
            
            if (previousSlot != null)
            {
                previousSlot.SetHasCore(false);
            }
            
            // 对齐到插槽位置
            rectTransform.position = currentSlot.transform.position;
        }
        else
        {
            // 如果返回原始位置是一个slot，更新其状态
            if (previousSlot != null)
            {
                previousSlot.SetHasCore(true);
                rectTransform.position = previousSlot.transform.position;
            }
            else
            {
                // 返回原始位置
                rectTransform.position = originalPosition;
            }
        }
        
        currentSlot = null;
    }
    
    private ECoreSlot CheckHoveredSlot()
    {
        // 使用射线检测当前鼠标下方是否有ECoreSlot
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;
        
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        
        foreach (RaycastResult result in results)
        {
            ECoreSlot slot = result.gameObject.GetComponent<ECoreSlot>();
            if (slot != null && !slot.HasCore)
            {
                return slot;
            }
        }
        
        return null;
    }

    private ECoreSlot GetOverlappingSlot()
    {
        var position = rectTransform.position;
        var slots = FindObjectsOfType<ECoreSlot>();

        foreach (var slot in slots)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(
                slot.GetComponent<RectTransform>(),
                Camera.main.WorldToScreenPoint(position),
                Camera.main))
            {
                return slot;
            }
        }
        
        return null;
    }
}
