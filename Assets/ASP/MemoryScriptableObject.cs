using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MemoryScriptableObject", menuName = "ASP/MemoryScriptableObject")]
public class MemoryScriptableObject : ScriptableObject
{
    public string id;
    public bool[] data;
    public int width;

    public void SetData(bool[,] data2DArray)
    {
        width = data2DArray.GetLength(0);
        int height = data2DArray.GetLength(1);
        data = new bool[height * width];
        for(int i = 0; i < data.Length; i += 1)
        {
            data[i] = data2DArray[i % width, i / width];
        }
    }

    public string GetASPCode()
    {
        string strMap = "";
        string aspCode = @"

        ";
        int height = data.Length / width;
        
        for(int i = 0; i < data.Length; i += 1)
        {
            if (data[i])
            {
                aspCode += $" tile({i % width + 1},{i / height + 1},track). \n";
                strMap += "1";
            }
            else
            {
                aspCode += $" tile({i % width + 1},{i / height + 1},empty). \n";
                strMap += "0";
            }
            if (i % width == width - 1) strMap += "\n";
        }
        Debug.Log(strMap);
        return aspCode;
    }
}
