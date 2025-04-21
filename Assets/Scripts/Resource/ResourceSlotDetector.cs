using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Draggable))]
public class ResourceSlotDetector : MonoBehaviour
{
    [SerializeField] private ResourceSlot resourceSlot;
    public ResourceSlot DetectedSlot
    {
        get
        {
            return resourceSlot;
        }
    }

    private Draggable draggable;

    void OnEnable()
    {
        if (draggable == null)
        {
            draggable = GetComponent<Draggable>();
        }
    }

    void Update()
    {
        if (draggable.IsDragging)
        {
            var pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = VirtualCursor.ScreenPosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);

            resourceSlot = null;
            foreach (var result in results)
            {
                if (result.gameObject.TryGetComponent(out ResourceSlot slot))
                {
                    resourceSlot = slot;
                }
            }
        }
    }
}
