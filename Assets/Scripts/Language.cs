using JTUtility.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Language : MonoBehaviour
{
    [SerializeField] string defaultLanguage = "en";

    private void Start()
    {
        SetLanguage(defaultLanguage);
    }

    public void SetLanguage(string language)
    {
        TextDatabase.Instance.CurrentLanguage = System.Enum.Parse<TextDatabase.Language>(language);
        EventDispatcher<TextDatabase.Language>.Dispatch(EventConstant.OnChangedLanguage, TextDatabase.Instance.CurrentLanguage);
    }
}
