using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ExploreResult
{
    public string id;
    public List<string> resources;
    public List<int> resourceAmounts;
    public string dialogueID;
    public float weight;
    public bool oneTimeOnly = false;
}

public class ExploreNode : MonoBehaviour
{
    [SerializeField] CharacterSlot characterSlot;
    [SerializeField] ECoreSlot eCoreSlot;

    [SerializeField] List<ExploreResult> exploreResults;

    [SerializeField] UnityEvent onExploreReady;

    bool isExploreReady = false;

    void Update()
    {
        if (characterSlot.HasObj)
        {
            eCoreSlot.gameObject.SetActive(true);
        }
        else if (!eCoreSlot.HasObj)
        {
            eCoreSlot.gameObject.SetActive(false);
        }

        if (!isExploreReady && characterSlot.HasObj && eCoreSlot.HasObj)
        {
            isExploreReady = true;
            onExploreReady.Invoke();
        }
        else if (isExploreReady && (!characterSlot.HasObj || !eCoreSlot.HasObj))
        {
            isExploreReady = false;
        }
    }

    public void Explore()
    {
        var weightSum = 0f;
        foreach (var result in exploreResults)
        {
            weightSum += result.weight;
        }
        
        var randomValue = UnityEngine.Random.Range(0f, weightSum);
        var weight = 0f;
        for (int i = 0; i < exploreResults.Count; i++)
        {
            var result = exploreResults[i];
            weight += result.weight;
            if (randomValue < weight)
            {
                ApplyExploreResult(result);
                break;
            }
        }
    }
    
    private void ApplyExploreResult(ExploreResult result)
    {
        for (int i = 0; i < result.resources.Count; i++)
        {
            var resource = result.resources[i];

            var inventorySlot = GameManager.Instance.GetStackableInventorySlot(resource);
            if (inventorySlot == null)
            {
                inventorySlot = GameManager.Instance.GetEmptyInventorySlot();
            }

            if (inventorySlot == null)
            {
                Debug.LogWarning("No inventory slot found for resource: " + resource);
                return;
            }

            if (resource != "ECrystal")
            {
                var resourceObj = Instantiate(PrefabHub.ResourceObjPrefab, inventorySlot.transform).GetComponent<ResourceObj>();
                resourceObj.Init(ResourceDatabase.Instance.GetTemplate(resource), result.resourceAmounts[i], null);
                inventorySlot.TryAddObj(resourceObj);
            }
            else
            {
                var eCrystalObj = Instantiate(PrefabHub.ECrystalObjPrefab, inventorySlot.transform).GetComponent<ECrystal>();
                inventorySlot.TryAddObj(eCrystalObj);
            }
        }

        if (result.dialogueID != null)
        {
            DialogueManager.Instance.PlayDialogue(result.dialogueID);
        }

        if (result.oneTimeOnly)
        {
            exploreResults.Remove(result);
        }
    }
}
