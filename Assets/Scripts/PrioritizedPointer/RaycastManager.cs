using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using JTUtility;

/// <summary>
/// 统一的射线检测管理器
/// 避免多个系统重复执行射线检测，提高性能
/// </summary>
public class RaycastManager : MonoSingleton<RaycastManager>
{
    [Header("射线检测设置")]
    [SerializeField] private LayerMask raycastLayerMask = -1;
    [SerializeField] private int maxRaycastResults = 10;
    
    [Header("性能设置")]
    [SerializeField] private bool enableCaching = true;
    [SerializeField] private float cacheTimeout = 0.016f; // 约60fps
    
    // 射线检测结果缓存
    private List<RaycastResult> cachedRaycastResults = new List<RaycastResult>();
    private Vector2 lastRaycastPosition;
    private float lastRaycastTime;
    private bool isCacheValid = false;
    
    // 事件系统
    private EventSystem eventSystem;
    
    // 单例访问器
    public static List<RaycastResult> RaycastResults => Instance.GetRaycastResults();
    public static RaycastResult FirstResult => Instance.GetFirstResult();
    
    protected override void OnInit()
    {
        eventSystem = EventSystem.current;
        if (eventSystem == null)
        {
            Debug.LogError("RaycastManager: EventSystem not found in scene!");
        }
    }
    
    /// <summary>
    /// 获取射线检测结果（带缓存）
    /// </summary>
    /// <param name="screenPosition">屏幕位置</param>
    /// <returns>射线检测结果列表</returns>
    public List<RaycastResult> GetRaycastResults(Vector2? screenPosition = null)
    {
        Vector2 position = screenPosition ?? VirtualCursor.ScreenPosition;
        
        // 检查缓存是否有效
        if (ShouldUseCache(position))
        {
            return cachedRaycastResults;
        }
        
        // 执行新的射线检测
        PerformRaycast(position);
        return cachedRaycastResults;
    }
    
    /// <summary>
    /// 获取第一个射线检测结果
    /// </summary>
    /// <param name="screenPosition">屏幕位置</param>
    /// <returns>第一个射线检测结果</returns>
    public RaycastResult GetFirstResult(Vector2? screenPosition = null)
    {
        var results = GetRaycastResults(screenPosition);
        return FindFirstRaycast(results);
    }
    
    /// <summary>
    /// 强制刷新射线检测缓存
    /// </summary>
    public void ForceRefresh()
    {
        isCacheValid = false;
    }
    
    /// <summary>
    /// 检查是否应该使用缓存
    /// </summary>
    private bool ShouldUseCache(Vector2 position)
    {
        if (!enableCaching || !isCacheValid)
        {
            return false;
        }
        
        // 检查位置是否变化
        if (Vector2.Distance(position, lastRaycastPosition) > 0.1f)
        {
            return false;
        }
        
        // 检查时间是否超时
        if (Time.time - lastRaycastTime > cacheTimeout)
        {
            return false;
        }
        
        return true;
    }
    
    /// <summary>
    /// 执行射线检测
    /// </summary>
    private void PerformRaycast(Vector2 screenPosition)
    {
        if (eventSystem == null)
        {
            Debug.LogWarning("RaycastManager: EventSystem is null, cannot perform raycast");
            return;
        }
        
        // 创建PointerEventData
        var eventData = new PointerEventData(eventSystem)
        {
            position = screenPosition
        };
        
        // 清空缓存列表
        cachedRaycastResults.Clear();
        
        // 执行射线检测
        eventSystem.RaycastAll(eventData, cachedRaycastResults);
        
        // 限制结果数量
        if (cachedRaycastResults.Count > maxRaycastResults)
        {
            cachedRaycastResults.RemoveRange(maxRaycastResults, cachedRaycastResults.Count - maxRaycastResults);
        }
        
        // 更新缓存状态
        lastRaycastPosition = screenPosition;
        lastRaycastTime = Time.time;
        isCacheValid = true;
    }
    
    /// <summary>
    /// 查找第一个有效的射线检测结果
    /// </summary>
    private RaycastResult FindFirstRaycast(List<RaycastResult> candidates)
    {
        if (candidates == null || candidates.Count == 0)
        {
            return new RaycastResult();
        }
        
        for (int i = 0; i < candidates.Count; i++)
        {
            if (candidates[i].gameObject != null)
            {
                return candidates[i];
            }
        }
        
        return new RaycastResult();
    }
    
    /// <summary>
    /// 获取指定位置的射线检测结果（静态方法）
    /// </summary>
    public static List<RaycastResult> GetRaycastResultsAt(Vector2 screenPosition)
    {
        return Instance.GetRaycastResults(screenPosition);
    }
    
    /// <summary>
    /// 获取指定位置的第一个射线检测结果（静态方法）
    /// </summary>
    public static RaycastResult GetFirstResultAt(Vector2 screenPosition)
    {
        return Instance.GetFirstResult(screenPosition);
    }
    
    /// <summary>
    /// 检查指定位置是否有射线检测结果
    /// </summary>
    public static bool HasRaycastResult(Vector2? screenPosition = null)
    {
        var result = Instance.GetFirstResult(screenPosition);
        return result.gameObject != null;
    }
    
    /// <summary>
    /// 获取射线检测结果数量
    /// </summary>
    public static int GetRaycastResultCount(Vector2? screenPosition = null)
    {
        return Instance.GetRaycastResults(screenPosition).Count;
    }
    
    private void OnDestroy()
    {
        // 清理缓存
        cachedRaycastResults.Clear();
    }
} 