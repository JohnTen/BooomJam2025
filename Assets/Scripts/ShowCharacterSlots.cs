using System.Collections;
using System.Collections.Generic;
using JTUtility.Event;
using UnityEngine;

public class ShowCharacterSlots : MonoBehaviour
{
    [SerializeField] List<GameObject> characterSlotsGO;
    [SerializeField] List<CharacterSlot> characterSlots;

    private void OnEnable()
    {
        EventRegister<DraggableObj>.Register(EventConstant.OnDragStart, OnDragStart);
        EventRegister<DraggableObj>.Register(EventConstant.OnDragEnd, OnDragEnd);
    }

    private void OnDisable()
    {
        EventRegister<DraggableObj>.UnRegister(EventConstant.OnDragStart, OnDragStart);
        EventRegister<DraggableObj>.UnRegister(EventConstant.OnDragEnd, OnDragEnd);
    }

    private void Start()
    {
        for (int i = 0; i < characterSlots.Count; i++)
        {
            if (characterSlotsGO.Count <= i)
            {
                characterSlotsGO.Add(characterSlots[i].gameObject);
            }
            else if (characterSlotsGO[i] == null)
            {
                characterSlotsGO[i] = characterSlots[i].gameObject;
            }
        }
    }

    private void OnDragStart(DraggableObj obj)
    {
        if (obj is Character character && character.CharacterType == CharacterType.Austronaut)
        {
            for (int i = 0; i < characterSlotsGO.Count; i++)
            {
                characterSlotsGO[i].SetActive(true);
            }
        }
    }

    private void OnDragEnd(DraggableObj obj)
    {
        for (int i = 0; i < characterSlotsGO.Count; i++)
        {
            if (!characterSlots[i].HasObj)
            {
                characterSlotsGO[i].SetActive(false);
            }
        }
    }
    
}
