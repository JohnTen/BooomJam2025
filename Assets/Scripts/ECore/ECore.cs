using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Draggable))]
[RequireComponent(typeof(DragDropDetector))]
public class ECore : DraggableObj
{
    [Header("Visual")]
    [SerializeField] Image visualImage;
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite breakdownSprite;
    [SerializeField] Sprite warmUpSprite;
    [SerializeField] Image warmUpProgress;

    private Coroutine warmUpCoroutine;

    public bool IsWarmUp { get; private set; }

    public bool IsBreakdown { get; private set; }

    private void Start()
    {
        StartCoroutine(InitializePosition());
        if (visualImage != null)
        {
            StartCoroutine(VisualEffect());
        }
    }

    public override void SetSlot(ObjSlot slot)
    {
        if (slot != previousSlot && slot is ECoreSlot)
        {
            warmUpCoroutine = StartCoroutine(WarmUp());
        }
        base.SetSlot(slot);
    }

    protected override void OnDragStart()
    {
        base.OnDragStart();
        VirtualCursor.Instance.CursorSpeedMultiplier = GameManager.Instance.GameProperty.CoreDragSpeed;
    }

    protected override void OnDragEnd()
    {
        base.OnDragEnd();
        VirtualCursor.Instance.CursorSpeedMultiplier = 1f;
    }

    public void TriggerBreakdown()
    {
        print(name + " TriggerBreakdown");
        StartCoroutine(Breakdown());
    }

    private IEnumerator InitializePosition()
    {
        yield return null;
        if (slotDetector.DetectComponent())
        {
            var slot = slotDetector.TargetComponent as ObjSlot;
            previousSlot = slot;
            SetSlot(slot);
        }
    }

    private IEnumerator Breakdown()
    {
        InterruptWarmUp();
        IsBreakdown = true;
        draggable.CanDrag = false;
        visualImage.sprite = breakdownSprite;
        yield return new WaitForSeconds(GameManager.Instance.GameProperty.breakdownDuration);
        visualImage.sprite = normalSprite;
        IsBreakdown = false;
        warmUpCoroutine = StartCoroutine(WarmUp());
    }

    private IEnumerator WarmUp()
    {
        warmUpProgress.fillAmount = 0;
        draggable.CanDrag = false;
        IsWarmUp = true;
        visualImage.sprite = warmUpSprite;
        while (warmUpProgress.fillAmount < 1)
        {
            if (!draggable.IsDragging)
            {
                warmUpProgress.fillAmount += Time.deltaTime * GameManager.Instance.GameProperty.WarmUpSpeed;
            }
            yield return null;
        }
        draggable.CanDrag = true;
        IsWarmUp = false;
        visualImage.sprite = normalSprite;
    }

    private void InterruptWarmUp()
    {
        IsWarmUp = false;
        draggable.CanDrag = true;
        visualImage.sprite = normalSprite;
        warmUpProgress.fillAmount = 0;
        if (warmUpCoroutine != null)
        {
            StopCoroutine(warmUpCoroutine);
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
