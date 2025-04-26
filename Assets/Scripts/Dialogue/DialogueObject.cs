using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueObject : MonoBehaviour
{
    [SerializeField] private Image portrait;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private TextTyping textTyping;

    [SerializeField] private Pop_upTween popUpTween;

    public DialogueEntry entry;

    public bool isTyping => textTyping.IsTyping;

    public void Init(DialogueEntry entry)
    {
        this.entry = entry;
        var portraitSprite = PortraitHub.GetPortrait(entry.portriat);
        if (portraitSprite != null)
        {
            portrait.sprite = portraitSprite;
        }
        
        nameText.text = entry.actorName;
        text.text = entry.text;
        if (entry.useTyping)
        {
            textTyping.isScramble = entry.useScramble;
            float time = entry.text.Length / (textTyping.characterPerSecond * entry.typingSpeed);
            textTyping.TypingEffect(entry.text, time);
        }
        else
        {
            text.text = entry.text;
        }
    }

    public void SkipTyping()
    {
        if (entry.useTyping)
        {
            textTyping.SkipTyping();
        }
    }
}
