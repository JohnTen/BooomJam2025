using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MiningNode : MonoBehaviour
{
    [SerializeField] CharacterSlot characterSlot;
    [SerializeField] ECoreSlot eCoreSlot;
    [SerializeField] ResourceSlot outputSlot;

    [SerializeField] float miningTime;
    [SerializeField] int outputAmount;

    float miningTimer;

    private void Update()
    {
        if (eCoreSlot.HasObj || 
        (characterSlot.HasObj && characterSlot.ObjInSlot is Character character && character.CharacterType != CharacterType.Worker))
        {
            eCoreSlot.gameObject.SetActive(true);
        }
        else
        {
            eCoreSlot.gameObject.SetActive(false);
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
