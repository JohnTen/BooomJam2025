using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using JTUtility;
using JTUtility.Event;
using UnityEngine;
using UnityEngine.EventSystems;

public class CoreWarnings : MonoBehaviour
{
    [Serializable] struct CoreWarning
    {
        public string Green;
        public string Yellow;
        public string Red;
        public string Zero;

        public string this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return Green;
                    case 1: return Yellow;
                    case 2: return Red;
                    case 3: return Zero;
                }
                return null;
            }
        }
    }
    [Serializable] class CoreWarningCollection : EnumBasedCollection<CoreSlotType, CoreWarning> {}
    [SerializeField] private GameObject warningSign;
    [SerializeField] private GameObject warningPanel;
    [SerializeField] private TMPro.TMP_Text warningText;
    [SerializeField] private CoreWarningCollection coreWarnings;

    void OnEnable()
    {
        EventRegister<CoreSlotType, int>.Register(EventConstant.CoreStageChanged, OnCoreStageChanged);
    }

    void OnDisable()
    {
        EventRegister<CoreSlotType, int>.UnRegister(EventConstant.CoreStageChanged, OnCoreStageChanged);
    }

    void OnCoreStageChanged(CoreSlotType coreSlotType, int stage)
    {
        StringBuilder stringBuilder = new StringBuilder();

        bool hasWarning = false;
        foreach (CoreSlotType slotType in Enum.GetValues(typeof(CoreSlotType)))
        {
            var warning = coreWarnings[slotType][GameManager.Instance.coreStage[slotType]];
            warning = TextDatabase.Instance.GetLNItem(warning);
            if (string.IsNullOrEmpty(warning))
            {
                continue;
            }

            if (stringBuilder.Length > 0)
            {
                stringBuilder.Append("\n");
            }

            stringBuilder.Append(warning);
            hasWarning = true;
        }

        warningText.text = stringBuilder.ToString();
        warningSign.SetActive(hasWarning);
        warningPanel.SetActive(hasWarning);
       
    }
}
