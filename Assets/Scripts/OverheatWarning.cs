using System.Collections;
using System.Collections.Generic;
using JTUtility;
using UnityEngine;
using UnityEngine.UI;

public class OverheatWarning : MonoBehaviour
{
    [SerializeField] Image overheatWarningImage;
    [SerializeField] Vector2 greenFlashRange;
    [SerializeField] Vector2 yellowFlashRange;
    [SerializeField] Vector2 redFlashRange;
    [SerializeField] Vector2 zeroFlashRange;
    [SerializeField] float flashSpeed;

    Vector2 currentFlashRange;

    int currentLevel = -1;

    void Start()
    {
        SetOverheatLevel(0);
    }

    public void SetOverheatLevel(int level)
    {
        if (currentLevel == level)
        {
            return;
        }

        currentLevel = level;

        switch (level)
        {
            case 0:
                currentFlashRange = greenFlashRange;
                break;
            case 1:
                currentFlashRange = yellowFlashRange;
                break;
            case 2:
                currentFlashRange = redFlashRange;
                break;
            case 3:
                currentFlashRange = zeroFlashRange;
                break;
            default:
                currentFlashRange = greenFlashRange;
                break;
        }

        StopAllCoroutines();
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        bool flashUp = false;
        float timer = 0;

        float diff = Mathf.Abs(overheatWarningImage.color.a - currentFlashRange.y);
        float speed = (currentFlashRange.y - currentFlashRange.x) * flashSpeed;
        if (speed <= 0)
        {
            speed = 0.3f;
        }
        float time = diff / speed;
        float currentAlpha = overheatWarningImage.color.a;
        print("CurrentAlpha: " + currentAlpha + " CurrentFlashRange: " + currentFlashRange.y + " Time: " + time);

        while (timer < time)
        {
            timer += Time.deltaTime;
            overheatWarningImage.color = overheatWarningImage.color.AlterAlpha(Mathf.Lerp(currentAlpha, currentFlashRange.y, timer / time));
            yield return null;
        }

        while (true)
        {
            timer += Time.deltaTime * flashSpeed;
            if (timer > 1)
            {
                flashUp = !flashUp;
                timer = 0;
            }

            if (flashUp)
            {
                overheatWarningImage.color = overheatWarningImage.color.AlterAlpha(Mathf.Lerp(currentFlashRange.x, currentFlashRange.y, Mathf.Sin(timer * Mathf.Deg2Rad * 90)));
            }
            else
            {
                overheatWarningImage.color = overheatWarningImage.color.AlterAlpha(Mathf.Lerp(currentFlashRange.y, currentFlashRange.x, Mathf.Sin(timer * Mathf.Deg2Rad * 90)));
            }

            yield return null;
        }
    }

}
