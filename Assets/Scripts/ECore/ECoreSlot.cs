using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using JTUtility;
using UnityEngine.Events;
using Unity.VisualScripting;

public class ECoreSlot : ObjSlot
{
    [SerializeField] Image indicator;
    [SerializeField] UnityEvent<bool> OnCoreChanges;
    [SerializeField] private bool blinkIndicator = true;
    [SerializeField] private bool noWarmUp = false;
    private Color greenColor = new Color(0, 1, 0, 1);
    private Color redColor = new Color(1, 0, 0, 1);
    private Coroutine blinkCoroutine;

    public ECore ECoreInSlot => ObjInSlot as ECore;

    public override bool HasActiveObj => base.HasActiveObj && !ECoreInSlot.IsWarmUp && !ECoreInSlot.IsBreakdown;

    public bool NoWarmUp => noWarmUp;

    private void Start()
    {
        UpdateIndicator(false);
    }

    public override bool TryAddObj(Component obj)
    {
        if (!CanAdd(obj))
        {
            return false;
        }

        if (obj != ObjInSlot)
        {
            OnCoreChanges.Invoke(HasObj);
        }

        AddObj(obj);
        return true;
    }

    public override void AddObj(Component obj)
    {
        base.AddObj(obj);
        UpdateIndicator(HasObj);
    }

    public override void ClearObj()
    {
        base.ClearObj();
        UpdateIndicator(HasObj);
    }

    private void UpdateIndicator(bool hasObj)
    {
        if (blinkIndicator)
        {
            indicator.color = hasObj ? greenColor : redColor;
        }
    }

    private IEnumerator BlinkIndicator()
    {
        while (true)
        {
            indicator.color = Color.yellow;
            yield return new WaitForSeconds(0.3f);
            indicator.color = Color.yellow.AlterAlpha(0.3f);
            yield return new WaitForSeconds(0.3f);
        }
    }

    public override bool CanAdd(Component obj)
    {
        return obj is ECore && !HasObj;
    }

    public override bool CanRemove(Component obj)
    {
        return obj is ECore && obj == ObjInSlot;
    }

    public override void OnObjEnter(Component obj)
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }

        if (blinkIndicator)
        {
            blinkCoroutine = StartCoroutine(BlinkIndicator());
        }
    }
    
    public override void OnObjExit(Component obj)
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }

        UpdateIndicator(HasObj);
    }
}
