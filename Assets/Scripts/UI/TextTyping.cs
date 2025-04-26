using UnityEngine;
using TMPro;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine.Events;

public class TextTyping : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;

    [Header("Parameter")]

    public float characterPerSecond = 3f;
    public bool isScramble = true; // 是否使用打乱模式
    public string content = "系统分析中";  // 要显示的文本内容

    [Header("Event")]
    public bool nextEvent = false; 
    public UnityEvent onCompleteEvent; 

    private TweenerCore<string, string, StringOptions> typingHandle;

    public bool IsTyping => typingHandle != null && typingHandle.IsPlaying();

    private GibberishText gibberishText;


    void Awake()
    {
        if (textMesh == null)
            textMesh = GetComponent<TextMeshProUGUI>();

        if (textMesh == null)
        {
            Debug.LogError("TextMeshProUGUI component not found!");
            return;
        }

        gibberishText = GetComponent<GibberishText>();
        if (gibberishText != null)
        {
            gibberishText.AutoUpdate = false;
        }
    }

    void OnEnable()
    {
        TypingEffect(content);
    }

    public void TypingEffect()
    {
        TypingEffect(content);
    }

    public void TypingEffect(string content)
    {
        float time = content.Length / characterPerSecond;
        TypingEffect(content, time);
    }

    public void TypingEffect(string content, float time)
    {
        if (gibberishText != null)
        {
            content = gibberishText.ConvertToGibberish(content);
        }

        // 设置文本内容为空
        textMesh.text = string.Empty;
        this.content = content;

        if (typingHandle != null && typingHandle.IsPlaying())
            typingHandle.Kill();

        if (isScramble)
        {
            typingHandle = textMesh.DOText(content, time, true, ScrambleMode.All).OnComplete(() =>
            {
                if (onCompleteEvent != null && nextEvent)
                {
                    onCompleteEvent.Invoke(); // 调用事件
                }
            });
        }
        else
        {
            typingHandle = textMesh.DOText(content, time).OnComplete(() =>
            {
                if (onCompleteEvent != null && nextEvent)
                {
                    onCompleteEvent.Invoke(); // 调用事件
                }
            });
        }
    }

    public void SkipTyping()
    {
        if (IsTyping)
        {
            typingHandle.Kill();
            textMesh.text = content;
        }
    }
}
