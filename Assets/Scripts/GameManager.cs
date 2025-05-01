using System;
using System.Collections;
using System.Collections.Generic;
using JTUtility;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
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
    public float screenEffectWeight;
}

public class GameManager : MonoSingleton<GameManager>
{
    [Serializable] public class CoreSlotCollection : EnumBasedCollection<CoreSlotType, ECoreSlot> {}
    [Serializable] private class CoreSlotSliderCollection : EnumBasedCollection<CoreSlotType, Slider> {}
    [Serializable] public class CorePercentCollection : EnumBasedCollection<CoreSlotType, float> {}

    [SerializeField] public CoreSlotCollection coreSlots;
    [SerializeField] private CoreSlotSliderCollection coreSlotPercentBars;
    [SerializeField] private float coreDegradeSpeed;
    [SerializeField] private float coreRestoreSpeed;
    [SerializeField] private float breakdownCheckInterval;
    [SerializeField] private float coreGreenPercent;
    [SerializeField] private float coreYellowPercent;
    [SerializeField] private float coreRedPercent;
    [SerializeField] private GameProperty coreGreenGameProperty;
    [SerializeField] private GameProperty coreYellowGameProperty;
    [SerializeField] private GameProperty coreRedGameProperty;
    [SerializeField] private GameProperty coreEmptyGameProperty;
    [SerializeField] private Volume screenEffectVolume;
    [SerializeField] private float screenEffectTransitionTime;

    [SerializeField] private OverheatWarning overheatWarning;

    public CharacterSlot defaultExplorerSlot;
    public CharacterSlot defaultWorkerSlot;

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

        if (corePercent[CoreSlotType.CoolingCore] >= coreGreenPercent)
        {
            modifiedGameProperty.breakdownChance = coreGreenGameProperty.breakdownChance;
            modifiedGameProperty.breakdownDuration = coreGreenGameProperty.breakdownDuration;
            overheatWarning.SetOverheatLevel(0);
        }
        else if (corePercent[CoreSlotType.CoolingCore] >= coreYellowPercent)
        {
            modifiedGameProperty.breakdownChance = coreYellowGameProperty.breakdownChance;
            modifiedGameProperty.breakdownDuration = coreYellowGameProperty.breakdownDuration;
            overheatWarning.SetOverheatLevel(1);
        }
        else if (corePercent[CoreSlotType.CoolingCore] > coreRedPercent)
        {
            modifiedGameProperty.breakdownChance = coreRedGameProperty.breakdownChance;
            modifiedGameProperty.breakdownDuration = coreRedGameProperty.breakdownDuration;
            overheatWarning.SetOverheatLevel(2);
        }
        else
        {
            modifiedGameProperty.breakdownChance = coreEmptyGameProperty.breakdownChance;
            modifiedGameProperty.breakdownDuration = coreEmptyGameProperty.breakdownDuration;
            overheatWarning.SetOverheatLevel(3);
        }

        if (corePercent[CoreSlotType.ControlCore] >= coreGreenPercent)
        {
            modifiedGameProperty.WarmUpSpeed = coreGreenGameProperty.WarmUpSpeed;
        }
        else if (corePercent[CoreSlotType.ControlCore] >= coreYellowPercent)
        {
            modifiedGameProperty.WarmUpSpeed = coreYellowGameProperty.WarmUpSpeed;
        }
        else if (corePercent[CoreSlotType.ControlCore] > coreRedPercent)
        {
            modifiedGameProperty.WarmUpSpeed = coreRedGameProperty.WarmUpSpeed;
        }
        else
        {
            modifiedGameProperty.WarmUpSpeed = coreEmptyGameProperty.WarmUpSpeed;
        }
        
        if (corePercent[CoreSlotType.MotionCore] >= coreGreenPercent)
        {
            modifiedGameProperty.CoreDragSpeed = coreGreenGameProperty.CoreDragSpeed;
        }
        else if (corePercent[CoreSlotType.MotionCore] >= coreYellowPercent)
        {
            modifiedGameProperty.CoreDragSpeed = coreYellowGameProperty.CoreDragSpeed;
        }
        else if (corePercent[CoreSlotType.MotionCore] > coreRedPercent)
        {
            modifiedGameProperty.CoreDragSpeed = coreRedGameProperty.CoreDragSpeed;
        }
        else
        {
            modifiedGameProperty.CoreDragSpeed = coreEmptyGameProperty.CoreDragSpeed;
        }

        var lastScreenEffectWeight = modifiedGameProperty.screenEffectWeight;
        if (corePercent[CoreSlotType.SensorCore] >= coreGreenPercent)
        {
            modifiedGameProperty.screenEffectWeight = coreGreenGameProperty.screenEffectWeight;
        }
        else if (corePercent[CoreSlotType.SensorCore] >= coreYellowPercent)
        {
            modifiedGameProperty.screenEffectWeight = coreYellowGameProperty.screenEffectWeight;
        }
        else if (corePercent[CoreSlotType.SensorCore] > coreRedPercent)
        {
            modifiedGameProperty.screenEffectWeight = coreRedGameProperty.screenEffectWeight;
        }
        else
        {
            modifiedGameProperty.screenEffectWeight = coreEmptyGameProperty.screenEffectWeight;
        }

        if (lastScreenEffectWeight != modifiedGameProperty.screenEffectWeight)
        {
            StartCoroutine(ScreenEffect(modifiedGameProperty.screenEffectWeight, screenEffectTransitionTime));
        }


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

    IEnumerator ScreenEffect(float newWeight, float duration)
    {
        float currentWeight = screenEffectVolume.weight;
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            screenEffectVolume.weight = Mathf.Lerp(currentWeight, newWeight, time / duration);
            yield return null;
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