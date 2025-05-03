using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;

public class Pop_upTween : MonoBehaviour
{
    public Vector3 originSize;
    public GameObject window;
    public RectTransform windowRT;

    [Header("TweenSeting")]
    public bool isStart = true; 
    public float fadeTime = 0.4f;
    public bool isOutBack = true;

    public bool enableUnfold = true;
    public bool enableScaleUp = false;
    public bool enableMove = true;

    [ConditionalField("enableMove")]
    public Vector3 moveTarget = new Vector3(10, 10, 0);

    private bool inited;

    private void OnEnable()
    {
        if (!inited)
        {
            originSize = windowRT.localScale;
            inited = true;
        }

        if (isStart)
        {
            ShowPopUp();
        }
    }

    public void ShowPopUp()
    {
        windowRT.localScale = Vector3.zero;

        window.SetActive(true);
        if (enableUnfold)
        {
            windowRT.localScale = new Vector3(originSize.x, 0f, 1f);
            windowRT.DOScaleY(originSize.y, fadeTime).SetEase(isOutBack?Ease.OutBack:Ease.OutSine).SetUpdate(true);
        }
        else if (enableScaleUp)
        {
            windowRT.DOScale(originSize, fadeTime).SetEase(Ease.OutSine).SetUpdate(true);
        }

        if (enableMove)
        {
            Vector3 origin = windowRT.localPosition;
            windowRT.localPosition = windowRT.localPosition - moveTarget;
            windowRT.DOAnchorPos(origin,fadeTime,true).SetEase(Ease.OutSine).SetUpdate(true);
        }
    }

    public void HidePopup()
    {
        windowRT.localScale = originSize;
        
        if (enableUnfold)
        {
            windowRT.DOScaleY(0, fadeTime).SetEase(isOutBack ? Ease.OutBack : Ease.OutSine).SetUpdate(true).OnComplete(() => { window.SetActive(false); });
        }
        else if (enableScaleUp)
        {
            Debug.Log("ScaleUp is not enabled, using default scale animation.");
            windowRT.DOScale(Vector3.zero, fadeTime).SetEase(Ease.OutSine).SetUpdate(true).OnComplete(() => { window.SetActive(false); });
        }

        if (enableMove)
        {
            Vector3 origin = windowRT.localPosition;
            windowRT.DOAnchorPos(origin-moveTarget, fadeTime, true).SetEase(Ease.OutSine).SetUpdate(true).OnComplete(() => { windowRT.localPosition = origin; window.SetActive(false); });
            
        }
    }



    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class ConditionalFieldAttribute : PropertyAttribute
    {
        public string FieldToCheck;
        public bool Inverse;

        public ConditionalFieldAttribute(string fieldToCheck, bool inverse = false)
        {
            FieldToCheck = fieldToCheck;
            Inverse = inverse;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ConditionalFieldAttribute))]
    public class ConditionalFieldDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ConditionalFieldAttribute conditional = (ConditionalFieldAttribute)attribute;
            SerializedProperty targetProperty = property.serializedObject.FindProperty(conditional.FieldToCheck);

            if (targetProperty != null)
            {
                bool enabled = targetProperty.boolValue;
                if (conditional.Inverse) enabled = !enabled;

                if (enabled)
                {
                    EditorGUI.PropertyField(position, property, label, true);
                }
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Error: Field not found");
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ConditionalFieldAttribute conditional = (ConditionalFieldAttribute)attribute;
            SerializedProperty targetProperty = property.serializedObject.FindProperty(conditional.FieldToCheck);

            if (targetProperty != null)
            {
                bool enabled = targetProperty.boolValue;
                if (conditional.Inverse) enabled = !enabled;

                if (enabled)
                {
                    return EditorGUI.GetPropertyHeight(property, label, true);
                }
            }

            return 0f;
        }
    }
#endif
}

