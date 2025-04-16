using UnityEngine;
using UnityEngine.EventSystems;

public class EventTriggerListener : UnityEngine.EventSystems.EventTrigger
{
    public delegate void VoidDelegate2(GameObject go);

    public delegate void VoidDelegate();

    public VoidDelegate onClick;
    public VoidDelegate onDown;
    public VoidDelegate onEnter;
    public VoidDelegate onExit;
    public VoidDelegate onUp;
    public VoidDelegate onSelect;
    public VoidDelegate onUpdateSelect;

    public VoidDelegate2 onClick2;
    public VoidDelegate2 onDown2;
    public VoidDelegate2 onEnter2;
    public VoidDelegate2 onExit2;
    public VoidDelegate2 onUp2;
    public VoidDelegate2 onSelect2;
    public VoidDelegate2 onUpdateSelect2;

    public KeyCode keyCode = KeyCode.None;

    public static EventTriggerListener Get(GameObject go)
    {
        EventTriggerListener listener = go.GetComponent(typeof(EventTriggerListener)) as EventTriggerListener;
        if (listener == null) listener = go.AddComponent(typeof(EventTriggerListener)) as EventTriggerListener;
        return listener;
    }

    public static EventTriggerListener Get(GameObject go, KeyCode keyCode)
    {
        EventTriggerListener listener = go.GetComponent(typeof(EventTriggerListener)) as EventTriggerListener;
        if (listener == null) listener = go.AddComponent(typeof(EventTriggerListener)) as EventTriggerListener;
        listener.keyCode = keyCode;
        return listener;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null) onClick();
        if (onClick2 != null) onClick2(gameObject);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (onDown != null) onDown();
        if (onDown2 != null) onDown2(gameObject);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (onEnter != null) onEnter();
        if (onEnter2 != null) onEnter2(gameObject);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (onExit != null) onExit();
        if (onExit2 != null) onExit2(gameObject);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (onUp != null) onUp();
        if (onUp2 != null) onUp2(gameObject);
    }

    public override void OnSelect(BaseEventData eventData)
    {
        if (onSelect != null) onSelect();
        if (onSelect2 != null) onSelect2(gameObject);
    }

    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if (onUpdateSelect != null) onUpdateSelect();
        if (onUpdateSelect2 != null) onUpdateSelect2(gameObject);
    }

    public void Update()
    {
        if (keyCode != KeyCode.None && Input.GetKeyDown(keyCode))
        {
            if (onClick != null) onClick();
            if (onClick2 != null) onClick2(gameObject);
        }
    }
}