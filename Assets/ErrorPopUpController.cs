using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ErrorPopUpController : MonoBehaviour
{
    public List<GameObject> windows;
    public float delayTime = 0.2f;
    public int i= 0;

    public UnityEvent unityEvent;

    public void StartErrorPlay()
    {
        StartCoroutine(ShowWindowsWithDelay());
    }

    private IEnumerator ShowWindowsWithDelay()
    {
        for (int index = 0; index < windows.Count; index++)
        {
            GameObject window = windows[index];
            window.SetActive(true);
            yield return new WaitForSeconds(delayTime);
            if (index == windows.Count - 1)
            {
                unityEvent?.Invoke();
            }            
        }
    }
}
