using System.Collections;
using JTUtility;
using UnityEngine;
using UnityEngine.UI;

public class ECore : ECrystal
{
    [Header("Visual")]
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite breakdownSprite;
    [SerializeField] Sprite warmUpSprite;
    [SerializeField] Image warmUpProgress;

    private Coroutine warmUpCoroutine;


    public bool IsWarmUp { get; private set; }

    public bool IsBreakdown { get; private set; }

    protected override void Start()
    {
        if (visualImage != null)
        {
            StartCoroutine(VisualEffect());
        }
        
        base.Start();
    }

    public override void SetSlot(ObjSlot slot, bool force = false)
    {
        if (slot != previousSlot && slot is ECoreSlot eCoreSlot && !eCoreSlot.NoWarmUp)
        {
            warmUpCoroutine = StartCoroutine(WarmUp());
        }
        base.SetSlot(slot, force);
    }

    public void TriggerBreakdown()
    {
        print(name + " TriggerBreakdown");
        StartCoroutine(Breakdown());
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
        var fillAmount = 0f;
        warmUpProgress.fillAmount = fillAmount;
        draggable.CanDrag = false;
        IsWarmUp = true;
        visualImage.sprite = warmUpSprite;
        while (fillAmount < 1)
        {
            if (!draggable.IsDragging)
            {
                fillAmount += Time.deltaTime * GameManager.Instance.GameProperty.WarmUpSpeed;
                warmUpProgress.fillAmount = fillAmount;
                material.SetFloat("_energy", fillAmount);
            }
            yield return null;
        }
        material.SetFloat("_energy", 0.9999f);
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
        yield return new WaitForSeconds(visualDiffer);
        
        while (true)
        {
            var time = Time.time + visualDiffer;
            material.SetFloat("_NoiseScale", Mathf.Lerp(0.01f, 0.1f, Mathf.Sin(time * 1f)));
            material.SetFloat("_y", Mathf.Lerp(-0.9f, 0.9f, Mathf.Tan(time * 0.5f)));
            yield return null;
        }
    }
}
