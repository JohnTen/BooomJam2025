using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public bool CanDrag { get; set; } = true;

    private bool isDragging = false;
    private Vector3 dragOffset;
    private Camera mainCamera;
    private Plane dragPlane;

    private Canvas canvas;

    private Transform parent;

    public bool IsDragging => isDragging;
    public UnityEvent OnDragStart;
    public UnityEvent OnDragEnd;
    public UnityEvent<Vector3> OnDragging;

    private void Awake()
    {
        mainCamera = Camera.main;
        canvas = GetComponentInParent<Canvas>();
        print(mainCamera);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!CanDrag) return;
        isDragging = true;

        // 创建一个与相机平行的平面
        dragPlane = new Plane(mainCamera.transform.forward, transform.position);
        
        // 计算鼠标射线与平面的交点
        Ray ray = mainCamera.ScreenPointToRay(VirtualCursor.ScreenPosition);
        float enter;
        if (dragPlane.Raycast(ray, out enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            dragOffset = transform.position - hitPoint;
        }

        parent = transform.parent;
        transform.SetParent(canvas.transform);

        OnDragStart.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        Ray ray = mainCamera.ScreenPointToRay(VirtualCursor.ScreenPosition);
        float enter;
        if (dragPlane.Raycast(ray, out enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            transform.position = hitPoint + dragOffset;
            OnDragging?.Invoke(transform.position);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDragging) return;
        isDragging = false;
        transform.SetParent(parent);
        OnDragEnd.Invoke();
    }
}
