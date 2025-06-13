using JTUtility;
using JTUtility.Event;
using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MultiLanguageSprite : MonoBehaviour
{
    [Serializable] private class LangSprite : EnumBasedCollection<TextDatabase.Language, Sprite> { }

    [SerializeField] private LangSprite sprites;

    private SpriteRenderer image;

    private void OnEnable()
    {
        image = GetComponent<SpriteRenderer>();
        EventRegister<TextDatabase.Language>.Register(EventConstant.OnChangedLanguage, OnChangedLanguage);

        var defaultKey = TextDatabase.Language.en;
        if (sprites[defaultKey] == null)
            sprites[defaultKey] = sprites[TextDatabase.Language.tcn];
        if (sprites[defaultKey] == null)
            sprites[defaultKey] = sprites[TextDatabase.Language.scn];

        foreach (TextDatabase.Language key in System.Enum.GetValues(typeof(TextDatabase.Language)))
        {
            if (sprites[key] == null)
            {
                sprites[key] = sprites[defaultKey];
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
        image.sprite = sprites[currentLanguage];
    }
}