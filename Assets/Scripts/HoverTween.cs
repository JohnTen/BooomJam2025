using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using Unity.VisualScripting;

public class HoverTween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{   
    [Header("Parameter")]
    [SerializeField] private float hoverScale = 1.2f; // 鼠标悬停时的缩放比例
    [SerializeField] private float hoverDuration = 0.3f; // 鼠标悬停时的缩放持续时间
    [SerializeField] private float hoverPunchAngle = 5;// 鼠标悬停时的抖动角度
    [SerializeField] private int hoverPunchStrength = 15; // 鼠标悬停时的抖动强度
    [SerializeField] private float downPunchScale = 0.1f; // 鼠标按下时的缩放比例
    [SerializeField] private float downDuration = 0.2f; // 鼠标按下时的缩放持续时间
    [SerializeField] private int downPunchStrength = 10; // 鼠标按下时的抖动强度
    [SerializeField] private float downPunchOffset = 10f; // 鼠标按下时的位移偏移量

    [Header("Function Toggles")]
    [SerializeField] private bool enableHoverScaleChange = false; 
    [SerializeField] private bool enableHoverPunch = false;
    [SerializeField] private bool enableDownPunch = false;
    [SerializeField] private bool enableDownMove = false;

    private void Start()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (enableHoverScaleChange)
        {
            HoverScaleChange();
        }

        if (enableHoverPunch)
        {
            HoverPunch();
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (enableHoverScaleChange)
        {
            HoverScaleRewind();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(enableDownPunch)
        {
            DownPunch();
        }

        if (enableDownMove)
        {
            DownMove();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
       
    }

    public void HoverScaleChange()
    {       
        gameObject.transform.DOScale(Vector3.one * hoverScale, hoverDuration).SetEase(Ease.OutBack);       
    }

    public void HoverScaleRewind()
    {
        gameObject.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
    }

    public void HoverPunch()
    {
        DOTween.Kill(1, true);
        gameObject.transform.DOPunchRotation(Vector3.forward * hoverPunchAngle, hoverDuration, hoverPunchStrength, 1).SetId(1);
    }

    public void DownPunch()
    {
        DOTween.Kill(2, true);
        gameObject.transform.DOPunchScale(Vector3.one * downPunchScale, downDuration, downPunchStrength, 0).SetId(2);
    }

    public void DownMove()
    { 
        DOTween.Kill(3, true);
        gameObject.transform.DOPunchPosition(Vector3.down * downPunchOffset, downDuration, downPunchStrength, 0).SetId(3);
    }

    public void PanelIdle()
    {
        float sine = Mathf.Sin(Time.time);
        float cosine = Mathf.Cos(Time.time);

        float randomX = Random.Range(0, 0.2f);

        float lerpX = Mathf.Lerp(gameObject.transform.position.x, sine + 1f, Time.deltaTime);
        float lerpY = Mathf.Lerp(gameObject.transform.position.y, cosine * 0.2f, Time.deltaTime);

        gameObject.transform.position = new Vector3(0f, lerpY, gameObject.transform.position.z);

    }

    private void Update()
    {

    }

    

}
