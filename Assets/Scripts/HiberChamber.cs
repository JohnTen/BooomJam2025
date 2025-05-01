using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JTUtility;
using UnityEngine;
using UnityEngine.UI;

public class HiberChamber : MonoBehaviour
{
    [SerializeField] private List<CharacterSlot> characterSlots;
    [SerializeField] private List<GameObject> covers;
    [SerializeField] private ECoreSlot eCoreSlot;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private float timeToUnlock;

    private bool allUnlocked = false;

    private float timer;

    private void OnEnable()
    {
        progressSlider.value = 0;
        timer = 0;
    }

    private void Update()
    {
        if (covers.IsNullOrEmpty() || covers.All(c => !c.activeSelf))
        {
            allUnlocked = true;
        }

        if (allUnlocked)
        {
            timer = 0;
            progressSlider.value = progressSlider.maxValue;
        }
        else if (eCoreSlot.HasActiveObj)
        {
            timer += Time.deltaTime;
            progressSlider.value = progressSlider.maxValue * timer / timeToUnlock;
        }
        else
        {
            timer = 0;
            progressSlider.value = 0;
        }

        if (timer >= timeToUnlock)
        {
            timer = 0;
            foreach (var cover in covers)
            {
                if (cover.activeSelf)
                {
                    cover.SetActive(false);
                    break;
                }
            }
        }
    }
}
