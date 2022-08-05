using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerlinNoiseMenu : MonoBehaviour
{
    public GameObject perlinNoisePanel;
    public bool active;
    public void Active(bool active)
    {
        this.active = active;
        perlinNoisePanel.SetActive(active);
    }
}
