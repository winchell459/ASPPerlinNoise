using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public static class Utility 
{
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
