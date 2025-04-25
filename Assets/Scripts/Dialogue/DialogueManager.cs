using Coffee.UIExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using JTUtility;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.UI;
using Unity.VisualScripting;
using DG.Tweening;
using DG.Tweening.Core;
using JTUtility.Event;

public class DialogueManager : MonoSingleton<DialogueManager>
{
    [Serializable] class StrGOPair : PairedValue<string, GameObject> {}

    [SerializeField] private GameObject visualParent;
    [SerializeField] private RectTransform contentParent;
    [SerializeField] private Image visualMask;
    [SerializeField] private RectTransform unmaskPrefab;

    [Header("Dialogue Object")]
    [SerializeField] private DialogueObject dialogueObjectPrefab;

    [SerializeField] private List<StrGOPair> gadgets;

    public bool IsPlaying => currentEntry != null;

    private Transform hideParent;

    private List<DialogueEntry> entries;

    private DialogueEntry currentEntry;
    private DialogueObject currentObject;
    private GameObject currentGadgetObject;

    private bool IsDialogueObject => currentObject != null;

    private List<RectTransform> unmasks = new List<RectTransform>();
    private List<UnmaskRaycastFilter> raycastFilters = new List<UnmaskRaycastFilter>();

    private List<DialogueObject> dialogueObjects = new List<DialogueObject>();

    private List<GameObject> gadgetObjectObjects = new List<GameObject>();

    private float delayTimer;

    private GameObject visualMaskGO;

    private Vector3 visualParentOriginalPosition;

    private TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> visualParentTween;

    protected override void OnInit()
    {
        visualMaskGO = visualMask.gameObject;
        visualParentOriginalPosition = visualParent.transform.localPosition;
        visualParent.transform.localPosition = -visualParentOriginalPosition;
    }

    void OnEnable()
    {
        EventRegister<string>.Register(EventConstant.ResourceRequirementMet, OnResourceRequirementMet);
        EventRegister<Character>.Register(EventConstant.OnCharacterStateChanged, OnCharacterStateChanged);
        EventRegister<Character, ObjSlot>.Register(EventConstant.OnCharacterSlotChanged, OnCharacterSlotChanged);
    }

    void OnDisable()
    {
        EventRegister<string>.UnRegister(EventConstant.ResourceRequirementMet, OnResourceRequirementMet);
        EventRegister<Character>.UnRegister(EventConstant.OnCharacterStateChanged, OnCharacterStateChanged);
        EventRegister<Character, ObjSlot>.UnRegister(EventConstant.OnCharacterSlotChanged, OnCharacterSlotChanged);
    }

    private void Start()
    {
        entries = DialogueEntry.GenerateDialogueEntries();
    }

    private void Update()
    {
        UpdateEntries(-1, null);
        SortDialogueObjects();
    }

    public void PlayDialogue(string dialogueID)
    {
        var entry = entries.Find(e => e.instructionID == dialogueID);
        if (entry != null)
        {
            currentEntry = entry;
            StartDialogue();
            BuildDialogueObject();
        }
    }

    private void UpdateEntries(int eventID, object[] args)
    {
        if (currentEntry != null)
        {
            UpdateCurrentEntry(eventID, args);
            return;
        }

        foreach (var entry in entries)
        {
            if (entry.done)
                continue;

            if (entry.oneTimeOnly && entry.playedTimes > 0)
                continue;

            if (!entry.condition(eventID, entry, args))
                continue;

            if (currentEntry != null)
            {
                Debug.LogWarning("Unfinished entry :" + currentEntry.instructionID);
            }

            currentEntry = entry;
            StartDialogue();
            BuildDialogueObject();
            break;
        }
    }

    private void SortDialogueObjects()
    {
        if (contentParent.sizeDelta.y > 0)
        {
            contentParent.anchoredPosition = new Vector2(0, contentParent.sizeDelta.y);
        }
        else
        {
            contentParent.anchoredPosition = new Vector2(0, 0);
        }
    }

    private void UpdateCurrentEntry(int eventID, object[] args)
    {
        if (currentEntry.done)
        {
            return;
        }
        
        if (currentEntry.inited == false)
        {
            try
            {
                currentEntry.onDialogueEntryExecInit?.Invoke(currentEntry);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        if (IsDialogueObject && currentObject.isTyping)
        {
            if (Input.GetMouseButtonDown(0))
            {
                currentObject.SkipTyping();
                return;
            }
        }

        if (currentEntry.delay > delayTimer)
        {
            if (!IsDialogueObject || !currentObject.isTyping)
            {
                delayTimer += Time.unscaledDeltaTime;
            }
            return;
        }

        if (!currentEntry.HasCondition)
        {
            currentEntry.started = true;
        }
        else if (currentEntry.waitForCondition(eventID, currentEntry, args))
        {
            currentEntry.started = true;
        }

        if (currentEntry.started == false)
        {
            return;
        }

        switch (currentEntry.type)
        {
            case DialogueEntryType.Pass:
                FinishCurrentEntry();
                break;
            case DialogueEntryType.ClickAnywhere:
                if (Input.GetMouseButtonDown(0))
                {
                    FinishCurrentEntry();
                }
                break;
            case DialogueEntryType.ClickMaskArea:
                if (!Input.GetMouseButtonDown(0))
                {
                    return;
                }
                
                foreach (var mask in currentEntry.unmasks)
                {
                    if (RectTransformUtility.RectangleContainsScreenPoint(mask, VirtualCursor.ScreenPosition, Camera.main))
                    {
                        FinishCurrentEntry();
                        break;
                    }
                }
                break;
        }
    }

    private void FinishCurrentEntry()
    {
        currentEntry.done = true;
        try
        {
            currentEntry.onDialogueEntryExecEnd?.Invoke(currentEntry);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        currentEntry.playedTimes++;
        if (string.IsNullOrEmpty(currentEntry.nextEntry))
        {
            currentEntry = null;
            FinishDialogue();
            return;
        }

        currentEntry = entries.Find(e => e.instructionID == currentEntry.nextEntry);
        BuildDialogueObject();
    }

    private void StartDialogue()
    {
        if (visualParentTween != null && visualParentTween.IsPlaying())
            visualParentTween.Kill();
        
        visualParent.transform.localPosition = visualParentOriginalPosition.AlterX(-visualParentOriginalPosition.x);
        visualParentTween = visualParent.transform.DOLocalMoveX(visualParentOriginalPosition.x, 0.5f).SetEase(Ease.OutCubic);
    }

    private void FinishDialogue()
    {
        if (visualParentTween != null && visualParentTween.IsPlaying())
            visualParentTween.Kill();

        visualParent.transform.localPosition = visualParentOriginalPosition;
        visualParentTween = visualParent.transform.DOLocalMoveX(-visualParentOriginalPosition.x, 0.5f).SetEase(Ease.InCubic);
        visualParentTween.onComplete = () =>
        {
            foreach (var dialogueObject in dialogueObjects)
            {
                dialogueObject.gameObject.SetActive(false);
            }
            foreach (var gadgetObject in gadgetObjectObjects)
            {
                gadgetObject.SetActive(false);
            }
        };
    }

    private void BuildDialogueObject()
    {
        if (!string.IsNullOrEmpty(currentEntry.gadget))
        {
            var gadget = gadgets.Find(g => g.Key == currentEntry.gadget);
            if (gadget != null)
            {
                currentGadgetObject = Instantiate(gadget.Value, contentParent);
                gadgetObjectObjects.Add(currentGadgetObject);
            }
            else
            {
                Debug.LogError("Gadget not found: " + currentEntry.gadget);
            }
        }
        else
        {
            currentObject = Instantiate(dialogueObjectPrefab, contentParent);
            currentObject.Init(currentEntry);
            dialogueObjects.Add(currentObject);
        }

        print("BuildDialogueObject: " + currentEntry.instructionID);
        currentEntry.inited = false;
        currentEntry.started = false;
        currentEntry.done = false;
        currentEntry.onDialogueEntryExecInit += OnEntryExecInit;
        currentEntry.onDialogueEntryExecStart += OnEntryExecStart;
        currentEntry.onDialogueEntryExecEnd += OnEntryExecEnd;

        if (!currentEntry.masks.IsNullOrEmpty() && currentEntry.unmasks.IsNullOrEmpty())
        {
            currentEntry.unmasks = new List<RectTransform>();
            for (int i = 0; i < currentEntry.masks.Count; i++)
            {
                var mask = currentEntry.masks[i];
                var unmask = AddMask(mask.pos, mask.size);
                unmask.SetParent(hideParent);
                currentEntry.unmasks.Add(unmask);
            }
        }
    }

    private void OnEntryExecInit(DialogueEntry entry)
    {
        delayTimer = 0;
        entry.inited = true;
    }

    private void OnEntryExecStart(DialogueEntry entry)
    {
        visualMaskGO.SetActive(!entry.unmasks.IsNullOrEmpty());
        if (!entry.unmasks.IsNullOrEmpty())
        {
            for (int i = 0; i < entry.unmasks.Count; i++)
            {
                var mask = entry.unmasks[i];
                mask.SetParent(visualMaskGO.transform, true);
                mask.SetAsFirstSibling();
                mask.sizeDelta = entry.masks[i].size;
                mask.anchoredPosition = entry.masks[i].pos;
            }
        }

        if (entry.transparentMask)
        {
            visualMask.color = Color.clear;
            visualMask.raycastTarget = false;
        }

        if (entry.pauseGame)
        {
            Time.timeScale = 0;
        }

        entry.onExecuting?.Invoke();
    }

    private void OnEntryExecEnd(DialogueEntry entry)
    {
        visualMaskGO.SetActive(false);
        if (!entry.unmasks.IsNullOrEmpty())
        {
            foreach (var mask in entry.unmasks)
            {
                mask.SetParent(hideParent);
            }
        }

        if (entry.transparentMask)
        {
            visualMask.color = Color.white;
            visualMask.raycastTarget = true;
        }

        if (entry.pauseGame)
        {
            Time.timeScale = 1;
        }

        entry.onExecuted?.Invoke();
        entry.onDialogueEntryExecInit -= OnEntryExecInit;
        entry.onDialogueEntryExecStart -= OnEntryExecStart;
        entry.onDialogueEntryExecEnd -= OnEntryExecEnd;
    }

    private RectTransform AddMask(Vector2 position, Vector2 size)
    {
        var newUnmask = Instantiate(unmaskPrefab, visualMaskGO.transform);
        var filter = visualMaskGO.AddComponent<UnmaskRaycastFilter>();
        filter.targetUnmask = newUnmask.GetComponent<Unmask>();

        unmasks.Add(newUnmask);
        raycastFilters.Add(filter);
        newUnmask.gameObject.SetActive(true);
        newUnmask.SetAsFirstSibling();
        newUnmask.sizeDelta = size;
        newUnmask.anchoredPosition = position;

        return newUnmask;
    }

    #region Event

    private void OnResourceRequirementMet(string requirementID)
    {
        UpdateEntries(EventConstant.ResourceRequirementMet, new object[] { requirementID });
    }

    private void OnCharacterStateChanged(Character character)
    {
        UpdateEntries(EventConstant.OnCharacterStateChanged, new object[] { character });
    }

    private void OnCharacterSlotChanged(Character character, ObjSlot slot)
    {
        UpdateEntries(EventConstant.OnCharacterSlotChanged, new object[] { character, slot });
    }

    #endregion
    
}
