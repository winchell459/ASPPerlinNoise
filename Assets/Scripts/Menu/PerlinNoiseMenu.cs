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
[System.Serializable]
public class PerlinNoiseControls
{
    public Sebastian.MapGenerator.DrawMode drawMode = Sebastian.MapGenerator.DrawMode.Mesh;

    public bool useFlatShading = true;

    public int mapWidth = 200;
    public int mapHeight = 200;
    public float noiseScale = 25;
    
    public int octaves = 4;
    [Range(0, 1)]
    public float persistance = 0.28f;
    public float lacunarity = 2.09f;


    public int seed = 303;
    public Vector2 offset;

    public bool useFalloff = true;
    
    public Sebastian.MapGenerator.FalloffMode falloffMode;

    public float meshHeightMultiplier = 30;
    public AnimationCurve meshHeightCurve;

    public bool autoUpdate;

    public TerrainType[] regions;
}
