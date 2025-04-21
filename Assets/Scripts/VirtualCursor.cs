using System.Collections;
using System.Collections.Generic;
using JTUtility;
using UnityEngine;

public class VirtualCursor : MonoSingleton<VirtualCursor>
{
    [SerializeField] RectTransform canvas;
    [SerializeField] Camera mainCamera;
    [SerializeField] RectTransform cursor;

    [SerializeField] private float cursorSpeed = 1;

    public static RectTransform CursorTransform => Instance.cursor;
    public static Vector2 AnchoredPosition => Instance.cursor.anchoredPosition;
    public static Vector2 ScreenPosition => Instance.screenPosition;

    public float CursorSpeedMultiplier = 1f;

    private Vector2 screenPosition;

    void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void OnDisable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        var newPosition = cursor.anchoredPosition + new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * CursorSpeedMultiplier * cursorSpeed;
        cursor.anchoredPosition = new Vector2(Mathf.Clamp(newPosition.x, 0, canvas.rect.width), Mathf.Clamp(newPosition.y, 0, canvas.rect.height));
        screenPosition = mainCamera.WorldToScreenPoint(cursor.position);
    }
}
