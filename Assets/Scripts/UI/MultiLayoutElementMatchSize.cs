using JTUtility;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(LayoutElement))]
public class MultiLayoutElementMatchSize : MonoBehaviour
{
    //[HideInInspector]
    public RectTransform Self;

    //[HideInInspector]
    public LayoutElement layoutElement;

    public RectTransform[] Targets;
    public Canvas canvas;

    public bool MatchPos;
    public bool MatchHeight = true;
    public bool MatchWidth;

    public Vector2 matchSizeOffset = Vector2.zero;
    public Vector2 matchPosOffset = Vector2.zero;

    public void Start()
    {
        if (Self == null)
        {
            Self = GetComponent<RectTransform>();
        }

        layoutElement = GetComponent<LayoutElement>();
        if (layoutElement == null)
        {
            layoutElement = this.gameObject.AddComponent<LayoutElement>();
        }

        layoutElement.preferredHeight = 0;
        layoutElement.preferredWidth = 0;

        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();
    }

    public Rect GetCanvasRect(Canvas canvas, Rect worldRect)
    {
        var size = worldRect.size;
        //worldRect.min = new Vector2(worldRect.min.x / canvas.transform.localScale.x, worldRect.min.y / canvas.transform.localScale.y);
        worldRect.size = new Vector2(size.x / canvas.transform.localScale.x, size.y / canvas.transform.localScale.y);
        return worldRect;
    }

    private Rect rect;

    private void Update()
    {
        //if (MatchHeight)
        //{
        //    if (layoutElement.preferredHeight != Target.sizeDelta.y)
        //    {
        //        layoutElement.preferredHeight = Target.sizeDelta.y;
        //        Self.sizeDelta = new Vector2(Self.sizeDelta.x, Target.sizeDelta.y);
        //    }
        //}

        if (Targets == null || Targets.Length == 0)
            return;

        if (MatchHeight || MatchWidth)
        {
            if (Targets.Length >= 2)
                rect = Targets.Where(a => a != null && a.gameObject.activeSelf).Select(a => a.WorldRect()).Aggregate((a, b) => Combine(a, b));
            else if (Targets.Length == 1)
                rect = Targets[0].WorldRect();

            if (canvas == null)
                canvas = GetComponentInParent<Canvas>();

            rect = GetCanvasRect(canvas, rect);
        }

        if (MatchHeight)
        {
            if (Mathf.Abs(layoutElement.preferredHeight - rect.size.y) > Mathf.Epsilon || Mathf.Abs(Self.sizeDelta.y - (rect.size.y + matchSizeOffset.y)) > Mathf.Epsilon)
            {
                layoutElement.preferredHeight = rect.size.y;
                Self.sizeDelta = new Vector2(Self.sizeDelta.x, rect.size.y + matchSizeOffset.y);
            }
        }

        if (MatchWidth)
        {
            if (Mathf.Abs(layoutElement.preferredWidth - rect.size.x) > Mathf.Epsilon || Mathf.Abs(Self.sizeDelta.x - (rect.size.x + matchSizeOffset.x)) > Mathf.Epsilon)
            {
                layoutElement.preferredWidth = rect.size.x;
                Self.sizeDelta = new Vector2(rect.size.x + matchSizeOffset.x, Self.sizeDelta.y);
            }
        }

        if (MatchPos)
        {
            Self.transform.position = rect.center + matchPosOffset;
        }
    }

    private Rect Combine(Rect rectA, Rect rectB)
    {
        //var rectA = a.WorldRect();
        //var rectB = b.WorldRect();

        var minX = Mathf.Min(rectA.min.x, rectB.min.x);
        var minY = Mathf.Min(rectA.min.y, rectB.min.y);

        var maxX = Mathf.Max(rectA.max.x, rectB.max.x);
        var maxY = Mathf.Max(rectA.max.y, rectB.max.y);

        return new Rect(minX, minY, maxX - minX, maxY - minY);
    }
}