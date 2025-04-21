using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pop_up : MonoBehaviour
    
{
    public GameObject window;
    public void ShowPopUp()
    {
        window.transform.localScale = new Vector3(1, 0, 1);

        window.SetActive(true);
 
        window.transform.DOScaleY(1f, 0.4f).SetEase(Ease.OutBack);
    }
}
