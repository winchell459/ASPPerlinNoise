using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDHandler : MonoBehaviour
{
    public Text debugText;

    public void SetTimer(string value)
    {
        debugText.text = value;
    }

    public void DisplayTimer(bool display)
    {
        debugText.gameObject.SetActive(display);
    }
}
