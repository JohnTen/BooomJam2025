using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverWindow : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private TMPro.TMP_Text title;
    [SerializeField] private TMPro.TMP_Text description;

    private Canvas canvas;
    private RectTransform contentRect;

    private Vector2 origPosition;

    void Start()
    {
        origPosition = transform.localPosition;
        canvas = GetComponentInParent<Canvas>();
        contentRect = content.GetComponent<RectTransform>();
    }

    void Update()
    {
        ShowHoverWindow showHoverWindow = null;
        foreach (var result in VirtualCursor.Instance.raycastResults)
        {
            if (result.gameObject.TryGetComponent(out showHoverWindow))
            {
                break;
            }
        }

        if (showHoverWindow != null && !Input.GetMouseButton(0))
        {
            title.text = TextDatabase.Instance.GetLNItem(showHoverWindow.Title);
            description.text = TextDatabase.Instance.GetLNItem(showHoverWindow.Description);
            content.SetActive(true);
        }
        else
        {
            content.SetActive(false);
        }

        // 确保窗口完全在Canvas内
        if (content.activeSelf)
        {
            // 计算窗口的世界空间边界
            Vector3[] corners = new Vector3[4];
            contentRect.GetWorldCorners(corners);
            
            // 转换为Canvas空间坐标
            for (int i = 0; i < 4; i++)
            {
                corners[i] = canvas.worldCamera.WorldToScreenPoint(corners[i]);
            }
            
            // 计算窗口宽度和高度
            float windowWidth = Mathf.Abs(corners[2].x - corners[0].x);
            
            // 获取当前鼠标位置
            Vector2 cursorPos = VirtualCursor.ScreenPosition;
            
            // 计算Canvas边界
            Vector2 canvasMax = new Vector2(Screen.width, Screen.height);
            
            // 调整位置，防止超出右边界
            Vector3 localPos = transform.localPosition;
            bool needFlip = cursorPos.x + windowWidth > canvasMax.x;
            
            if (needFlip)
            {
                // 翻转到鼠标左侧
                localPos.x = -Mathf.Abs(localPos.x);
            }
            else
            {
                // 保持在鼠标右侧
                localPos.x = Mathf.Abs(localPos.x);
            }
            
            transform.localPosition = localPos;
        }
        else
        {
            // 当窗口不显示时，重置位置
            transform.localPosition = new Vector3(Mathf.Abs(origPosition.x), origPosition.y, 0);
        }
    }
}
