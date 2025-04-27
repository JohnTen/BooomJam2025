using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExploreNode : MonoBehaviour
{
    [SerializeField] CharacterSlot characterSlot;
    [SerializeField] ECoreSlot eCoreSlot;

    [SerializeField] UnityEvent onExploreReady;

    bool isExploreReady = false;

    void Update()
    {
        if (characterSlot.HasObj)
        {
            eCoreSlot.gameObject.SetActive(true);
        }
        else if (!eCoreSlot.HasObj)
        {
            eCoreSlot.gameObject.SetActive(false);
        }

        if (!isExploreReady && characterSlot.HasObj && eCoreSlot.HasObj)
        {
            isExploreReady = true;
            onExploreReady.Invoke();
        }
        else if (isExploreReady && (!characterSlot.HasObj || !eCoreSlot.HasObj))
        {
            isExploreReady = false;
        }
    }

    
}
