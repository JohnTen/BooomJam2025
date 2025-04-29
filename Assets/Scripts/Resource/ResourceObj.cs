using System.Collections;
using System.Collections.Generic;
using JTUtility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Draggable))]
[RequireComponent(typeof(DragDropDetector))]
public class ResourceObj : DraggableObj
{
    [SerializeField] Image icon;
    [SerializeField] TMPro.TMP_Text stackText;
    [SerializeField] int stack;
    [SerializeField] string templateID;

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

    protected override void OnEnable()
    {
        base.OnEnable();

        if (!string.IsNullOrEmpty(templateID))
        {
            var template = ResourceDatabase.Instance.GetTemplate(templateID);
            if (template != null)
            {
                Init(template, stack, null);
            }
        }
    }

    public void Init(ResourceTemplate template, int stack, ObjSlot slot)
    {
        this.template = template;
        icon.sprite = template.icon;
        Stack = stack;
        currentSlot = slot;
        name = template.name;
    }

    public override void SetSlot(ObjSlot slot, bool force = false)
    {
        if (slot.IsNotNull())
        transform.SetParent(slot.ObjParent);
        base.SetSlot(slot, force);
    }
}
