using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class NodeManager : MonoBehaviour
{
    public string currentNodeId;
    public UnityEvent initializeNode;

    public void InitializeNode()
    { 
        initializeNode.Invoke();
    }
}
