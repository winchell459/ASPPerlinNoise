using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderButton : MonoBehaviour
{
    public Text label, value;
    public Slider slider;
    [SerializeField] private float currentValue;
    public float step = 1;

    private void Start()
    {
        Display();
    }

    public void LeftButton()
    {
        Debug.Log("LeftButton");
        currentValue -= step;
        Display();
    }
    public void RightButton()
    {
        currentValue += step;
        Display();
    }

    public void OnValueChanged(float value)
    {
        Display();
    }

    private void Display()
    {
        value.text = currentValue.ToString();

    }


}
