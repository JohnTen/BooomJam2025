using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSlot : ObjSlot
{
    public override bool CanAdd(Component obj)
    {
        return obj is Character character && !HasObj;
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
