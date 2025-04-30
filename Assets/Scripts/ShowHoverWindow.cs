using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHoverWindow : MonoBehaviour
{
    [SerializeField] private string title;
    [SerializeField] private string description;
    [SerializeField] bool isResource;

    ResourceObj resourceObj;

    public string Title => isResource ? resourceObj.Template.name : title;
    public string Description => isResource ? resourceObj.Template.description : description;

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
