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
    public GameObject ValueUpdatedObject;

    private void Start()
    {
        UpdateSlider();
    }

    public void LeftButton()
    {
        Debug.Log("LeftButton");
        currentValue -= step;
        UpdateSlider();
    }
    public void RightButton()
    {
        currentValue += step;
        UpdateSlider();
    }

    private void UpdateSlider()
    {
        slider.value = currentValue;
        Display();
    }
    public void OnValueChanged(float value)
    {
        currentValue = value;
        Display();
    }

    private void Display()
    {
        value.text = currentValue.ToString();
        if(ValueUpdatedObject) ValueUpdatedObject.SendMessage("ValueUpdated");
    }


}
