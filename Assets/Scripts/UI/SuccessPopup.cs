using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SuccessPopup : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject root;
    [SerializeField] private TextMeshProUGUI Text;

    public void ShowPopUp(string Name)
    {
        Text.text = Name;
        root.SetActive(true);
    }
}
