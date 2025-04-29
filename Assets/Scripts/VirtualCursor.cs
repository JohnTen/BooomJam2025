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

    public List<RaycastResult> raycastResults = new List<RaycastResult>();
    RaycastResult firstResult;
    public RaycastResult FirstResult => firstResult;

    void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        VerifyComponents();
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
        VerifyComponents();
        var newPosition = cursor.anchoredPosition + new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * (CursorSpeedMultiplier * cursorSpeed);
        cursor.anchoredPosition = new Vector2(Mathf.Clamp(newPosition.x, 0, canvasRectTransform.rect.width), Mathf.Clamp(newPosition.y, 0, canvasRectTransform.rect.height));
        screenPosition = mainCamera.WorldToScreenPoint(cursor.position);

        EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current) { position = screenPosition }, raycastResults);
        firstResult = FindFirstRaycast(raycastResults);
    }

    private RaycastResult FindFirstRaycast(List<RaycastResult> candidates)
    {
        var candidatesCount = candidates.Count;
        for (var i = 0; i < candidatesCount; ++i)
        {
            if (candidates[i].gameObject == null)
                continue;

            return candidates[i];
        }
        return new RaycastResult();
    }
}
