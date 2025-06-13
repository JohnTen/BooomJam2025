using JTUtility;
using System;
using UnityEngine;

[Serializable]
public class MultiLangStrings : EnumBasedCollection<TextDatabase.Language, string>
{
    public MultiLangStrings() : base() { }

    public MultiLangStrings(EnumBasedCollection<TextDatabase.Language, string> ebc) : base(ebc) { } 
}

[Serializable]
public class MultiLangFonts : EnumBasedCollection<TextDatabase.Language, Font> { }

[Serializable]
public class MultiLangTmpFonts : EnumBasedCollection<TextDatabase.Language, TMPro.TMP_FontAsset> { }