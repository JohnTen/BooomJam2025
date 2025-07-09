using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class HighlightHandler : BasePrioritizedPointerHandler
{
    [SerializeField] private Color highlightColor = Color.yellow;
    [SerializeField] private float highlightScale = 1.2f;
    [SerializeField] private float animationDuration = 0.2f;
    
    private Color originalColor;
    private Vector3 originalScale;
    private Material originalMaterial;
    
    protected void Awake()
    {
        priority = PointerPriority.Highlight;
        enableDuringDrag = true;  // 拖拽时保持高亮
        ignoreObstacles = true;   // 即使被遮挡也高亮
    }
    
    protected override void OnPointerEnter(PointerEventData eventData)
    {
        // 保存原始状态
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            originalMaterial = renderer.material;
            originalColor = originalMaterial.color;
            renderer.material.color = highlightColor;
        }
        
        originalScale = transform.localScale;
        transform.DOScale(originalScale * highlightScale, animationDuration);
    }
    
    protected override void OnPointerExit(PointerEventData eventData)
    {
        // 恢复原始状态
        var renderer = GetComponent<Renderer>();
        if (renderer != null && originalMaterial != null)
        {
            renderer.material.color = originalColor;
        }
        
        transform.DOScale(originalScale, animationDuration);
    }
} 