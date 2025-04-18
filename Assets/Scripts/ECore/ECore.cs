using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Draggable))]
[RequireComponent(typeof(DragDropDetector))]
public class ECore : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] Image visualImage;

    private Draggable draggable;
    private DragDropDetector eCoreSlotDetector;

    private ECoreSlot currentSlot;
    private ECoreSlot previousSlot;

    void OnEnable()
    {
        if (draggable == null)
        {
            draggable = GetComponent<Draggable>();
        }

        if (eCoreSlotDetector == null)
        {
            eCoreSlotDetector = GetComponent<DragDropDetector>();
        }
        eCoreSlotDetector.TargetComponentType = typeof(ECoreSlot);

        draggable.OnDragStart.AddListener(OnDragStart);
        draggable.OnDragEnd.AddListener(OnDragEnd);
    }

    void OnDisable()
    {
        draggable.OnDragStart.RemoveListener(OnDragStart);
        draggable.OnDragEnd.RemoveListener(OnDragEnd);
    }

    private void Start()
    {
        StartCoroutine(InitializePosition());
        if (visualImage != null)
        {
            StartCoroutine(VisualEffect());
        }
    }
    
    private void Update()
    {
        if (draggable.IsDragging)
        {
            // 处理slot的状态变化
            if (eCoreSlotDetector.TargetComponent != null)
            {
                var slot = eCoreSlotDetector.TargetComponent as ECoreSlot;
                if (slot != null && !slot.HasCore && currentSlot != slot)
                {
                    slot.StartBlink();
                    if (currentSlot != null)
                    {
                        currentSlot.StopBlink();
                    }
                }
                currentSlot = slot;
            }
            else
            {
                if (currentSlot != null)
                {
                    currentSlot.StopBlink();
                    currentSlot = null;
                }
            }
        }
    }

    public void SetSlot(ECoreSlot slot)
    {
        if (currentSlot != null)
        {
            currentSlot.SetCore(null);
        }
        currentSlot = slot;
        currentSlot.SetCore(this);
        currentSlot.StopBlink();

        transform.rotation = slot.transform.rotation;
        transform.position = slot.transform.position;
    }

    private void OnDragStart()
    {
        previousSlot = currentSlot;
        if (currentSlot != null)
        {
            currentSlot.SetCore(null);
            currentSlot = null;
        }

        transform.rotation = Quaternion.identity;
    }

    private void OnDragEnd()
    {
        if (eCoreSlotDetector.TargetComponent != null)
        {
            var slot = eCoreSlotDetector.TargetComponent as ECoreSlot;
            SetSlot(slot);
        }
        else
        {
            SetSlot(previousSlot);
        }
    }

    private IEnumerator InitializePosition()
    {
        yield return null;
        yield return null;
        yield return null;
        if (eCoreSlotDetector.DetectComponent())
        {
            var slot = eCoreSlotDetector.TargetComponent as ECoreSlot;
            SetSlot(slot);
        }
    }

    private IEnumerator VisualEffect()
    {
        float visualDiffer = Random.value * 5f;
        
        var material = visualImage.materialForRendering;
        visualImage.material = new Material(material);
        material = visualImage.material;
        while (true)
        {
            var time = Time.time + visualDiffer;
            material.SetFloat("_NoiseScale", Mathf.Lerp(0.01f, 0.1f, Mathf.Sin(time * 1f)));
            material.SetFloat("_y", Mathf.Lerp(-0.9f, 0.9f, Mathf.Tan(time * 0.5f)));
            yield return null;
        }
    }
}
