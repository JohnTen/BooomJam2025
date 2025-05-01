using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReverseBoolEvent : MonoBehaviour
{
    [SerializeField] UnityEvent<bool> onEvent;

    public void OnEvent(bool value)
    {
        onEvent.Invoke(!value);
    }
}
