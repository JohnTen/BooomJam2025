using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JTUtility.UI
{
    [System.Serializable]
    internal class SwitchButtonData
    {
        public string Label = string.Empty;
        public Color ButtonColor = Color.white;
        public Color LabelColor = Color.black;
        public bool UseButtonSprite = false;
        public Sprite ButtonSprite = null;
        public StringEvent OnSwitchOn = new StringEvent();
    }

    internal class SwitchButton : MonoBehaviour
    {
        [System.Serializable]
        private class IntEvent : UnityEvent<int>
        { }

        [SerializeField]
        private SwitchButtonData[] data = new SwitchButtonData[0];

        [SerializeField]
        private int CurrentIndex = 0;

        [SerializeField]
        private IntEvent OnSwitch = null;

        [SerializeField]
        private Image ButtonImage = null;

        [SerializeField]
        private Text ButtonLabel = null;

        [SerializeField]
        private TMPro.TMP_Text ButtonTMPLabel = null;

        private bool disabledEvent;

        private void Awake()
        {
            disabledEvent = true;
            SetTo(CurrentIndex);
            disabledEvent = false;
        }

        public void Switch()
        {
            CurrentIndex++;
            if (CurrentIndex >= data.Length) CurrentIndex = 0;

            setButton(CurrentIndex);
        }

        public void SetTo(int index)
        {
            setButton(index);
        }

        private void setButton(int index)
        {
            CurrentIndex = index;
            if (ButtonImage != null) ButtonImage.color = data[index].ButtonColor;
            if (ButtonImage != null && data[index].UseButtonSprite) ButtonImage.sprite = data[index].ButtonSprite;
            if (ButtonLabel != null) ButtonLabel.color = data[index].LabelColor;
            if (ButtonLabel != null) ButtonLabel.text = data[index].Label;
            if (ButtonTMPLabel != null) ButtonTMPLabel.color = data[index].LabelColor;
            if (ButtonTMPLabel != null) ButtonTMPLabel.text = data[index].Label;

            if (disabledEvent)
                return;

            if (data[index].OnSwitchOn != null)
                data[index].OnSwitchOn.Invoke(data[index].Label);

            OnSwitch.Invoke(index);
        }
    }
}