using System;
using System.Collections;
using System.Collections.Generic;
using JTUtility;
using UnityEngine;
using UnityEngine.UI;

public enum CoreSlotType
{
    MemoryCore,
    SensorCore,
    MotionCore,
    CoolingCore,
    ControlCore,
    EthicCore,
}

[Serializable]
public struct GameProperty
{
    public float WarmUpSpeed;
    public float CoreDragSpeed;
    public float breakdownChance;
    public float breakdownDuration;
}

public class GameManager : MonoSingleton<GameManager>
{
    [Serializable] private class CoreSlotCollection : EnumBasedCollection<CoreSlotType, ECoreSlot> {}
    [Serializable] private class CoreSlotSliderCollection : EnumBasedCollection<CoreSlotType, Slider> {}
    [Serializable] public class CorePercentCollection : EnumBasedCollection<CoreSlotType, float> {}

    [SerializeField] private CoreSlotCollection coreSlots;
    [SerializeField] private CoreSlotSliderCollection coreSlotPercentBars;
    [SerializeField] private float coreDegradeSpeed;
    [SerializeField] private float coreRestoreSpeed;
    [SerializeField] private float breakdownCheckInterval;
    [SerializeField] private GameProperty coreFullGameProperty;
    [SerializeField] private GameProperty coreEmptyGameProperty;

    public List<InventorySlot> inventorySlots;
    
    private float breakdownCheckTimer;

    public List<ECore> eCores;

    [SerializeField] private GameProperty modifiedGameProperty;
    public CorePercentCollection corePercent;
    public GameProperty GameProperty
    {
        get
        {
            return modifiedGameProperty;
        }
    }

    public class MainCoreCollection : EnumBasedCollection<CoreSlotType, ECoreSlot> {}
    public MainCoreCollection mainCoreCollection;

    void Awake()
    {
        mainCoreCollection = new MainCoreCollection();
        foreach (CoreSlotType coreSlotType in Enum.GetValues(typeof(CoreSlotType)))
        {
            mainCoreCollection[coreSlotType] = coreSlots[coreSlotType];
            corePercent[coreSlotType] = 1f;
        }

        eCores = new List<ECore>();
        eCores.AddRange(FindObjectsByType<ECore>(FindObjectsInactive.Include, FindObjectsSortMode.None));
    }

    void Update()
    {
        foreach (CoreSlotType mainCore in Enum.GetValues(typeof(CoreSlotType)))
        {
            if (!mainCoreCollection[mainCore].HasActiveObj)
            {
                ChangeCorePercent(mainCore, -coreDegradeSpeed * Time.deltaTime);
            }
            else
            {
                ChangeCorePercent(mainCore, coreRestoreSpeed * Time.deltaTime);
            }
        }

        modifiedGameProperty.breakdownChance = Mathf.Lerp(coreEmptyGameProperty.breakdownChance, coreFullGameProperty.breakdownChance, corePercent[CoreSlotType.CoolingCore]);
        modifiedGameProperty.breakdownDuration = Mathf.Lerp(coreEmptyGameProperty.breakdownDuration, coreFullGameProperty.breakdownDuration, corePercent[CoreSlotType.CoolingCore]);
        modifiedGameProperty.WarmUpSpeed = Mathf.Lerp(coreEmptyGameProperty.WarmUpSpeed, coreFullGameProperty.WarmUpSpeed, corePercent[CoreSlotType.ControlCore]);
        modifiedGameProperty.CoreDragSpeed = Mathf.Lerp(coreEmptyGameProperty.CoreDragSpeed, coreFullGameProperty.CoreDragSpeed, corePercent[CoreSlotType.MotionCore]);

        for (int i = 0; i < eCores.Count; i++)
        {
            if (eCores[i].IsNull() || !eCores[i].isActiveAndEnabled)
            {
                eCores.RemoveAt(i);
                i--;
            }
        }

        CheckBreakdown();
    }

    void ChangeCorePercent(CoreSlotType coreSlotType, float percent)
    {
        corePercent[coreSlotType] = Mathf.Clamp01(corePercent[coreSlotType] + percent);
        coreSlotPercentBars[coreSlotType].value = 
            Mathf.Lerp(
                coreSlotPercentBars[coreSlotType].minValue, 
                coreSlotPercentBars[coreSlotType].maxValue, 
                corePercent[coreSlotType]);
        //coreSlotPercentBars[coreSlotType].color = Color.Lerp(Color.red, Color.green, corePercent[coreSlotType]);
    }

    void CheckBreakdown()
    {
        breakdownCheckTimer += Time.deltaTime;
        if (breakdownCheckTimer >= breakdownCheckInterval)
        {
            breakdownCheckTimer = 0;
            if (UnityEngine.Random.value < GameProperty.breakdownChance)
            {
                print("Breakdown");
                eCores[UnityEngine.Random.Range(0, eCores.Count)].TriggerBreakdown();
            }
        }
    }

    public InventorySlot GetEmptyInventorySlot()
    {
        foreach (var slot in inventorySlots)
        {
            if (slot.HasObj == false)
            {
                return slot;
            }
        }

        return null;
    }

    public InventorySlot GetStackableInventorySlot(string resourceID)
    {
        foreach (var slot in inventorySlots)
        {
            if (slot.HasObj && slot.ObjInSlot is ResourceObj resourceObj && resourceObj.Template.uid == resourceID)
            {
                return slot;
            }
        }

        if (resourceID == "ECrystal")
        {
            
        }

        return null;
    }
}