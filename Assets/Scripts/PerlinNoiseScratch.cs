using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseScratch : MonoBehaviour
{
    float randlcg;
    [SerializeField] List<float> vectors = new List<float>();
    [SerializeField] float height = 50;
    [SerializeField] float width = 50;
    public GameObject pixel;
    public int UnitSize;
    public bool update = false;
    private GameObject[,] pixelMap;

    
    
    void Start()
    {

        if(update)perlinScratchSetup();
        setup(426257, UnitSize);
    }
    void Update()
    {
        if (update)
        {
            perlinNoise(426257, UnitSize);
            update = false;
        }
    }
    void perlinScratchSetup()
    {
        pixelMap = new GameObject[(int)width, (int)height];
        for (int y = 0; y < height + 1; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject pixelpre = Instantiate(pixel, new Vector2(x, y), transform.rotation);
                pixelMap[x, y] = pixelpre;
            }
        }
    }

    void setup(int seed, int unitSize)
    {
        randlcg = seed;
        vectors.Clear();
        for (int i = 0; i < (width / unitSize + 1) * (height / unitSize + 1); i++)
        {
            vectors.Add(randbetween(0, 360));
        }
    }
    void perlinNoise(int seed, int unitSize)
    {


        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float perlinVal = GetColorAt((float)x / unitSize, (float)y / unitSize, (float)width / unitSize) + 0.2f;
                Color color = new Color(perlinVal * 255, perlinVal * 255, perlinVal * 255);
                pixelMap[x,y].GetComponent<SpriteRenderer>().color = color;
                Debug.Log(perlinVal);
            }
        }
    }

    float randbetween(float min, float max)
    {
        randlcg = ((randlcg * 23256567) + 5746743) % 4563768874;
        return min + ((max - min) * (randlcg / 4563768874));
    }

    public float GetColorAt(float x, float y, float colum)
    {
        int listIndex;
        listIndex = (int)((colum * Mathf.Floor(y)) + Mathf.Floor(x));
        float dotproductA = (Mathf.Cos(vectors[listIndex] * Mathf.Rad2Deg) * (x - Mathf.Floor(x))) + (Mathf.Sin(vectors[listIndex] * Mathf.Rad2Deg) * (y - Mathf.Floor(y)));
        listIndex++;
        float dotproductB = (Mathf.Cos(vectors[listIndex] * Mathf.Rad2Deg) * (x - (Mathf.Floor(x) + 1))) + (Mathf.Sin(vectors[listIndex] * Mathf.Rad2Deg) * (y - Mathf.Floor(y)));
        float lowerVal = interpolate(dotproductA, dotproductB, easeCurve(x % 1));
        listIndex += (int)(colum - 1);
        dotproductA = (Mathf.Cos(vectors[listIndex] * Mathf.Rad2Deg) * (x - Mathf.Floor(x))) + (Mathf.Sin(vectors[listIndex] * Mathf.Rad2Deg) * (y - (Mathf.Floor(y) + 1)));
        listIndex++;
        dotproductB = (Mathf.Cos(vectors[listIndex] * Mathf.Rad2Deg) * (x - (Mathf.Floor(x) + 1))) + (Mathf.Sin(vectors[listIndex] * Mathf.Rad2Deg) * (y - (Mathf.Floor(y) + 1)));
        float upperVal = interpolate(dotproductA, dotproductB, easeCurve(x % 1));
        float perlinval = interpolate(lowerVal, upperVal, easeCurve(y % 1));
        return perlinval;
    }

    float easeCurve(float x)
    {
        return ((x * x * x) * ((x * ((x * 6) - 15)) + 10));
    }

    float interpolate(float a, float b, float t)
    {
        return (a * (1 - t)) + (b * t);
    }

}
