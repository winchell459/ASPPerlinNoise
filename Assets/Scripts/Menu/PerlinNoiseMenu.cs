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

    public Toggle AutoGenerateToggle;

    public void ValueUpdated()
    {
        Generate();
    }
    public void GenerateButton()
    {
        Generate();
    }
    private void Generate()
    {
        SetNoiseScale(GetNoiseScale());
        SetOctaves(GetOctaves());
        SetSeed(GetSeed());
        SetOffset(GetOffset());
        SetMeshHeightMultiplier(GetMeshHeightMultiplier());
        //SetFalloffMode(GetFalloff());


        FindObjectOfType<RaceGameHandler>().GenerateMap(perlinNoiseControls.seed);
    }

    public PerlinNoiseControls perlinNoiseControls;
    public Text /*drawMode, noiseScale, octaves,*/ offsetX, offsetY, seed, fallOff, heightMultiplier;
    public Slider noiseScale, octaves;
    public Sebastian.MapGenerator mapGenerator;

    //public Sebastian.MapGenerator.DrawMode GetDrawMode()
    //{
    //    Sebastian.MapGenerator.DrawMode drawMode = Sebastian.MapGenerator.DrawMode.
    //}

    //public void SetDrawMode(Sebastian.MapGenerator.DrawMode drawMode) {
    //    perlinNoiseControls.drawMode = drawMode;
    //    mapGenerator.drawMode = drawMode;
    //}


    //public void SetUseFlatShading(bool useFlatShading) {
    //    perlinNoiseControls.useFlatShading = useFlatShading;
    //    mapGenerator.useFlatShading = useFlatShading;
    //}
    public float GetNoiseScale()
    {
        return noiseScale.value;
    }
    public void SetNoiseScale(float noiseScale) {
        SetNoiseScale((int)noiseScale);

    }
    public void SetNoiseScale(int noiseScale)
    {
        perlinNoiseControls.noiseScale = noiseScale;
        mapGenerator.noiseScale = noiseScale;
    }

    public float GetOctaves()
    {
        return octaves.value;
    }
    public void SetOctaves(int octaves) {
        perlinNoiseControls.octaves = octaves;
        mapGenerator.octaves = octaves;
    }
    public void SetOctaves(float octaves)
    {
        SetOctaves((int)octaves);
    }

    //public void GetPersistance()
    //{
    //    return 
    //}
    //public void SetPersistance( float persistance) {
    //    perlinNoiseControls.persistance = persistance;
    //    mapGenerator.persistance = persistance; }

    //public void SetLacunarity(float lacunarity) {
    //    perlinNoiseControls.lacunarity = lacunarity;
    //    mapGenerator.lacunarity = lacunarity;
    //}


    public int GetSeed()
    {
        return int.Parse(seed.text);
    }
    public void SetSeed( int seed) {
        perlinNoiseControls.seed = seed;
        mapGenerator.seed = seed;
    }
    public void SetRandomSeed()
    {
        SetSeed(Random.Range(0, 10000));
        seed.text = perlinNoiseControls.seed.ToString();
    }

    public Vector2 GetOffset()
    {
        return new Vector2(float.Parse(offsetX.text), float.Parse(offsetY.text));
    }
    public void SetOffset( Vector2 offset) {
        perlinNoiseControls.offset = offset;
        mapGenerator.offset = offset;
    }

    public void GetFalloff()
    {

    }
    public void SetUseFalloff( bool useFalloff) {
        perlinNoiseControls.useFalloff = useFalloff;
        mapGenerator.useFalloff = useFalloff;
    }

    public void SetFalloffMode( Sebastian.MapGenerator.FalloffMode falloffMode) {
        perlinNoiseControls.falloffMode = falloffMode;
        mapGenerator.falloffMode = falloffMode;
    }

    public void SetFalloffMode()
    {
        string falloffMode = fallOff.text;
        switch (falloffMode)
        {
            case "None":
                SetUseFalloff(false);
                break;

            case "Square":
                SetUseFalloff(true);
                SetFalloffMode(Sebastian.MapGenerator.FalloffMode.Square);
                break;
            case "Circle":
                SetUseFalloff(true);
                SetFalloffMode(Sebastian.MapGenerator.FalloffMode.Circle);
                break;
            case "Noise":
                SetUseFalloff(true);
                SetFalloffMode(Sebastian.MapGenerator.FalloffMode.Noise);
                break;
        }
    }

    public float GetMeshHeightMultiplier()
    {
        return float.Parse(heightMultiplier.text);
    }
    public void SetMeshHeightMultiplier( float meshHeightMultiplier) {
        perlinNoiseControls.meshHeightMultiplier = meshHeightMultiplier;
        mapGenerator.meshHeightMultiplier = meshHeightMultiplier;
    }

    public void SetAutoUpdate( bool autoUpdate)
    {
        perlinNoiseControls.autoUpdate = autoUpdate;
        mapGenerator.autoUpdate = autoUpdate;
    }

    public void AddMeshButton()
    {
        mapGenerator.AddMesh();
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
