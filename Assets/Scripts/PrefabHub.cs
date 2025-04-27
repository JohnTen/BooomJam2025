using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabHub", menuName = "PrefabHub")]
public class PrefabHub : ScriptableObject
{
    [SerializeField] private GameObject resourceObjPrefab;
    [SerializeField] private GameObject eCrystalObjPrefab;

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
    public static GameObject ECrystalObjPrefab => Instance.eCrystalObjPrefab;
    void OnEnable()
    {
        instance = this;
    }
}
