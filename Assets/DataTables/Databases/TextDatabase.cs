using JTUtility;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TextModel : ICloneable
{
    public string id;
    public MultiLangStrings lnStrings;

    public TextModel(string id, MultiLangStrings multiLangStr)
    {
        this.id = id;
        this.lnStrings = new MultiLangStrings(multiLangStr);
    }

    public object Clone()
    {
        return new TextModel(id, lnStrings);
    }
}

[CreateAssetMenu(fileName = "TextDatabase", menuName = "NewDatabase/text")]
public class TextDatabase : GameData.NewDatabase<TextDatabase, TextModel, TextTableData>
{
    public enum Language
    {
        scn,
        tcn,
        en,
        jp,
        kr,
        de,
        fr,
        es,
        pt,
        ru
    }

    public Language CurrentLanguage { get; set; } = Language.scn;

    public MultiLangFonts defaultFonts = new MultiLangFonts();
    public MultiLangTmpFonts defaultTmpFonts = new MultiLangTmpFonts();

    public override void InitDatabase()
    {
        if (itemDict != null)
        {
            return;
        }

        itemDict = new Dictionary<string, TextModel>();

        var datatable = dataTable as TextTable;
        if (datatable == null)
        {
            Debug.LogError("TextDatabase requires TextTable asset to work");
            return;
        }

        foreach (var item in datatable.dataArray)
        {
            try
            {
                var toItem = ToItem(item);
                if (toItem != null)
                    itemDict.Add(item.ID, toItem);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }

    public override TextModel ToItem(TextTableData data)
    {
        if (string.IsNullOrEmpty(data.ID))
            return null;

        MultiLangStrings mls = new MultiLangStrings();

        mls[Language.scn] = data.Scn;
        mls[Language.tcn] = data.Tcn;
        mls[Language.en] = data.En;
        mls[Language.jp] = data.Jp;
        mls[Language.kr] = data.Kr;
        mls[Language.de] = data.De;
        mls[Language.fr] = data.Fr;
        mls[Language.es] = data.Es;
        mls[Language.pt] = data.Pt;
        mls[Language.ru] = data.Ru;

        return new TextModel(data.ID, mls);
    }

    public string GetLNItem(string id)
    {
        var item = GetItem(id);
        if (item == null)
            return string.Empty;

        string text = item.lnStrings[CurrentLanguage];
        if (string.IsNullOrEmpty(text) && CurrentLanguage != Language.en)
        {
            Debug.LogWarning($"No text for {id} with {Enum.GetName(typeof(Language), CurrentLanguage)}, fallback to en.");
            text = item.lnStrings[Language.en];
        }

        if (string.IsNullOrEmpty(text) && CurrentLanguage != Language.scn)
        {
            Debug.LogWarning($"No text for {id} with {Enum.GetName(typeof(Language), CurrentLanguage)}, fallback to scn.");
            text = item.lnStrings[Language.scn];
        }

        return text;
    }

    public bool TryGetLNItem(string id, out string text)
    {
        var item = GetItem(id);
        if (item == null)
        {
            text = string.Empty;
            return false;
        }

        text = item.lnStrings[CurrentLanguage];
        if (string.IsNullOrEmpty(text) && CurrentLanguage != Language.en)
        {
            Debug.LogWarning($"No text for {id} with {Enum.GetName(typeof(Language), CurrentLanguage)}, fallback to en.");
            text = item.lnStrings[Language.en];
        }

        if (string.IsNullOrEmpty(text) && CurrentLanguage != Language.scn)
        {
            Debug.LogWarning($"No text for {id} with {Enum.GetName(typeof(Language), CurrentLanguage)}, fallback to scn.");
            text = item.lnStrings[Language.scn];
        }

        return true;
    }
}