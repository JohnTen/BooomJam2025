using JTUtility;
using JTUtility.Event;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class MultiLanguageText : MonoBehaviour
{
    [SerializeField] private string _textID;
    [SerializeField] private Text legacyText;
    [SerializeField] private TMPro.TMP_Text tmpText;
    [SerializeField] private MultiLangFonts fonts;
    [SerializeField] private MultiLangTmpFonts tmpFonts;

    private TextDatabase textDatabase;

    private List<Func<string, string>> textPostProcessors = new List<Func<string, string>>();

    public string textID
    {
        get => _textID;
        set
        {
            _textID = value;
            UpdateText();
        }
    }

    public string text
    {
        get
        {
            if (legacyText.IsNotNull())
            {
                return legacyText.text;
            }
            if (tmpText.IsNotNull())
            {
                return tmpText.text;
            }
            return string.Empty;
        }
        set
        {
            if (legacyText.IsNotNull())
            {
                legacyText.text = value;
            }
            if (tmpText.IsNotNull())
            {
                tmpText.text = value;
            }
        }
    }

    private void OnEnable()
    {
        EventRegister<TextDatabase.Language>.Register(EventConstant.OnChangedLanguage, OnChangedLanguage);

        if (textDatabase.IsNull())
        {
            textDatabase = TextDatabase.Instance;
        }

        foreach (TextDatabase.Language lang in System.Enum.GetValues(typeof(TextDatabase.Language)))
        {
            if (fonts[lang] == null)
                fonts[lang] = textDatabase.defaultFonts[lang];
            if (tmpFonts[lang] == null)
                tmpFonts[lang] = textDatabase.defaultTmpFonts[lang];
        }

        UpdateText();
    }

    private void OnDisable()
    {
        EventRegister<TextDatabase.Language>.UnRegister(EventConstant.OnChangedLanguage, OnChangedLanguage);
    }

    public void RegisterPostProcesser(Func<string, string> postProcessor)
    {
        textPostProcessors.Add(postProcessor);
    }

    private void OnChangedLanguage(TextDatabase.Language newLanguage)
    {
        UpdateText();
    }

    public void UpdateText()
    {
        if (textDatabase.IsNull())
        {
            textDatabase = TextDatabase.Instance;
        }

        if (legacyText.IsNull() && tmpText.IsNull())
        {
            legacyText = GetComponent<Text>();
            tmpText = GetComponent<TMPro.TMP_Text>();
        }

        if (legacyText.IsNull() && tmpText.IsNull())
        {
            Debug.LogWarning($"No text or TMPro.TMP_Text found at this GameObject({gameObject.name})!");
            return;
        }

        if (!textDatabase.ContainsID(_textID))
        {
            Debug.LogWarning("Invalid textID \"" + _textID + "\"! is set on " + gameObject.name);
            return;
        }

        var atext = textDatabase.GetLNItem(_textID);
        if (!textPostProcessors.IsNullOrEmpty())
        {
            foreach (var processor in textPostProcessors)
            {
                atext = processor(atext);
            }
        }

        if (legacyText.IsNotNull())
        {
            legacyText.text = atext;
            legacyText.font = fonts[textDatabase.CurrentLanguage];
        }
        if (tmpText.IsNotNull())
        {
            tmpText.text = atext;
            tmpText.font = tmpFonts[textDatabase.CurrentLanguage];
        }
    }
}