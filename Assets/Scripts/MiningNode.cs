using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTUtility;

public class MiningNode : MonoBehaviour
{
    [SerializeField] CharacterSlot characterSlot;
    [SerializeField] ECoreSlot eCoreSlot;
    [SerializeField] ResourceSlot outputSlot;

    [SerializeField] float miningTime;
    [SerializeField] int outputAmount;

    float miningTimer;

    DraggableObj draggable;

    private void Update()
    {
        if (eCoreSlot.HasObj || 
        (characterSlot.HasObj && characterSlot.ObjInSlot is Character character && character.CharacterType != CharacterType.Worker))
        {
            eCoreSlot.gameObject.SetActive(true);
        }
        else if (!eCoreSlot.HasObj && draggable.IsNotNull() && draggable.CurrentSlot != null)
        {
            eCoreSlot.gameObject.SetActive(false);
            draggable = null;
        }

        if (eCoreSlot.HasObj && draggable != eCoreSlot.ObjInSlot)
        {
            draggable = eCoreSlot.ObjInSlot as DraggableObj;
        }

        if (CanMine())
        {
            miningTimer += Time.deltaTime;
            if (miningTimer >= miningTime)
            {
                miningTimer -= miningTime;
                outputSlot.AddResource(outputAmount);
            }
        }
        else
        {
            miningTimer = 0;
        }
    }

    private bool CanMine()
    {
        if (!characterSlot.HasObj)
        {
            return false;
        }

        var character = characterSlot.ObjInSlot as Character;
        if (character == null)
        {
            return false;
        }

        if (character.CharacterType == CharacterType.Worker)
        {
            return true;
        }

        return eCoreSlot.HasActiveObj;
    }
}
