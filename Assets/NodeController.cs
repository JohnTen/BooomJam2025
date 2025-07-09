using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class NodeController : BasePrioritizedPointerHandler
{
    public GameObject nodeDetail;
    public GameObject nodeSimplify;
    public GameObject nodeIcon;

    public float nodeOutTime = 0.2f;

    public bool isSimlify = false;
    private bool isUnfold = false;
    private Vector3 nodeDetailPosition;

    void Start()
    {
        isUnfold = nodeDetail.activeSelf;
        nodeDetailPosition = nodeDetail.transform.localPosition;
    }

    public void InitializedNode()
    {
        nodeDetail.SetActive(false);
        nodeSimplify.SetActive(false);
    }

    public void UnfoldNode()
    {
        if (isUnfold) return;
        nodeIcon.SetActive(false);
        nodeSimplify.SetActive(false);
        nodeDetail.SetActive(true);
        nodeDetail.transform.localScale = Vector3.zero;
        nodeDetail.transform.DOScale(Vector3.one, nodeOutTime).SetEase(Ease.OutSine);
        nodeDetail.transform.position = nodeIcon.transform.position;
        nodeDetail.transform.DOLocalMove(nodeDetailPosition, nodeOutTime).SetEase(Ease.OutSine);
    }

    public void FoldNode()
    {
        nodeDetail.transform.DOMove(nodeIcon.transform.position, nodeOutTime).SetEase(Ease.InSine);
        nodeDetail.transform.DOScale(Vector3.zero, nodeOutTime).SetEase(Ease.InSine).OnComplete(() =>
        {
            isUnfold = false;
            if (isSimlify)
            {
                nodeIcon.SetActive(false);
                nodeSimplify.SetActive(true);
            }
            else
            {
                nodeIcon.SetActive(true);
                nodeSimplify.SetActive(false);
            }
            
        });
    }

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    throw new System.NotImplementedException();
    //}

    protected override void OnPointerEnter(PointerEventData eventData)
    {
        UnfoldNode();
    }

    protected override void OnPointerExit(PointerEventData eventData)
    {
        FoldNode();
    }
}
