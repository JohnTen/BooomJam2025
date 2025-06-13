using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHoverWindow : MonoBehaviour
{
    [SerializeField] private string title;
    [SerializeField] private string description;
    [SerializeField] bool isResource;

    [Header("Debug")]
    [SerializeField] private string debugTitle;
    [SerializeField] private string debugDescription;

    ResourceObj resourceObj;

    public string Title => isResource ? resourceObj.Template.name : title;
    public string Description => isResource ? resourceObj.Template.description : description;

    void OnValidate()
    {
        debugTitle = string.Empty;
        debugDescription = string.Empty;
        if (TextDatabase.Instance.ContainsID(title))
        {
            debugTitle = TextDatabase.Instance.GetLNItem(title);
        }
        if (TextDatabase.Instance.ContainsID(description))
        {
            debugDescription = TextDatabase.Instance.GetLNItem(description);
        }
    }

    private void OnEnable()
    {
        if (isResource && resourceObj == null)
        {
            resourceObj = GetComponent<ResourceObj>();
        }
    }

    void Update()
    {
        if (isResource && resourceObj == null)
        {
            resourceObj = GetComponent<ResourceObj>();
        }
    }
}
