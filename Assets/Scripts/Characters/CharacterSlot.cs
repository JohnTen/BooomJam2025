using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSlot : ObjSlot
{
    [SerializeField] private List<CharacterType> characterTypes;
    [SerializeField] private List<CharacterState> characterState;

    public override bool CanAdd(Component obj)
    {
        var character = obj as Character;
        if (character == null || HasObj)
        {
            return false;
        }
        if (characterTypes.Count > 0 && !characterTypes.Contains(character.CharacterType))
        {
            return false;
        }
        if (characterState.Count > 0 && !characterState.Contains(character.CharacterState))
        {
            return false;
        }
        
        return true;
    }

    public override bool CanRemove(Component obj)
    {
        return obj == ObjInSlot;
    }

    public override void OnObjEnter(Component obj)
    {
        
    }

    public override void OnObjExit(Component obj)
    {
        
    }
}
