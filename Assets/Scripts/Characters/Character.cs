using System.Collections;
using System.Collections.Generic;
using JTUtility.Event;
using UnityEngine;

public enum CharacterType
{
    None,
    Austronaut,
    Explorer,
    Worker,
}

public enum CharacterState
{
    /// <summary>
    /// 无效状态
    /// </summary>
    None,
    /// <summary>
    /// 角色活跃可用的状态
    /// </summary>
    Idle,
    /// <summary>
    /// 角色尚未解锁时的状态
    /// </summary>
    Lock,
    /// <summary>
    /// 角色处于休眠状态
    /// </summary>
    Hiber,
    /// <summary>
    /// 角色死亡状态
    /// </summary>
    Dead,
}

[RequireComponent(typeof(Draggable))]
[RequireComponent(typeof(DragDropDetector))]
public class Character : DraggableObj
{
    [SerializeField] private string characterName;
    [SerializeField] private Sprite characterPortriat;
    [SerializeField] private CharacterType characterType;
    [SerializeField] private CharacterState characterState;

    public CharacterType CharacterType => characterType;

    public CharacterState CharacterState
    {
        get => characterState;
        set
        {
            if (characterState == value)
            {
                return;
            }
            characterState = value;
            EventDispatcher<Character>.Dispatch(EventConstant.OnCharacterStateChanged, this);

            if (characterState != CharacterState.Idle)
            {
                draggable.CanDrag = false;
            }
        }
    }

    public string CharacterName => characterName;

    public override void SetSlot(ObjSlot slot, bool force = false)
    {
        if (slot != previousSlot)
        {
            EventDispatcher<Character, ObjSlot>.Dispatch(EventConstant.OnCharacterSlotChanged, this, slot);
        }
        if (slot != null)
        {
            transform.SetParent(slot.transform);
        }
        base.SetSlot(slot, force);
    }
}
