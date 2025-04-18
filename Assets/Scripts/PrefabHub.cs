using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabHub", menuName = "PrefabHub")]
public class PrefabHub : ScriptableObject
{
    [SerializeField] private GameObject resourceObjPrefab;

    private static PrefabHub instance;
    public static PrefabHub Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<PrefabHub>("PrefabHub");
            }
            return instance;
        }
    }
    
    public static GameObject ResourceObjPrefab => Instance.resourceObjPrefab;

    void OnEnable()
    {
        instance = this;
    }
}
