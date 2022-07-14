using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public static class TrackVerify 
{
    public static System.Action<Track> callBack;

    public static bool[,] CheckTrack(float[,] heightMap, float minY, float maxY, System.Action<Track> callBack)
    {
        TrackVerify.callBack = callBack;

        bool valid = true;

        int height = heightMap.GetLength(1);
        int width = heightMap.GetLength(0);

        node[,] obstacleNodes = new node[width, height];
        node[,] trackNodes = new node[width, height];
        bool[,] trackNodeBools = new bool[width, height];
        for (int x = 0; x < width; x += 1)
        {
            for (int y = 0; y < height; y += 1)
            {
                if(ValidHeight(heightMap[x,y], minY, maxY))
                {
                    node trackNode = new node(new Vector2Int(x,y));

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

        Track track = new Track();
        track.GenerateTrack(heightMap, minY, maxY, trackNodes, obstacleNodes);


        return trackNodeBools;
    }
    

    

    public static bool ValidHeight(float height, float minY, float maxY)
    {
        if (height >= minY && height <= maxY) return true;
        else return false;
    }

    

    public static List<List<node>> GetLoops(node[,] trackNodes)
    {
        List<List<node>> loops = new List<List<node>>();
        List<node> toVisit = GetNodes(trackNodes);
        while(toVisit.Count > 0)
        {
            List<node> loop = new List<node>();
            loops.Add(loop);
            GetNeighbors(toVisit[0], trackNodes, loop, toVisit);
        }
        return loops;
    }
    static void GetNeighbors(node current, node[,] trackNodes, List<node> loop, List<node> toVisit)
    {
        loop.Add(current);
        toVisit.Remove(current);
        if (current.up != null && !loop.Contains(current.up))
        {
            loop.Add(current.up);
            GetNeighbors(current.up, trackNodes, loop, toVisit);
        }
        if (current.right != null && !loop.Contains(current.right))
        {
            loop.Add(current.right);
            GetNeighbors(current.right, trackNodes, loop, toVisit);
        }
        if (current.down != null && !loop.Contains(current.down))
        {
            loop.Add(current.down);
            GetNeighbors(current.down, trackNodes, loop, toVisit);
        }
        if (current.left != null && !loop.Contains(current.left))
        {
            loop.Add(current.left);
            GetNeighbors(current.left, trackNodes, loop, toVisit);
        }
    }

    static List<node> GetNodes(node[,] trackNodes)
    {
        List<node> toVisit = new List<node>();
        for (int x = 0; x < trackNodes.GetLength(0); x += 1)
        {
            for (int y = 0; y < trackNodes.GetLength(1); y += 1)
            {
                if (trackNodes[x, y] != null) toVisit.Add( trackNodes[x, y]);
            }
        }
        return toVisit;
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
    public double buildTime;
    public List<List<node>> track;
    public List<List<node>> obstacles;
    float[,] heightMap;
    float minY, maxY;
    node[,] trackNodes, obstacleNodes;
    System.DateTime buildStart;

    public void GenerateTrack(float[,] heightMap, float minY, float maxY, node[,] trackNodes, node[,] obstacleNodes)
    {
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
                if (TrackVerify.ValidHeight(heightMap[x, y], minY, maxY))
                {
                    node trackNode = trackNodes[x, y];
                    if (y > 0 && TrackVerify.ValidHeight(heightMap[x, y - 1], minY, maxY))
                    {
                        trackNode.up = trackNodes[x, y - 1];
                    }
                    if (x < width - 1 && TrackVerify.ValidHeight(heightMap[x + 1, y], minY, maxY))
                    {
                        trackNode.right = trackNodes[x + 1, y];
                    }
                    if (y < height - 1 && TrackVerify.ValidHeight(heightMap[x, y + 1], minY, maxY))
                    {
                        trackNode.down = trackNodes[x, y + 1];
                    }
                    if (x > 0 && TrackVerify.ValidHeight(heightMap[x - 1, y], minY, maxY))
                    {
                        trackNode.left = trackNodes[x - 1, y];
                    }
                }
                else
                {
                    node obstacleNode = obstacleNodes[x, y];
                    if (y > 0 && !TrackVerify.ValidHeight(heightMap[x, y - 1], minY, maxY))
                    {
                        obstacleNode.up = obstacleNodes[x, y - 1];
                    }
                    if (x < width - 1 && !TrackVerify.ValidHeight(heightMap[x + 1, y], minY, maxY))
                    {
                        obstacleNode.right = obstacleNodes[x + 1, y];
                    }
                    if (y < height - 1 && !TrackVerify.ValidHeight(heightMap[x, y + 1], minY, maxY))
                    {
                        obstacleNode.down = obstacleNodes[x, y + 1];
                    }
                    if (x > 0 && !TrackVerify.ValidHeight(heightMap[x - 1, y], minY, maxY))
                    {
                        obstacleNode.left = obstacleNodes[x - 1, y];
                    }
                }
            }
        }
        Debug.Log("Checking for track " + System.DateTime.Now);
        List<List<node>> loops = TrackVerify.GetLoops(trackNodes);
        Debug.Log("Checking for obstacles " + System.DateTime.Now);
        List<List<node>> obstacles = TrackVerify.GetLoops(obstacleNodes);
        foreach (List<node> loop in loops)
        {
            string loopList = "";
            foreach (node square in loop)
            {
                loopList += square.pos + " ";
            }
            Debug.Log(loopList);
        }

        //Track track = new Track();
        track = loops;
        //obstacles = obstacles;
        buildTime = (System.DateTime.Now - buildStart).TotalSeconds;
        //Debug.Log(System.DateTime.Now + " " + (System.DateTime.Now - buildStart).TotalSeconds + " " + buildTime);
        TrackVerify.callBack(this);
    }

}
