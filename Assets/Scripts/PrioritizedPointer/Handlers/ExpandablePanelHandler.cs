using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ExpandablePanelHandler : BasePrioritizedPointerHandler
{
    [SerializeField] private GameObject panelToExpand;
    [SerializeField] private float expandDuration = 0.3f;
    [SerializeField] private Vector3 expandedScale = Vector3.one;
    [SerializeField] private Vector3 collapsedScale = Vector3.zero;
    
    private bool isExpanded = false;
    
    protected void Awake()
    {
        priority = PointerPriority.Expandable;
        enableDuringDrag = true;
        ignoreObstacles = false;
        
        // 初始化面板状态
        if (panelToExpand != null)
        {
            panelToExpand.transform.localScale = collapsedScale;
            panelToExpand.SetActive(false);
        }
    }
    
    protected override void OnPointerEnter(PointerEventData eventData)
    {
        if (!isExpanded && panelToExpand != null)
        {
            isExpanded = true;
            panelToExpand.SetActive(true);
            // 添加展开动画
            panelToExpand.transform.DOScale(expandedScale, expandDuration);
        }
    }
    
    protected override void OnPointerExit(PointerEventData eventData)
    {
        if (isExpanded && panelToExpand != null)
        {
            isExpanded = false;
            // 添加收起动画
            panelToExpand.transform.DOScale(collapsedScale, expandDuration)
                .OnComplete(() => panelToExpand.SetActive(false));
        }
    }
} 