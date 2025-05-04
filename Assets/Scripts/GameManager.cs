using System;
using System.Collections;
using System.Collections.Generic;
using JTUtility;
using JTUtility.Event;
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
    public float memoryDegrade;
    public float breakdownChance;
    public float breakdownDuration;
    public float screenEffectWeight;
}

public class GameManager : MonoSingleton<GameManager>
{
    [Serializable] public class CoreSlotCollection : EnumBasedCollection<CoreSlotType, ECoreSlot> {}
    [Serializable] private class CoreSlotSliderCollection : EnumBasedCollection<CoreSlotType, Slider> {}
    [Serializable] public class CorePercentCollection : EnumBasedCollection<CoreSlotType, float> {}
    [Serializable] public class CoreStageCollection : EnumBasedCollection<CoreSlotType, int> {}

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

    public CoreStageCollection coreStage;

    public class MainCoreCollection : EnumBasedCollection<CoreSlotType, ECoreSlot> {}
    public MainCoreCollection mainCoreCollection;

    bool gameEnd = false;

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
        coreStage = new CoreStageCollection();
        foreach (CoreSlotType coreSlotType in Enum.GetValues(typeof(CoreSlotType)))
        {
            coreStage[coreSlotType] = 0;
        }
    }

    void Update()
    {
        foreach (CoreSlotType mainCore in Enum.GetValues(typeof(CoreSlotType)))
        {
            if (!mainCoreCollection[mainCore].HasActiveObj && !gameEnd)
            {
                ChangeCorePercent(mainCore, -coreDegradeSpeed * Time.deltaTime);
            }
            else
            {
                ChangeCorePercent(mainCore, coreRestoreSpeed * Time.deltaTime);
            }
        }

        var lastCoreStage = coreStage[CoreSlotType.CoolingCore];
        if (corePercent[CoreSlotType.CoolingCore] >= coreGreenPercent)
        {
            modifiedGameProperty.breakdownChance = coreGreenGameProperty.breakdownChance;
            modifiedGameProperty.breakdownDuration = coreGreenGameProperty.breakdownDuration;
            overheatWarning.SetOverheatLevel(0);
            coreStage[CoreSlotType.CoolingCore] = 0;
        }
        else if (corePercent[CoreSlotType.CoolingCore] >= coreYellowPercent)
        {
            modifiedGameProperty.breakdownChance = coreYellowGameProperty.breakdownChance;
            modifiedGameProperty.breakdownDuration = coreYellowGameProperty.breakdownDuration;
            overheatWarning.SetOverheatLevel(1);
            coreStage[CoreSlotType.CoolingCore] = 1;
        }
        else if (corePercent[CoreSlotType.CoolingCore] > coreRedPercent)
        {
            modifiedGameProperty.breakdownChance = coreRedGameProperty.breakdownChance;
            modifiedGameProperty.breakdownDuration = coreRedGameProperty.breakdownDuration;
            overheatWarning.SetOverheatLevel(2);
            coreStage[CoreSlotType.CoolingCore] = 2;
        }
        else
        {
            modifiedGameProperty.breakdownChance = coreEmptyGameProperty.breakdownChance;
            modifiedGameProperty.breakdownDuration = coreEmptyGameProperty.breakdownDuration;
            overheatWarning.SetOverheatLevel(3);
            coreStage[CoreSlotType.CoolingCore] = 3;
        }

        if (lastCoreStage != coreStage[CoreSlotType.CoolingCore])
        {
            EventDispatcher<CoreSlotType, int>.Dispatch(EventConstant.CoreStageChanged, CoreSlotType.CoolingCore, coreStage[CoreSlotType.CoolingCore]);
        }

        lastCoreStage = coreStage[CoreSlotType.ControlCore];
        if (corePercent[CoreSlotType.ControlCore] >= coreGreenPercent)
        {
            modifiedGameProperty.WarmUpSpeed = coreGreenGameProperty.WarmUpSpeed;
            coreStage[CoreSlotType.ControlCore] = 0;
        }
        else if (corePercent[CoreSlotType.ControlCore] >= coreYellowPercent)
        {
            modifiedGameProperty.WarmUpSpeed = coreYellowGameProperty.WarmUpSpeed;
            coreStage[CoreSlotType.ControlCore] = 1;
        }
        else if (corePercent[CoreSlotType.ControlCore] > coreRedPercent)
        {
            modifiedGameProperty.WarmUpSpeed = coreRedGameProperty.WarmUpSpeed;
            coreStage[CoreSlotType.ControlCore] = 2;
        }
        else
        {
            modifiedGameProperty.WarmUpSpeed = coreEmptyGameProperty.WarmUpSpeed;
            coreStage[CoreSlotType.ControlCore] = 3;
        }

        if (lastCoreStage != coreStage[CoreSlotType.ControlCore])
        {
            EventDispatcher<CoreSlotType, int>.Dispatch(EventConstant.CoreStageChanged, CoreSlotType.ControlCore, coreStage[CoreSlotType.ControlCore]);
        }

        lastCoreStage = coreStage[CoreSlotType.MotionCore];
        if (corePercent[CoreSlotType.MotionCore] >= coreGreenPercent)
        {
            modifiedGameProperty.CoreDragSpeed = coreGreenGameProperty.CoreDragSpeed;
            coreStage[CoreSlotType.MotionCore] = 0;
        }
        else if (corePercent[CoreSlotType.MotionCore] >= coreYellowPercent)
        {
            modifiedGameProperty.CoreDragSpeed = coreYellowGameProperty.CoreDragSpeed;
            coreStage[CoreSlotType.MotionCore] = 1;
        }
        else if (corePercent[CoreSlotType.MotionCore] > coreRedPercent)
        {
            modifiedGameProperty.CoreDragSpeed = coreRedGameProperty.CoreDragSpeed;
            coreStage[CoreSlotType.MotionCore] = 2;
        }
        else
        {
            modifiedGameProperty.CoreDragSpeed = coreEmptyGameProperty.CoreDragSpeed;
            coreStage[CoreSlotType.MotionCore] = 3;
        }

        if (lastCoreStage != coreStage[CoreSlotType.MotionCore])
        {
            EventDispatcher<CoreSlotType, int>.Dispatch(EventConstant.CoreStageChanged, CoreSlotType.MotionCore, coreStage[CoreSlotType.MotionCore]);
        }

        lastCoreStage = coreStage[CoreSlotType.SensorCore];
        if (corePercent[CoreSlotType.SensorCore] >= coreGreenPercent)
        {
            modifiedGameProperty.screenEffectWeight = coreGreenGameProperty.screenEffectWeight;
            coreStage[CoreSlotType.SensorCore] = 0;
        }
        else if (corePercent[CoreSlotType.SensorCore] >= coreYellowPercent)
        {
            modifiedGameProperty.screenEffectWeight = coreYellowGameProperty.screenEffectWeight;
            coreStage[CoreSlotType.SensorCore] = 1;
        }
        else if (corePercent[CoreSlotType.SensorCore] > coreRedPercent)
        {
            modifiedGameProperty.screenEffectWeight = coreRedGameProperty.screenEffectWeight;
            coreStage[CoreSlotType.SensorCore] = 2;
        }
        else
        {
            modifiedGameProperty.screenEffectWeight = coreEmptyGameProperty.screenEffectWeight;
            coreStage[CoreSlotType.SensorCore] = 3;
        }

        if (lastCoreStage != coreStage[CoreSlotType.SensorCore])
        {
            EventDispatcher<CoreSlotType, int>.Dispatch(EventConstant.CoreStageChanged, CoreSlotType.SensorCore, coreStage[CoreSlotType.SensorCore]);
            StartCoroutine(ScreenEffect(modifiedGameProperty.screenEffectWeight, screenEffectTransitionTime));
        }

        lastCoreStage = coreStage[CoreSlotType.MemoryCore];
        if (corePercent[CoreSlotType.MemoryCore] >= coreGreenPercent)
        {
            coreStage[CoreSlotType.MemoryCore] = 0;
            modifiedGameProperty.memoryDegrade = coreGreenGameProperty.memoryDegrade;
        }
        else if (corePercent[CoreSlotType.MemoryCore] >= coreYellowPercent)
        {
            coreStage[CoreSlotType.MemoryCore] = 1;
            modifiedGameProperty.memoryDegrade = coreYellowGameProperty.memoryDegrade;
        }
        else if (corePercent[CoreSlotType.MemoryCore] > coreRedPercent)
        {
            coreStage[CoreSlotType.MemoryCore] = 2;
            modifiedGameProperty.memoryDegrade = coreRedGameProperty.memoryDegrade;
        }
        else
        {
            coreStage[CoreSlotType.MemoryCore] = 3;
            modifiedGameProperty.memoryDegrade = coreEmptyGameProperty.memoryDegrade;
        }

        if (lastCoreStage != coreStage[CoreSlotType.MemoryCore])
        {
            EventDispatcher<CoreSlotType, int>.Dispatch(EventConstant.CoreStageChanged, CoreSlotType.MemoryCore, coreStage[CoreSlotType.MemoryCore]);
        }

        lastCoreStage = coreStage[CoreSlotType.EthicCore];
        if (corePercent[CoreSlotType.EthicCore] >= coreGreenPercent)
        {
            coreStage[CoreSlotType.EthicCore] = 0;
        }
        else if (corePercent[CoreSlotType.EthicCore] >= coreYellowPercent)
        {
            coreStage[CoreSlotType.EthicCore] = 1;
        }
        else if (corePercent[CoreSlotType.EthicCore] > coreRedPercent)
        {
            coreStage[CoreSlotType.EthicCore] = 2;
        }
        else
        {
            coreStage[CoreSlotType.EthicCore] = 3;
        }

        if (lastCoreStage != coreStage[CoreSlotType.EthicCore])
        {
            EventDispatcher<CoreSlotType, int>.Dispatch(EventConstant.CoreStageChanged, CoreSlotType.EthicCore, coreStage[CoreSlotType.EthicCore]);
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
            Mathf.CeilToInt(
                Mathf.Lerp(
                    coreSlotPercentBars[coreSlotType].minValue, 
                    coreSlotPercentBars[coreSlotType].maxValue, 
                    corePercent[coreSlotType]));
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
        if (resourceID == "ECrystal")
        {
            return null;
        }


        foreach (var slot in inventorySlots)
        {
            if (slot.HasObj && slot.ObjInSlot is ResourceObj resourceObj && resourceObj.Template.uid == resourceID)
            {
                return slot;
            }
        }
        
        return null;
    }

    public void GameEnd()
    {
        if (gameEnd)
        {
            return;
        }

        gameEnd = true;
        foreach (CoreSlotType coretype in Enum.GetValues(typeof(CoreSlotType)))
        {
            corePercent[coretype] = 1;
        }
    }
}