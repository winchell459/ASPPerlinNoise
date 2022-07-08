using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sebastian
{
    public static class FalloffGenerator
    {
        public static float[,] GenerateFalloffMap(int size, bool circular)
        {
            float[,] map = new float[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    float x = i / (float)size * 2 - 1;
                    float y = j / (float)size * 2 - 1;

                    float value = 0;
                    if(circular) value = Mathf.Sqrt(x * x + y * y);
                    else value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                     
                    
                    map[i, j] = Evaluate(value);
                }
            }
            return map;
        }

        public static float[,] GenerateFalloffMap(MapGenerator mapSample, Noise.NormalizeMode normalizeMode)
        {
            float[,] map = Noise.GenerateNoiseMap(mapSample.mapWidth, mapSample.mapHeight, mapSample.seed, mapSample.continentNoiseScale, mapSample.continentOctaves, mapSample.continentPersistance, mapSample.continentLacunarity, (mapSample.noiseScale/mapSample.continentNoiseScale) * mapSample.offset + mapSample.continentOffset, normalizeMode);

            
            return map;
        }


        static float Evaluate(float value)
        {
            float a = 3;
            float b = 2.2f;
            return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));
        }
    }
}