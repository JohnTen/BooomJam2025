using JTUtility;
using JTUtility.Event;
using System;
using UnityEngine;

public class MultiLanguageGOGroup : MonoBehaviour
{
    [Serializable] private class LangUEvents : EnumBasedCollection<TextDatabase.Language, GameObject> { }

    [SerializeField] private LangUEvents group;

    private void OnEnable()
    {
        EventRegister<TextDatabase.Language>.Register(EventConstant.OnChangedLanguage, OnChangedLanguage);

        var defaultKey = TextDatabase.Language.en;
        if (group[defaultKey] == null)
            group[defaultKey] = group[TextDatabase.Language.tcn];
        if (group[defaultKey] == null)
            group[defaultKey] = group[TextDatabase.Language.scn];

        foreach (TextDatabase.Language key in System.Enum.GetValues(typeof(TextDatabase.Language)))
        {
            if (group[key] == null)
            {
                group[key] = group[defaultKey];
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
        foreach (TextDatabase.Language value in Enum.GetValues(typeof(TextDatabase.Language)))
        {
            if (group[value].activeSelf)
                group[value].SetActive(false);
        }

        foreach (TextDatabase.Language value in Enum.GetValues(typeof(TextDatabase.Language)))
        {
            if (value == currentLanguage && !group[value].activeSelf)
            {
                group[value].SetActive(true);
            }
        }
    }
}