using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTUtility;

[CreateAssetMenu(fileName = "PortraitHub", menuName = "Dialogue/PortraitHub")]
public class PortraitHub : ScriptableObject
{
    [System.Serializable]
    public class StrSpritePair : PairedValue<string, Sprite> {}

    public List<StrSpritePair> portraits;

    private static PortraitHub instance;
    public static PortraitHub Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<PortraitHub>("PortraitHub");
            }

            return instance;
        }
    }

    void OnEnable()
    {
        instance = this;
    }

    public static Sprite GetPortrait(string name)
    {
        var portrait = instance.portraits.Find(pair => pair.Key == name)?.Value;
        if (portrait == null)
        {
            Debug.LogWarning($"Portrait {name} not found");
        }
        return portrait;
    }
}
