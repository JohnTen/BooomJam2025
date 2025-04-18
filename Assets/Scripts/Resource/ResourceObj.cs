using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

[RequireComponent(typeof(Draggable))]
[RequireComponent(typeof(DragDropDetector))]
public class ResourceObj : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TMPro.TMP_Text stackText;
    [SerializeField] int stack;
    
    Draggable draggable;
    DragDropDetector resourceSlotDetector;

    private ResourceTemplate template;
    public ResourceTemplate Template
    {
        get
        {
            return template;
        }
    }

    public int Stack
    {
        get
        {
            return stack;
        }
        set
        {
            stack = value;
            stackText.text = stack.ToString();
            
            if (stack < 0)
            {
                Debug.LogWarning("ResourceObj " + template.name + " stack is less than 0");
            }

            if (stack <= 0)
            {
                Destroy(gameObject);
                print ("ResourceObj " + template.name + " is destroyed");
            }
        }
    }

    public ResourceSlot Slot;
    public ResourceSlot PreviousSlot;

    void OnEnable()
    {
        if (draggable == null)
        {
            draggable = GetComponent<Draggable>();
        }

        if (resourceSlotDetector == null)
        {
            resourceSlotDetector = GetComponent<DragDropDetector>();
        }
        resourceSlotDetector.TargetComponentType = typeof(ResourceSlot);

        draggable.OnDragStart.AddListener(OnDragStart);
        draggable.OnDragEnd.AddListener(OnDragEnd);
    }

    void OnDisable()
    {
        draggable.OnDragStart.RemoveListener(OnDragStart);
        draggable.OnDragEnd.RemoveListener(OnDragEnd);
    }

    public void Init(ResourceTemplate template, int stack, ResourceSlot slot)
    {
        this.template = template;
        icon.sprite = template.icon;
        Stack = stack;
        Slot = slot;
        name = template.name;
    }

    private void OnDragStart()
    {
        PreviousSlot = Slot;
        var resObj = Slot.TakeResource();
        if (resObj != this)
        {
            Debug.LogWarning("OnDragStart: resObj is not this");
        }
    }

    private void OnDragEnd()
    {
        if (resourceSlotDetector.TargetComponent != null)
        {
            var slot = resourceSlotDetector.TargetComponent as ResourceSlot;
            if (!slot.TryAddResource(this))
            {
                PreviousSlot.TryAddResource(this, true);
            }
        }
        else
        {
            PreviousSlot.TryAddResource(this, true);
        }
    }
}
