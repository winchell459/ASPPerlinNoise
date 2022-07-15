using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public static class TrackVerify 
{
    //public static System.Action<Track> callBack;

    public static void CheckTrack(float[,] heightMap, float minY, float maxY, System.Action<Track> callBack)
    {
        //TrackVerify.callBack = callBack;

        bool valid = true;

        int height = heightMap.GetLength(1);
        int width = heightMap.GetLength(0);

        

        Track track = new Track();
        track.GenerateTrack(heightMap, minY, maxY, callBack);


        
    }
    

    

    
}
public class node
{
    public Vector2Int pos;
    public node up;
    public node right;
    public node down;
    public node left;

    public node(Vector2Int pos)
    {
        this.pos = pos;
    }
}

public class Track
{
    System.Action<Track> callBack;
    public double buildTime;
    public List<List<node>> track;
    public List<List<node>> obstacles;
    float[,] heightMap;
    float minY, maxY;
    node[,] trackNodes, obstacleNodes;
    System.DateTime buildStart;

    public void GenerateTrack(float[,] heightMap, float minY, float maxY, System.Action<Track> callBack)
    {
        this.callBack = callBack;
        int height = heightMap.GetLength(1);
        int width = heightMap.GetLength(0);
        node[,] obstacleNodes = new node[width, height];
        node[,] trackNodes = new node[width, height];
        bool[,] trackNodeBools = new bool[width, height];

        for (int x = 0; x < width; x += 1)
        {
            for (int y = 0; y < height; y += 1)
            {
                if (ValidHeight(heightMap[x, y], minY, maxY))
                {
                    node trackNode = new node(new Vector2Int(x, y));

                    trackNodes[x, y] = trackNode;
                    trackNodeBools[x, y] = true;
                }
                else
                {
                    node obstacleNode = new node(new Vector2Int(x, y));
                    obstacleNodes[x, y] = obstacleNode;
                }
            }
        }

        this.heightMap = heightMap;
        this.minY = minY;
        this.maxY = maxY;
        this.trackNodes = trackNodes;
        this.obstacleNodes = obstacleNodes;

        Thread thread = new Thread(GenerateTrack);
        thread.Start();
    }

    void GenerateTrack()
    {
        buildStart = System.DateTime.Now ;
        Debug.Log(System.DateTime.Now);
        int height = heightMap.GetLength(1);
        int width = heightMap.GetLength(0);

        for (int x = 0; x < width; x += 1)
        {
            for (int y = 0; y < height; y += 1)
            {
                if (ValidHeight(heightMap[x, y], minY, maxY))
                {
                    node trackNode = trackNodes[x, y];
                    if (y > 0 && ValidHeight(heightMap[x, y - 1], minY, maxY))
                    {
                        trackNode.up = trackNodes[x, y - 1];
                    }
                    if (x < width - 1 &&ValidHeight(heightMap[x + 1, y], minY, maxY))
                    {
                        trackNode.right = trackNodes[x + 1, y];
                    }
                    if (y < height - 1 && ValidHeight(heightMap[x, y + 1], minY, maxY))
                    {
                        trackNode.down = trackNodes[x, y + 1];
                    }
                    if (x > 0 && ValidHeight(heightMap[x - 1, y], minY, maxY))
                    {
                        trackNode.left = trackNodes[x - 1, y];
                    }
                }
                else
                {
                    node obstacleNode = obstacleNodes[x, y];
                    if (y > 0 && !ValidHeight(heightMap[x, y - 1], minY, maxY))
                    {
                        obstacleNode.up = obstacleNodes[x, y - 1];
                    }
                    if (x < width - 1 && !ValidHeight(heightMap[x + 1, y], minY, maxY))
                    {
                        obstacleNode.right = obstacleNodes[x + 1, y];
                    }
                    if (y < height - 1 && !ValidHeight(heightMap[x, y + 1], minY, maxY))
                    {
                        obstacleNode.down = obstacleNodes[x, y + 1];
                    }
                    if (x > 0 && !ValidHeight(heightMap[x - 1, y], minY, maxY))
                    {
                        obstacleNode.left = obstacleNodes[x - 1, y];
                    }
                }
            }
        }
        Debug.Log("Checking for track " + System.DateTime.Now);
        List<List<node>> loops = GetChunks(trackNodes);
        Debug.Log("Checking for obstacles " + System.DateTime.Now);
        //obstacles = GetChunks(obstacleNodes);
        

        //Track track = new Track();
        track = loops;
        //obstacles = obstacles;
        buildTime = (System.DateTime.Now - buildStart).TotalSeconds;
        //Debug.Log(System.DateTime.Now + " " + (System.DateTime.Now - buildStart).TotalSeconds + " " + buildTime);
        callBack(this);
    }

    public static bool ValidHeight(float height, float minY, float maxY)
    {
        if (height >= minY && height <= maxY) return true;
        else return false;
    }



    public static List<List<node>> GetChunks(node[,] trackNodes)
    {
        List<List<node>> chunks = new List<List<node>>();
        //Debug.Log("GetNodes");
        List<node> toVisit = GetNodes(trackNodes);
        int count = 0;
        while (toVisit.Count > 0)
        {
            count++;
            //Debug.Log(count);
            List<node> chunk = new List<node>();
            chunks.Add(chunk);
            GetNeighbors(toVisit[0], trackNodes, chunk, toVisit);
            
        }
        return chunks;
    }
    static void GetNeighbors(node current, node[,] chunkNodes, List<node> chunk, List<node> toVisit)
    {
        chunk.Add(current);
        toVisit.Remove(current);
        //Debug.Log($"GetNeighbors chunk.Count: {chunk.Count} {chunk.Capacity} toVist.Count {toVisit.Count} {toVisit.Capacity}");
        if (current.up != null && !chunk.Contains(current.up))
        {
            chunk.Add(current.up);
            GetNeighbors(current.up, chunkNodes, chunk, toVisit);
        }
        if (current.right != null && !chunk.Contains(current.right))
        {
            chunk.Add(current.right);
            GetNeighbors(current.right, chunkNodes, chunk, toVisit);
        }
        if (current.down != null && !chunk.Contains(current.down))
        {
            chunk.Add(current.down);
            GetNeighbors(current.down, chunkNodes, chunk, toVisit);
        }
        if (current.left != null && !chunk.Contains(current.left))
        {
            chunk.Add(current.left);
            GetNeighbors(current.left, chunkNodes, chunk, toVisit);
        }
        //Debug.Log("GotNeighbors");
    }

    static List<node> GetNodes(node[,] trackNodes)
    {
        List<node> toVisit = new List<node>();
        for (int x = 0; x < trackNodes.GetLength(0); x += 1)
        {
            for (int y = 0; y < trackNodes.GetLength(1); y += 1)
            {
                //Debug.Log($"{x},{y}");
                if (trackNodes[x, y] != null) toVisit.Add(trackNodes[x, y]);
            }
        }
        return toVisit;
    }
}
