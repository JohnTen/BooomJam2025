using JTUtility;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSFX : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private List<AudioClip> clickSFX;
    [SerializeField] private List<AudioClip> hoverSFX;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        if (clickSFX.IsNullOrEmpty())
        {
            return;
        }

        AudioManager.instance.PlaySFX(clickSFX.PickRandom());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable == false || hoverSFX.IsNullOrEmpty())
            return;

        AudioManager.instance.PlaySFX(hoverSFX.PickRandom());
    }
}