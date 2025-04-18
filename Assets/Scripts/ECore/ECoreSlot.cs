using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using JTUtility;
using UnityEngine.Events;

public class ECoreSlot : MonoBehaviour
{
    [SerializeField] Image indicator;
    [SerializeField] UnityEvent<bool> OnCoreChanges;
    private Color greenColor = new Color(0, 1, 0, 1);
    private Color redColor = new Color(1, 0, 0, 1);
    private Coroutine blinkCoroutine;

    public bool HasCore => eCore != null;

    private ECore eCore;
    public ECore ECoreInSlot => eCore;

    private void Start()
    {
        UpdateIndicator(false);
    }

    public void StartBlink()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
        blinkCoroutine = StartCoroutine(BlinkIndicator());
    }

    public void StopBlink()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
        UpdateIndicator(HasCore);
    }

    public void SetCore(ECore eCore)
    {
        if (this.eCore != eCore)
        {
            this.eCore = eCore;
            OnCoreChanges.Invoke(HasCore);
        }

        UpdateIndicator(HasCore);
    }

    private void UpdateIndicator(bool hasCore)
    {
        indicator.color = hasCore ? greenColor : redColor;
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
}
