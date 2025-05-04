using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlinkIndicator : Indicator
{
    [SerializeField] private Image indicator;
    [SerializeField] private Color blinkColor = new Color(1, 1, 1, 0.6f);
    [SerializeField] private float blinkInterval = 0.25f;

    private Color defaultColor = Color.white;

    private Coroutine blinkCoroutine;

    private void Start()
    {
        if (indicator == null)
        {
            indicator = GetComponent<Image>();
        }

        if (indicator != null)
        {
            defaultColor = indicator.color;
        }
    }

    private IEnumerator BlinkCoroutine()
    {
        if (indicator == null)
        {
            yield break;
        }

        while (true)
        {
            indicator.color = defaultColor;
            yield return new WaitForSeconds(blinkInterval);
            indicator.color = blinkColor;
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    public override void SetActive(bool active)
    {
        if (active && blinkCoroutine == null)
        {
            blinkCoroutine = StartCoroutine(BlinkCoroutine());
        }
        else if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            if (indicator != null)
            {
                indicator.color = defaultColor;
            }
            blinkCoroutine = null;
        }
    }
}
