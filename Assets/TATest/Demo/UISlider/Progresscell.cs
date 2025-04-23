using UnityEngine;
using UnityEngine.UI;

public class Progresscell : RawImage {
    
    private Slider _Progresscell;
    
    // Define color thresholds
    private const float HealthyThreshold = 8f;  // 80%
    private const float WarningThreshold = 3f;   // 30%
    
    // Define colors for different states
    private readonly Color HealthyColor = Color.green;
    private readonly Color WarningColor = Color.yellow;
    private readonly Color DangerColor = Color.red;

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();

        // Get the progress bar (Slider component)
        if (_Progresscell == null)
            _Progresscell = transform.parent.parent.GetComponent<Slider>();

        // Update the progress bar display
        if (_Progresscell != null)
        {
            // Get the current value
            float value = _Progresscell.value;
            
            // Update the UV rectangle to show the correct progress
            uvRect = new Rect(0, 0, value, 1);
            
            // Change color based on the current value
            if (value > HealthyThreshold)
            {
                color = HealthyColor;
            }
            else if (value > WarningThreshold)
            {
                color = WarningColor;
            }
            else
            {
                color = DangerColor;
            }
        }
    }
}