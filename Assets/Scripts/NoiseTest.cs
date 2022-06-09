using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseTest : MonoBehaviour
{
    public SpriteRenderer spritePrefab;
    public int octaves = 10;
    public double persistence = 0.1;
    private SpriteRenderer[,] map;
    public Renderer renderer;
    public Terrain terrain;

    public int depth = 20;
    public int width = 256, height = 256;
    public float scale = 20;
    public float offsetX, offsetY;
    public bool updatePerlinNoise;
    public bool cubeWorld;
    public GameObject cubePrefab;
    private GameObject[,] cubeTerrain;

    // Start is called before the first frame update
    void Start()
    {
        //map = new SpriteRenderer[256, 256];
        //for (int x = 0; x < 256; x += 1)
        //{
        //    for (int y = 0; y < 256; y += 1)
        //    {
        //        SpriteRenderer sprite = Instantiate(spritePrefab);
        //        sprite.transform.position = new Vector2(x, y);
        //        map[x, y] = sprite;
        //    }
        //}
        if (cubeWorld)
        {
            cubeTerrain = new GameObject[width, height];
            for(int x = 0; x < width; x += 1)
            {
                for(int y = 0; y < height; y += 1)
                {
                    GameObject cube = Instantiate(cubePrefab);
                    cube.transform.position = new Vector3(x, 0, y);
                    cubeTerrain[x, y] = cube;
                }
            }
        }
    }

    private void Update()
    {
        if (updatePerlinNoise)
        {
            renderer.material.mainTexture = GenerateTexture();
            if (cubeWorld)
            {
                float[,] heights = GenerateHeights();
                for (int x = 0; x < width; x += 1)
                {
                    for (int y = 0; y < height; y += 1)
                    {
                        cubeTerrain[x, y].transform.position = new Vector3(x, (int)(heights[x,y] * depth), y);
                    }
                }
            }
            else
            {
                terrain.terrainData = GenerateTerrain(terrain.terrainData);
            }
            
        }
        
    }
    TerrainData GenerateTerrain(TerrainData terrainData)
    {
       
        terrainData.heightmapResolution = width /*+ 1*/;
        terrainData.size = new Vector3(width, depth, height);

        terrainData.SetHeights(0, 0, GenerateHeights());

        return terrainData;
    }

    float[,] GenerateHeights()
    {

        float[,] heights = new float[width, height];
        for(int x = 0; x < width; x += 1)
        {
            for(int y = 0; y < height; y += 1)
            {
                heights[x, y] = CalculateHeight(x, y);
            }
        }
        return heights;
    }


    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);
        for(int x = 0; x < width; x += 1)
        {
            for(int y = 0; y <height; y += 1)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        return texture;
    }

    float CalculateHeight(int x, int y)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale + offsetY;
        float sample = GetPerlinNoise(xCoord, yCoord);
        return sample;
    }

    Color CalculateColor(int x, int y)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale + offsetY;
        float sample = GetPerlinNoise(xCoord, yCoord);
        return new Color(sample, sample, sample);
    }


    Perlin perlin = new Perlin();
    public PerlinNoiseScratch PerlinNoiseScratch;

    public enum PerlinSource
    {
        UnityDefault,
        OctavePerlin,
        Scratch
    }
    public PerlinSource perlinSource;

    float GetPerlinNoise(float xCoord, float yCoord)
    {
        if (perlinSource == PerlinSource.OctavePerlin)
            return (float)perlin.OctavePerlin(xCoord, yCoord, 1, octaves, persistence);
        else if (perlinSource == PerlinSource.UnityDefault)
            return Mathf.PerlinNoise(xCoord, yCoord);
        else
        {
            Debug.Log($"{xCoord} {yCoord}");
            return PerlinNoiseScratch.GetColorAt(xCoord, yCoord, width / PerlinNoiseScratch.UnitSize);
        }
            
    }
    
}
