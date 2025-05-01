using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTUtility;

public class MiningNode : MonoBehaviour
{
    [SerializeField] CharacterSlot characterSlot;
    [SerializeField] ECoreSlot eCoreSlot;
    [SerializeField] ResourceSlot outputSlot;
    [SerializeField] CharacterSlot accCharacterSlot;

    [SerializeField] float miningTime;
    [SerializeField] int outputAmount;
    [SerializeField] float accMiningTime;
    [SerializeField] int accOutputAmount;

    float miningTimer;

    DraggableObj draggable;

    private void Update()
    {
        if (characterSlot.HasObj && characterSlot.ObjInSlot is Character character && character.CharacterType != CharacterType.Worker)
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
            var time = accCharacterSlot.HasObj ? accMiningTime : miningTime;
            var output = accCharacterSlot.HasObj ? accOutputAmount : outputAmount;
            if (miningTimer >= time)
            {
                miningTimer -= time;
                outputSlot.AddResource(output);
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
