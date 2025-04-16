using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISizeController : MonoBehaviour
{
    public RectTransform parent;
    public RectTransform[] childs;
    public bool ignoreWidth;
    public bool ignoreHeight;
    public bool isTextChild;

    //取子物体最大的适配
    public bool controlByOne;

    private Text[] txtRect;
    private Vector2 parentSize;
    private Vector2[] childsSize;

    private TextMeshProUGUI[] _tmps;

    private void InitText()
    {
        _tmps = new TextMeshProUGUI[childs.Length];

        txtRect = new Text[childs.Length];

        for (int i = 0; i < childs.Length; i++)
        {
            txtRect[i] = childs[i].GetComponent<Text>();
            _tmps[i] = childs[i].GetComponent<TextMeshProUGUI>();
        }
    }

    private Vector2 GetTextRect(int idx)
    {
        Vector2 result;

        if (txtRect[idx] != null)
        {
            result = new Vector2(txtRect[idx].preferredWidth, txtRect[idx].preferredHeight);
        }
        else
        {
            result = new Vector2(_tmps[idx].preferredWidth, _tmps[idx].preferredHeight);
        }

        return result;
    }

    private void Start()
    {
        parentSize = parent.sizeDelta;
        childsSize = new Vector2[childs.Length];

        for (int i = 0; i < childs.Length; i++)
        {
            childsSize[i] = childs[i].sizeDelta;
        }
        if (isTextChild)
        {
            InitText();
        }
    }

    private void Update()
    {
        Vector2 totalOffsetSize = Vector2.zero;
        for (int i = 0; i < childs.Length; i++)
        {
            if (childs[i].gameObject.activeSelf)
            {
                Vector2 perSize = childs[i].sizeDelta;
                if (isTextChild)
                    perSize = GetTextRect(i);

                Vector2 offsetSize = perSize - childsSize[i];
                if (controlByOne)
                {
                    totalOffsetSize.x = offsetSize.x > totalOffsetSize.x ? offsetSize.x : totalOffsetSize.x;
                    totalOffsetSize.y = offsetSize.y > totalOffsetSize.y ? offsetSize.y : totalOffsetSize.y;
                }
                else
                    totalOffsetSize += offsetSize;
            }
        }

        if (ignoreWidth)
            totalOffsetSize.x = 0;
        if (ignoreHeight)
            totalOffsetSize.y = 0;
        parent.sizeDelta = parentSize + totalOffsetSize;
    }
}