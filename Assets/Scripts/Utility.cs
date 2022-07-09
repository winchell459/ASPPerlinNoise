using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public static class Utility 
{
    public static T[,] GetSubArray<T>(T[,] array, int x0, int y0, int x1, int y1)
    {
        int width = x1 - x0 + 1;
        int height = y1 - y0 + 1;
        T[,] subArray = new T[width, height];
        for (int x = 0; x < width; x += 1)
        {
            for (int y = 0; y < height; y += 1)
            {
                subArray[x, y] = array[x + x0, y + y0];
            }
        }
        return subArray;
    }
    public static T[] GetSubArray<T> (T[] array, int i0, int i1)
    {
        int width = i1 - i0 + 1;
        T[] subArray = new T[width];
        for(int i = 0; i < width; i += 1)
        {
            subArray[i] = array[i + i0];
        }
        return subArray;
    }

    public static string CreateFile(string content, string DataFilePath)
    {
        string FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt";
        return CreateFile(content, DataFilePath, FileName);
    }

    public static string CreateFile(string content, string DataFilePath, string filename)
    {
        string relativePath;
        if (Application.isEditor)
        {
            if (!Directory.Exists(Path.Combine("Assets", DataFilePath)))
            {
                Directory.CreateDirectory(Path.Combine("Assets", DataFilePath));
            }
            relativePath = Path.Combine("Assets", DataFilePath, filename);
        }
        else
        {
            if (!Directory.Exists(DataFilePath))
            {
                Directory.CreateDirectory(DataFilePath);
            }
            relativePath = Path.Combine(DataFilePath, filename);
        }

        using (StreamWriter streamWriter = File.CreateText(relativePath))
        {
            streamWriter.Write(content);
        }
        return Path.Combine(DataFilePath, filename);
    }
}
