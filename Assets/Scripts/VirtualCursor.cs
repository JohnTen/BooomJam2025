using System.Collections;
using System.Collections.Generic;
using JTUtility;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualCursor : MonoSingleton<VirtualCursor>
{
    [SerializeField] Canvas canvas;
    [SerializeField] Camera mainCamera;
    [SerializeField] RectTransform cursor;

    [SerializeField] private float cursorSpeed = 1;

    public static RectTransform CursorTransform => Instance.cursor;
    public static Vector2 AnchoredPosition => Instance.cursor.anchoredPosition;
    public static Vector2 ScreenPosition => Instance.screenPosition;

    public float CursorSpeedMultiplier = 1f;

    private RectTransform canvasRectTransform;

    private Vector2 screenPosition;

    private static Vector2 lastPosition;

    // 使用统一的RaycastManager
    public static List<RaycastResult> RaycastResults => RaycastManager.RaycastResults;
    public static RaycastResult FirstResult => RaycastManager.FirstResult;

    void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        VerifyComponents();
        cursor.anchoredPosition = lastPosition;
    }

    void OnDisable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void VerifyComponents()
    {
        if (mainCamera.IsNull())
        {
            mainCamera = Camera.main;
        }

        if (canvas.IsNull())
        {
            canvas = GetComponentInParent<Canvas>();
        }
        
        if (canvas.IsNotNull())
        {
            if (canvasRectTransform.IsNull())
            {
                canvasRectTransform = canvas.GetComponent<RectTransform>();
            }

            if (canvas.worldCamera.IsNull())
            {
                canvas.worldCamera = mainCamera;
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }

        VerifyComponents();
        var newPosition = cursor.anchoredPosition + new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * (CursorSpeedMultiplier * cursorSpeed);
        cursor.anchoredPosition = new Vector2(Mathf.Clamp(newPosition.x, 0, canvasRectTransform.rect.width), Mathf.Clamp(newPosition.y, 0, canvasRectTransform.rect.height));
        screenPosition = mainCamera.WorldToScreenPoint(cursor.position);
        lastPosition = cursor.anchoredPosition;

        // 使用统一的RaycastManager，不再重复执行射线检测
        // 射线检测现在由RaycastManager统一管理，通过raycastResults和firstResult属性访问

        if (GameManager.HasInstance)
        {
            CursorSpeedMultiplier = GameManager.Instance.GameProperty.CoreDragSpeed;
        }
    }
}
