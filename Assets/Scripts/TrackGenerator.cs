using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGenerator : MonoBehaviour
{
    public GameObject trackPrefab;
    public int width = 40, depth = 40;
    private GameObject[,] track;
    [SerializeField] private List<Vector2Int> trackPath = new List<Vector2Int>();
    [SerializeField]private List<Vector2Int> deadEnds = new List<Vector2Int>();
    System.Random rnd;


    Vector2Int start;
    Vector2Int end ;
    Vector2Int current;
    // Start is called before the first frame update
    void Start()
    {
        track = new GameObject[width, depth];
        rnd = new System.Random();
        //GenerateTrack();

        start = new Vector2Int(0, 0);
        end = new Vector2Int(width - 1, depth - 1);
        current = start;
    }
    private void Update()
    {
        
        if (current.x != end.x || current.y != end.y) GenerateTrack();
    }
    void GenerateTrack()
    {
        Debug.Log("GenerateTrack()");
        if(track[current.x, current.y])
        {
            track[current.x, current.y].SetActive(true);
        }
        else
        {
            GameObject chunk = Instantiate(trackPrefab);
            chunk.transform.position = new Vector3(current.x, 0, current.y);
            track[current.x, current.y] = chunk;
        }
        
        

        List<Vector2Int> neighbors = GetNeighbors(current);

        if (neighbors.Count > 0)
        {
            trackPath.Add(current);
            int rand = rnd.Next(0, neighbors.Count);
            current = neighbors[rand];
        }
        else
        {

            track[current.x, current.y].SetActive(false);
            deadEnds.Add(current);
            trackPath.RemoveAt(trackPath.Count - 1);
            
            current = trackPath[trackPath.Count - 1];
            //track[current.x, current.y].SetActive(false);
        }
        //do
        //{
            
        //}
        //while (current.x != end.x && current.y != end.y);
    }

    List<Vector2Int> GetNeighbors(Vector2Int current)
    {

        List<Vector2Int> neighbors = new List<Vector2Int>();
        if (current.x > 0 && track[current.x - 1, current.y] == null && !Contains(new Vector2Int(current.x - 1, current.y))) neighbors.Add(new Vector2Int(current.x - 1, current.y));
        if (current.x < width -1 && track[current.x + 1, current.y] == null && !Contains(new Vector2Int(current.x + 1, current.y))) neighbors.Add(new Vector2Int(current.x + 1, current.y));
        if (current.y > 0 && track[current.x, current.y - 1] == null && !Contains(new Vector2Int(current.x, current.y-1))) neighbors.Add(new Vector2Int(current.x, current.y - 1));
        if (current.y < depth - 1 && track[current.x, current.y+1] == null && !Contains(new Vector2Int(current.x, current.y+1))) neighbors.Add(new Vector2Int(current.x, current.y+1));
        return neighbors;
    }

    bool Contains(Vector2Int section)
    {
        bool contains = false;
        foreach(Vector2Int path in trackPath)
        {
            if (path.x == section.x && path.y == section.y) contains = true;
        }
        return contains;
    }
}
