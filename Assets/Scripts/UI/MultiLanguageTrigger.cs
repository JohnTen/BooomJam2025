using JTUtility;
using JTUtility.Event;
using System;
using UnityEngine;
using UnityEngine.Events;

public class MultiLanguageTrigger : MonoBehaviour
{
    [Serializable] private class UnityEventLang : UnityEvent<TextDatabase.Language> { }

    [Serializable] private class LangUEvents : EnumBasedCollection<TextDatabase.Language, UnityEventLang> { }

    [SerializeField] private LangUEvents events;

    private void OnEnable()
    {
        EventRegister<TextDatabase.Language>.Register(EventConstant.OnChangedLanguage, OnChangedLanguage);


        var defaultKey = TextDatabase.Language.en;
        if (events[defaultKey] == null)
            events[defaultKey] = events[TextDatabase.Language.tcn];
        if (events[defaultKey] == null)
            events[defaultKey] = events[TextDatabase.Language.scn];

        foreach (TextDatabase.Language key in System.Enum.GetValues(typeof(TextDatabase.Language)))
        {
            if (events[key] == null)
            {
                events[key] = events[defaultKey];
            }
        }

        UpdateImage(TextDatabase.Instance.CurrentLanguage);
    }

    private void OnDisable()
    {
        EventRegister<TextDatabase.Language>.UnRegister(EventConstant.OnChangedLanguage, OnChangedLanguage);
    }

    private void OnChangedLanguage(TextDatabase.Language newLanguage)
    {
        UpdateImage(newLanguage);
    }

    private void UpdateImage(TextDatabase.Language currentLanguage)
    {
        events[currentLanguage].Invoke(currentLanguage);
    }
}