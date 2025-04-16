using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
public class TextMeshAutoSize : MonoBehaviour
{
    public RectTransform Self;
    public TMPro.TextMeshProUGUI text;

    public bool MatchHeight = false;
    public bool MatchWidth = false;

    public float MaxWidth = 200;
    public float MaxHeight = 200;

    public void Start()
    {
        if (Self == null)
        {
            Self = GetComponent<RectTransform>();
        }

        text = GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void Update()
    {
        if (MatchWidth)
        {
            if (Self.sizeDelta.x != text.preferredWidth)
            {
                Self.sizeDelta = new Vector2(Mathf.Min(text.preferredWidth, MaxWidth), Self.sizeDelta.y);
            }
        }

        if (MatchHeight)
        {
            if (Self.sizeDelta.y != text.preferredHeight)
            {
                Self.sizeDelta = new Vector2(Self.sizeDelta.x, Mathf.Min(text.preferredHeight, MaxHeight));
            }
        }
    }
}