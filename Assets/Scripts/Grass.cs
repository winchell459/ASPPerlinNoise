using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    public float growthRate = 1;
    public float nutrience = 100;
    
    public void Eat(float value)
    {
        nutrience -= value;
        SetScale(nutrience / maxNutrience);
    }

    [SerializeField] private float maxNutrience;
    [SerializeField] private float maxScale;
    [SerializeField] private float percentBuffer = 0.1f;

    private void Start()
    {
        maxScale = transform.localScale.x;
        maxNutrience = nutrience;
    }
    private void Update()
    {
        float nutriencePercent = transform.localScale.x / maxScale;
        float scalePercent = nutrience / maxNutrience;
        //Debug.Log($"{nutriencePercent} {scalePercent}");
        if (nutriencePercent - scalePercent > percentBuffer)
        {
            transform.localScale -= Vector3.one * maxScale * Time.deltaTime * growthRate / 100;
        }else if (scalePercent - nutriencePercent > percentBuffer)
        {
            transform.localScale += Vector3.one * maxScale * Time.deltaTime * growthRate / 100;
        }
    }
    
    void SetScale(float scalePercent)
    {
        transform.localScale = Vector3.one * scalePercent / maxScale;
    }

    //float GetScalePercent()
    //{
    //    return transform.localScale.x/maxScale;
    //}

    //float GetNutriencePercent()
    //{
    //    return nutrience / maxScale;
    //}
}
