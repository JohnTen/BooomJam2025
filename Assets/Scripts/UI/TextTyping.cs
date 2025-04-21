using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TextTyping : MonoBehaviour
{
    TextMeshProUGUI textMeshPro;

    [Header("Parameter")]

    public float time=3f;
    public bool isScramble = true; // 是否使用打乱模式
    public string content = "系统分析中。。。" +
        "分析成功！"; // 要显示的文本内容

    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        if (textMeshPro == null)
        {
            Debug.LogError("TextMeshProUGUI component not found!");
            return;
        }
        TypingEffect(textMeshPro, content, time); // 调用 TypingEffect 方法
    }


    public void TypingEffect(TextMeshProUGUI tmp, string content, float time)
    {
        // 设置文本内容为空
        tmp.text = string.Empty;

        if (isScramble)
        {
            tmp.DOText(content, time, true, ScrambleMode.All).OnComplete(() =>
            {
                // 动画完成后的回调
                Debug.Log("Typing effect completed!");
            });
        }
        else
        {
            tmp.DOText(content, time).OnComplete(() =>
            {
                // 动画完成后的回调
                Debug.Log("Typing effect completed!");
            });
        }

    }
}
